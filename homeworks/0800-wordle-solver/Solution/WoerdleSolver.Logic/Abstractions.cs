namespace WoerdleSolver.Logic;

/// <summary>
/// Factory used to create Woerdle solvers
/// </summary>
/// <seealso cref="ISolver"/>
public interface ISolverFactory
{
    /// <summary>
    /// Creates a Woerdle solver (<see cref="ISolver"/>)
    /// </summary>
    ISolver Create();
}

/// <summary>
/// Woerdle solver
/// </summary>
public interface ISolver
{
    /// <summary>
    /// Get all possible words based on the previous guessing results.
    /// </summary>
    /// <seealso cref="StoreResult"/>
    /// <returns>Possible words</returns>
    /// <exception cref="InvalidOperationException">No solution could be found</exception>
    IEnumerable<string> PossibleWords();

    /// <summary>
    /// Stores a guessing result
    /// </summary>
    /// <param name="word">Guessed word</param>
    /// <param name="result">Result from woerdle.at (see remarks for details)</param>
    /// <exception cref="ArgumentException">
    /// Guessed word is not in the allow list (see <see cref="Words.AllowList"/>) 
    /// and not in the solution list (see <see cref="Words.Solutions"/>).
    /// </exception>
    /// <exception cref="ArgumentException">Result contains invalid characters"/></exception>
    /// <remarks>
    /// The <paramref name="result"/> has to contain one of three possibilities per
    /// character: <c>?</c> means that character does not appear in solution.
    /// <c>Y</c> (yellow) means that character is in the solution, but on a
    /// different spot. <c>G</c> (green) means that the character is on the
    /// correct spot.
    /// </remarks>
    void StoreResult(string word, string result);
}
