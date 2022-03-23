using Xunit;

namespace Mathler.Solver.Tests;

public class PuzzleSolverFactoryTests
{
    [Fact]
    public void Create()
    {
        Assert.IsAssignableFrom<IPuzzleSolver>(new PuzzleSolverFactory(new FormulaEvaluator()).Create(42));
    }
}
