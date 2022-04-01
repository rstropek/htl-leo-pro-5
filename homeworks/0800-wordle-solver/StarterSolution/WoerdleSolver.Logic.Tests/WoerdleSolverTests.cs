using Xunit;

namespace WoerdleSolver.Logic.Tests;

public class WoerdleSolverTests
{
    [Theory]
    [InlineData("A????")]
    [InlineData("????")]
    public void StoreResultInvalidResult(string res)
    {
        var solver = new Solver();
        Assert.Throws<ArgumentException>("result", () => solver.StoreResult("DUMMY", res));
        Assert.Throws<ArgumentException>("result", () => solver.StoreResult("DUMMY", res));
    }

    /// <summary>
    /// Ensures that <see cref="ArgumentException"/> is thrown if a word is guessed
    /// that is not in the allow list (see <see cref="Words.AllowList"/>) 
    /// and not in the solution list (see <see cref="Words.Solutions"/>)
    /// (e.g. "YXYXY").
    /// </summary>
    [Fact]
    public void StoreResultInvalidWord()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void StoreResult()
    {
        var solver = new Solver();
        solver.StoreResult("APFEL", "GY???");

        // First spot was guessed correctly -> single possible value left
        Assert.Single(solver.PossibleValues[0]);
        Assert.Equal('A', solver.PossibleValues[0].First());

        // Second letter is in solution, but on different spot
        Assert.DoesNotContain('P', solver.PossibleValues[1]);

        // Third letter is not in solution
        Assert.True(Enumerable.Range(1, 4).All(ix => "FEL".All(c => !solver.PossibleValues[ix].Contains(c))));
    }

    /// <summary>
    /// Ensures that the result "?????" is processed correctly.
    /// </summary>
    /// <remarks>
    /// Remember: "?" means that the letter does not appear in the solution at all.
    /// </remarks>
    [Fact]
    public void StoreResultNothingCorrect()
    {
        var solver = new Solver();
        solver.StoreResult("APFEL", "?????");

        // Todo: Add assertations here
        throw new NotImplementedException();
    }

    /// <summary>
    /// Ensures that the result "GGGGG" is processed correctly.
    /// </summary>
    /// <remarks>
    /// Remember: "G" means that the letter is in the right spot
    /// </remarks>
    [Fact]
    public void StoreResultEverythingCorrect()
    {
        var solver = new Solver();
        solver.StoreResult("APFEL", "GGGGG");

        // Todo: Add assertations here
        throw new NotImplementedException();
    }

    [Fact]
    public void PossibleWordsAll()
    {
        var solver = new Solver();
        Assert.Equal(Words.Solutions, solver.PossibleWords());
    }

    [Fact]
    public void PossibleWords()
    {
        var solver = new Solver();
        solver.StoreResult("ZAPFE", "?YYYY");

        var possibleWords = solver.PossibleWords().ToArray();
        Assert.Single(possibleWords);
        Assert.Equal("APFEL", possibleWords[0]);
    }

    /// <summary>
    /// Ensures that the result "GGGGG" leads to a single possible word
    /// </summary>
    [Fact]
    public void PossibleWordsAllCorrect()
    {
        var solver = new Solver();
        solver.StoreResult("APFEL", "GGGGG");

        var possibleWords = solver.PossibleWords().ToArray();

        // Todo: Add assertations here
        throw new NotImplementedException();
    }

    /// <summary>
    /// Ensures that an exception is thrown if there are no possible words
    /// </summary>
    [Fact]
    public void PossibleWordsNoSolution()
    {
        var solver = new Solver();
        solver.StoreResult("APFEL", "GGGG?");

        // Todo: Add assertations here
        throw new NotImplementedException();
    }
}
