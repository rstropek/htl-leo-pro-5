using Xunit;

namespace Mathler.Solver.Tests;

public class FormulaEvaluatorTests
{
    [Theory]
    [InlineData((float)12 + 34, "12+34")]
    [InlineData((float)45 - 44, "45-44")]
    [InlineData((float)67 * 89, "67*89")]
    [InlineData((float)240 / 2, "240/2")]
    public void Evaluate(int expectedResult, string formula)
    {
        Assert.Equal(expectedResult, new FormulaEvaluator().Evaluate(formula));
    }

    [Theory]
    [InlineData("12+3")]
    [InlineData("-4+44")]
    [InlineData("04+44")]
    [InlineData("67*a9")]
    [InlineData("24021")]
    public void EnsureValidFormula(string formula)
    {
        Assert.False(new FormulaEvaluator().IsValidFormula(formula));
    }
}
