namespace MyFirstUnitTests;

public class TestBasicMath
{
    [Fact]
    public void Add()
    {
        var result = BasicMath.Add(1, 2);
        Assert.Equal(3, result);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1000, 2000, 3000)]
    public void AddTheory(int x, int y, int expectedResult)
    {
        var result = BasicMath.Add(x, y);
        Assert.Equal(expectedResult, result);
    }
}
