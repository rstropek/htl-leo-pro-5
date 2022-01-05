using Xunit;

namespace CityCongestionCharge.Data.Tests;

public partial class TestChargeCalculator
{
    [Theory]
    [InlineData(CarType.PassengerCar, 12)]
    [InlineData(CarType.Lorry, 18)]
    [InlineData(CarType.Motorcycle, 6)]
    public void Example1(CarType carType, decimal expectedFee)
    {
        Assert.Equal(expectedFee, new ChargeCalculator().CalculateFee(GetExample1Parameters(carType)));
    }

    [Fact]
    public void Example2()
    {
        Assert.Equal(12m, new ChargeCalculator().CalculateFee(GetExample2Parameters(new DateTime(2021, 12, 27))));
        Assert.Equal(20m, new ChargeCalculator().CalculateFee(GetExample2Parameters(new DateTime(2021, 12, 28))));
        Assert.Equal(20m, new ChargeCalculator().CalculateFee(GetExample2Parameters(new DateTime(2021, 12, 29))));
        Assert.Equal(20m, new ChargeCalculator().CalculateFee(GetExample2Parameters(new DateTime(2021, 12, 30))));
        Assert.Equal(18m, new ChargeCalculator().CalculateFee(GetExample2Parameters(new DateTime(2021, 12, 31))));
    }

    [Fact]
    public void Example3()
    {
        Assert.Equal(7m, new ChargeCalculator().CalculateFee(GetExample3Parameters(new DateTime(2022, 1, 1))));
        Assert.Equal(2m, new ChargeCalculator().CalculateFee(GetExample3Parameters(new DateTime(2022, 1, 2))));
    }

    [Theory]
    [InlineData(CarType.PassengerCar, 3)]
    [InlineData(CarType.Lorry, 4.5)]
    [InlineData(CarType.Motorcycle, 1.5)]
    public void Example4(CarType carType, decimal expectedFee)
    {
        Assert.Equal(expectedFee, new ChargeCalculator().CalculateFee(GetExample4Parameters(carType)));
    }

    [Fact]
    public void Example5()
    {
        Assert.Equal(0m, new ChargeCalculator().CalculateFee(GetExample5Parameters(new DateTime(2021, 12, 27))));
        Assert.Equal(0m, new ChargeCalculator().CalculateFee(GetExample5Parameters(new DateTime(2021, 12, 28))));
        Assert.Equal(0m, new ChargeCalculator().CalculateFee(GetExample5Parameters(new DateTime(2021, 12, 29))));
        Assert.Equal(0m, new ChargeCalculator().CalculateFee(GetExample5Parameters(new DateTime(2021, 12, 30))));
        Assert.Equal(0m, new ChargeCalculator().CalculateFee(GetExample5Parameters(new DateTime(2021, 12, 31))));
    }

    [Fact]
    public void Todo()
    {
        // Todo: Replace this test with test for test cases 6 to 9.
        //       Implement your unit tests based on the same priciples as shown above.

        throw new NotImplementedException("Implement unit tests for test cases 6 to 9");
    }
}
