using System.Text.RegularExpressions;

namespace WoerdleSolver.Logic;

public class SolverFactory : ISolverFactory
{
    /// <inheritdoc />
    public ISolver Create() => new Solver();
}

public class Solver : ISolver
{
    internal readonly HashSet<char>[] PossibleValues;
    private readonly HashSet<char> SomewhereIncluded = new();
    private static readonly Regex ResultValidationRegex = new(@"^[\?GY]{5}$", RegexOptions.Compiled);

    public Solver()
    {
        PossibleValues = Enumerable.Range(0, 5).Select(_ =>
            Enumerable.Range(0, 26).Select(n => (char)('A' + n)).ToHashSet()).ToArray();
    }

    /// <inheritdoc />
    public IEnumerable<string> PossibleWords()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void StoreResult(string word, string result)
    {
        // Todo: Add parameter validation here
        #warning Parameter validation missing, add it!

        foreach (var (letter, ix) in result.Select((l, i) => (l, i)))
        {
            switch (letter)
            {
                case '?':
                    // The letter does not appear in solution
                    foreach (var pv in PossibleValues.Where(pv => pv.Count > 1))
                    {
                        pv.Remove(word[ix]);
                    }

                    break;
                case 'G':
                    // The letter is on the correct spot
                    PossibleValues[ix].Clear();
                    PossibleValues[ix].Add(word[ix]);
                    SomewhereIncluded.Remove(word[ix]);
                    break;
                case 'Y':
                    // The letter is in the solution, but on a different spot
                    PossibleValues[ix].Remove(word[ix]);
                    if (!SomewhereIncluded.Contains(word[ix]))
                    {
                        SomewhereIncluded.Add(word[ix]);
                    }

                    break;
            }
        }
    }
}