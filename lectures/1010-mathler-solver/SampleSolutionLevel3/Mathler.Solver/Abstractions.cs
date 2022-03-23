namespace Mathler.Solver;

/// <summary>
/// Factory used to create puzzle solvers
/// </summary>
/// <seealso cref="IPuzzleSolver"/>
public interface IPuzzleSolverFactory
{
    /// <summary>
    /// Creates a puzzle solver (<see cref="IPuzzleSolver"/>)
    /// </summary>
    /// <param name="expectedResult">Expected result for the puzzle solver</param>
    IPuzzleSolver Create(int expectedResult);
}

/// <summary>
/// Mathler puzzle solver
/// </summary>
public interface IPuzzleSolver
{
    /// <summary>
    /// Get a random guess based on the previous guessing results <see cref="StoreResult"/>
    /// </summary>
    /// <returns>Guess</returns>
    /// <exception cref="InvalidOperationException">No solution could be found</exception>
    string Guess();

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
    void StoreResult(string formula, string result);
}

/// <summary>
/// Evaluates and validates formulas
/// </summary>
public interface IFormulaEvaluator
{
    /// <summary>
    /// Calculate result of given formula
    /// </summary>
    /// <param name="formula">Formula to evaulate</param>
    /// <returns>Result of formula</returns>
    /// <exception cref="ArgumentException">Invalid formula</exception>
    float Evaluate(string formula);

    /// <summary>
    /// Verifies that formula is valid.
    /// </summary>
    /// <param name="formula">Formula to validate</param>
    /// <returns>
    /// <c>true</c> if formula is valid, otherwise <c>false</c>.
    /// </returns>
    bool IsValidFormula(string formula);
}
