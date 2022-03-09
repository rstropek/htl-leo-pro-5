using System;
using Xunit;

namespace Mathler.Solver.Tests;

public class PuzzleSolverTests
{
    [Theory]
    [InlineData((float)12 + 34, "12+34")]
    [InlineData((float)45 - 44, "45-44")]
    [InlineData((float)67 * 89, "67*89")]
    [InlineData((float)240 / 2, "240/2")]
    public void Evaluate(int expectedResult, string formula)
    {
        Assert.Equal(expectedResult, PuzzleSolver.Evaluate(formula));
    }

    [Theory]
    [InlineData("12+3")]
    [InlineData("-4+44")]
    [InlineData("04+44")]
    [InlineData("67*a9")]
    [InlineData("24021")]
    public void EnsureValidFormula(string formula)
    {
        Assert.False(PuzzleSolver.IsValidFormula(formula));
    }

    [Fact]
    public void Guess()
    {
        for (var tryCount = 0; tryCount < 100; tryCount++)
        {
            var solver = new PuzzleSolver(42);
            var formula = solver.Guess();
            Assert.Equal(42, PuzzleSolver.Evaluate(formula));
        }
    }

    [Fact]
    public void Guess_Invalid_Result()
    {
        var solver = new PuzzleSolver(44);
        Assert.Throws<ArgumentException>(() => solver.StoreResult("11+33", "??X??"));
    }

    [Fact]
    public void Guess_One_Known()
    {
        var solver = new PuzzleSolver(44);
        solver.StoreResult("11+33", "??G??");
        var formula = solver.Guess();
        Assert.Equal('+', formula[2]);
    }

    [Fact]
    public void Guess_One_Somewhere()
    {
        var solver = new PuzzleSolver(44);
        solver.StoreResult("11+33", "Y????");
        var formula = solver.Guess();
        Assert.Contains('1', formula);
    }
}
