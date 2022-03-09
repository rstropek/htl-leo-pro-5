using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mathler.Solver.Tests")]

namespace Mathler.Solver;

public class PuzzleSolver
{
    public PuzzleSolver(int expectedResult)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Stores a guessing result
    /// </summary>
    /// <param name="formula">Guessed formula</param>
    /// <param name="result">Result from mathler (see remarks for details)</param>
    /// <exception cref="ArgumentException">Invalid argument</exception>
    /// <remarks>
    /// The <paramref name="result"/> has to contain one of three possibilities per
    /// character: <c>?</c> means that character does not appear in solution.
    /// <c>Y</c> (yellow) means that character is in the solution, but on a
    /// different spot. <c>G</c> (green) means that the character is on the
    /// correct spot.
    /// </remarks>
    public void StoreResult(string formula, string result)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a random guess based on the previous guessing results <see cref="StoreResult"/>
    /// </summary>
    /// <returns>Guess</returns>
    /// <exception cref="InvalidOperationException">No solution could be found</exception>
    public string Guess()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculate result of given formula
    /// </summary>
    /// <param name="formula">Formula to evaulate</param>
    /// <returns>Result of formula</returns>
    /// <exception cref="ArgumentException">Invalid formula</exception>
    public static float Evaluate(string formula)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifies that formula is valid.
    /// </summary>
    /// <param name="formula">Formula to validate</param>
    /// <returns>
    /// <c>true</c> if formula is valid, otherwise <c>false</c>.
    /// </returns>
    public static bool IsValidFormula(string formula)
    {
        throw new NotImplementedException();
    }
}

