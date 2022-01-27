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

    private static CalculationParameters GetExample6Parameters(CarType carType) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 27, 8, 45, 0),
                    Leaving: new(2021, 12, 27, 15, 15, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
                new(
                    Entering: new(2021, 12, 27, 17, 10, 0),
                    Leaving: new(2021, 12, 27, 18, 30, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            DayToCalculate: new(2021, 12, 27),
            carType,
            IsElectricOrHybrid: false);

    private static CalculationParameters GetExample7Parameters(DateTime dayToCalculate) =>
        new(
            new Trip[]
            {
                new(
                    Entering: DateTime.Today.AddDays(-2) + new TimeSpan(13, 10, 0),
                    Leaving: null,
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            dayToCalculate,
            CarType.PassengerCar,
            IsElectricOrHybrid: false);

    private static CalculationParameters GetExample8Parameters(CarType carType) =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 27, 13, 10, 0),
                    Leaving: new(2021, 12, 27, 13, 40, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            DayToCalculate: new(2021, 12, 27),
            carType,
            IsElectricOrHybrid: false);

    private static CalculationParameters GetExample9Parameters() =>
        new(
            new Trip[]
            {
                new(
                    Entering: new(2021, 12, 27, 13, 10, 0),
                    Leaving: new(2021, 12, 27, 14, 00, 0),
                    DetectionsInside: Array.Empty<DateTime>()),
            },
            DayToCalculate: new(2021, 12, 27),
            CarType.PassengerCar,
            IsElectricOrHybrid: false);
}
