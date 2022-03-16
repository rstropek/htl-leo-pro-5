using Xunit;

namespace Mathler.Solver.Tests;

public class PuzzleSolverTests
{
    [Fact]
    public void Guess()
    {
        for (var tryCount = 0; tryCount < 100; tryCount++)
        {
            var evaluator = new FormulaEvaluator();
            var solver = new PuzzleSolver(42, evaluator);
            var formula = solver.Guess();
            Assert.Equal(42, evaluator.Evaluate(formula));
        }
    }

    [Fact]
    public void Guess_Invalid_Result()
    {
        var solver = new PuzzleSolver(44, new FormulaEvaluator());
        Assert.Throws<ArgumentException>(() => solver.StoreResult("11+33", "??X??"));
    }

    [Fact]
    public void Guess_One_Known()
    {
        var solver = new PuzzleSolver(44, new FormulaEvaluator());
        solver.StoreResult("11+33", "??G??");
        var formula = solver.Guess();
        Assert.Equal('+', formula[2]);
    }

    [Fact]
    public void Guess_One_Somewhere()
    {
        var solver = new PuzzleSolver(44, new FormulaEvaluator());
        solver.StoreResult("11+33", "Y????");
        var formula = solver.Guess();
        Assert.Contains('1', formula);
    }
}
