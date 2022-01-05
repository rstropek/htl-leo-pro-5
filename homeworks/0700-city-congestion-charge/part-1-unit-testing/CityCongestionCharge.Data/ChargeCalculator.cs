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
        throw new NotImplementedException("You have to implement this logic");
    }
}
