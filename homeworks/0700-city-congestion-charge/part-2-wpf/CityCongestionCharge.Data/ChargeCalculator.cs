using System.Diagnostics.CodeAnalysis;

namespace CityCongestionCharge.Data;

public class ChargeCalculator
{
    /// <summary>
    /// Represents a single trip
    /// </summary>
    /// <param name="Entering">Timestamp when the car entered the city</param>
    /// <param name="Leaving">Timestamp when the car left the city</param>
    /// <param name="DetectionsInside">Timestamps between <paramref name="Entering"/> 
    ///     and <paramref name="Leaving"/> when the car has been detected driving inside the city
    /// </param>
    /// <remarks>
    /// Note that <paramref name="Leaving"/> can be <c>null</c>. In that case, the trip is not over
    /// (i.e. the car is still inside the city).
    /// </remarks>
    public record Trip(DateTime Entering, DateTime? Leaving, IEnumerable<DateTime> DetectionsInside);

    /// <summary>
    /// Parameters for fee calculation
    /// </summary>
    /// <param name="Trips">Data of the trips relevant for the day to calculate</param>
    /// <param name="CarType">Car type for which the fee has to be calculated</param>
    /// <param name="DayToCalculate">Day for which the fee has to be calculated</param>
    /// <param name="IsElectricOrHybrid">Indicates whether the car was an EV or HEV</param>
    public record CalculationParameters(IEnumerable<Trip> Trips, DateTime DayToCalculate, CarType CarType, bool IsElectricOrHybrid);


    /// <summary>
    /// Calculate the charge for a trip.
    /// </summary>
    /// <param name="parameters">Calculation parameters</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if fee cannot be calculated. This is the case of <paramref name="dayToCalculate"/>
    /// is today and the trip has not been completed (i.e. car has not left the city yet).
    /// </exception>
    /// <returns>Total fee for the given day</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "For dependency injection")]
    public decimal CalculateFee(CalculationParameters parameters)
    {
        // Check if fee can be calculated
        if (parameters.Trips.Any(t => !t.Leaving.HasValue && parameters.DayToCalculate.Date == DateTime.Today))
        {
            throw new InvalidOperationException("Fee cannot be calculated. " +
                "Day to calculate is today and the trip has not been completed " +
                "(i.e. car has not left the city yet).");
        }

        var baseFee = 0m;
        var ccFee = 0m;
        foreach (var trip in parameters.Trips)
        {
            // Calculate base fee for fossile fuel vehicles
            if (!parameters.IsElectricOrHybrid)
            {
                var tripStart = trip.Entering;
                if (trip.Entering < parameters.DayToCalculate)
                {
                    // Trip started before the day to calculate -> use midnight of day to calculate as start
                    tripStart = parameters.DayToCalculate;
                }

                var tripEnd = trip.Leaving;
                if (tripEnd.HasValue && tripEnd >= parameters.DayToCalculate.AddDays(1))
                {
                    // Trip ended after the day to calculate -> use midnight of next day after day to calculate as end
                    tripEnd = parameters.DayToCalculate.AddDays(1).AddHours(-1);
                }

                // Calculate base fee
                baseFee += (tripEnd?.Hour ?? 23) + 1 - tripStart.Hour;
            }
        }

        // Check if driving detections happend during rush hours
        // Helper function for findout out if given time is in rush hour
        static bool IsInRushHour(DateTime d) =>
            (d.TimeOfDay >= new TimeSpan(7, 30, 0) && d.TimeOfDay < new TimeSpan(10, 0, 0))
            || (d.TimeOfDay >= new TimeSpan(15, 30, 0) && d.TimeOfDay < new TimeSpan(18, 0, 0));

        if (parameters.Trips.Any(
            // Driven in city
            trip => (trip.DetectionsInside.Any(IsInRushHour)
            // Entered city during rush hour on the day to calculate
            || (trip.Entering.Date == parameters.DayToCalculate && IsInRushHour(trip.Entering))
            // Left city during rush hour on the day to calculate
            || (trip.Leaving.HasValue && trip.Leaving.Value.Date == parameters.DayToCalculate && IsInRushHour(trip.Leaving.Value)))
            // No weekend
            && parameters.DayToCalculate.DayOfWeek != DayOfWeek.Saturday
            && parameters.DayToCalculate.DayOfWeek != DayOfWeek.Sunday))
        {
            ccFee += 3m;
        }

        // Calculate total fee
        var totalFee = (baseFee + ccFee) * parameters.CarType switch
        {
            CarType.PassengerCar => 1m,
            CarType.Lorry or CarType.Van => 1.5m,
            CarType.Motorcycle => 0.5m,
            _ => throw new InvalidOperationException("Invalid car type; this should never happen!")
        };

        // Return minimum of max. daily fee and calculated fee
        return Math.Min(parameters.CarType switch
        {
            CarType.PassengerCar => 20m,
            CarType.Lorry or CarType.Van => 30m,
            CarType.Motorcycle => 10m,
            _ => throw new InvalidOperationException("Invalid car type; this should never happen!")
        }, totalFee);
    }
}
