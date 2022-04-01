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
        var solutions = Words.Solutions.Where(w => 
            // Find word where each letter is in possible values
            w.Select((c, ix) => (c, ix)).All(c => PossibleValues[c.ix].Contains(c.c))
            // Make sure that the word contains all letters that must be in the solution on any spot
            && SomewhereIncluded.All(c => w.Contains(c))).ToArray();

        if (solutions.Length == 0)
        {
            throw new InvalidOperationException("No solution found");
        }

        return solutions;
    }

    /// <inheritdoc />
    public void StoreResult(string word, string result)
    {
        if (!ResultValidationRegex.IsMatch(result))
        {
            throw new ArgumentException("Invalid result", nameof(result));
        }

        if (!Words.AllowList.Contains(word) && !Words.Solutions.Contains(word))
        {
            throw new ArgumentException("Word not in allow list and not in solution list", nameof(word));
        }

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