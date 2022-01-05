using static CityCongestionCharge.Data.ChargeCalculator;

namespace CityCongestionCharge.Data.Tests;

public partial class TestChargeCalculator
{
    private static CalculationParameters GetExample1Parameters(CarType carType) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 28, 8, 30, 0),
                    Leaving: new(2021, 12, 28, 16, 15, 0),
                    DetectionsInside: Array.Empty<DateTime>())
            },
            DayToCalculate: new(2021, 12, 28),
            carType,
            IsElectricOrHybrid: false);

    private static CalculationParameters GetExample2Parameters(DateTime dayToCalculate) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 27, 15, 45, 0),
                    Leaving: new(2021, 12, 31, 14, 15, 0),
                    DetectionsInside: new DateTime[]
                    {
                        new(2021, 12, 29, 9, 15, 0),
                        new(2021, 12, 30, 16, 45, 0),
                        new(2021, 12, 31, 8, 45, 0),
                    }),
            },
            dayToCalculate,
            CarType.PassengerCar,
            IsElectricOrHybrid: false);

    private static CalculationParameters GetExample3Parameters(DateTime dayToCalculate) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2022, 1, 1, 17, 0, 0),
                    Leaving: new(2022, 1, 2, 1, 30, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            dayToCalculate,
            CarType.PassengerCar,
            IsElectricOrHybrid: false);

    private static CalculationParameters GetExample4Parameters(CarType carType) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 28, 8, 30, 0),
                    Leaving: new(2021, 12, 28, 16, 15, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            DayToCalculate: new(2021, 12, 28),
            carType,
            IsElectricOrHybrid: true);

    private static CalculationParameters GetExample5Parameters(DateTime dayToCalculate) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 27, 12, 30, 0),
                    Leaving: new(2021, 12, 31, 14, 15, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            dayToCalculate,
            CarType.PassengerCar,
            IsElectricOrHybrid: true);

    // Todo: Add parameter generation functions for the test cases 6 to 9.
    //       Implement your functions based on the same priciples as shown above.
}
