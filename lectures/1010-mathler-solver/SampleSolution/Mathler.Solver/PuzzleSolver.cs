using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Mathler.Solver.Tests")]

namespace Mathler.Solver;

public class PuzzleSolver
{
    private readonly List<char>[] PossibleValues;
    private readonly List<char> SomewhereIncluded = new();
    private readonly int ExpectedResult;
    private static readonly Regex ValidationRegex = new(@"^(0|([1-9][0-9]*))[\+\-\*\/](0|([1-9][0-9]*))$", RegexOptions.Compiled);
    private static readonly Regex ResultValidationRegex = new(@"^[\?GY]{5}$", RegexOptions.Compiled);

    public PuzzleSolver(int expectedResult)
    {
        var digitsWithoutZero = Enumerable.Range(1, 9).Select(n => (char)('0' + n));
        var digits = new[] { '0' }.Concat(digitsWithoutZero);
        var digitsAndOperators = digits.Concat(new [] { '+', '-', '*', '/' });
        PossibleValues = new[]
        {
            digitsWithoutZero.ToList(),
            digitsAndOperators.ToList(),
            digitsAndOperators.ToList(),
            digitsAndOperators.ToList(),
            digitsAndOperators.ToList(),
        };
        ExpectedResult = expectedResult;
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
        if (!IsValidFormula(formula))
        {
            throw new ArgumentException("Invalid formula", nameof(formula));
        }

        if (!ResultValidationRegex.IsMatch(result))
        {
            throw new ArgumentException("Invalid result", nameof(result));
        }

        foreach (var (letter, ix) in result.Select((l, i) => (l, i)))
        {
            switch (letter)
            {
                case '?':
                    // The digit/operator does not appear in solution
                    foreach(var pv in PossibleValues.Where(pv => pv.Count > 1))
                    {
                        pv.Remove(formula[ix]);
                    }

                    break;
                case 'G':
                    // The digit/operator is on the correct spot
                    PossibleValues[ix].Clear();
                    PossibleValues[ix].Add(formula[ix]);
                    SomewhereIncluded.Remove(formula[ix]);
                    break;
                case 'Y':
                    // The digit/operator is in the solution, but on a different spot
                    PossibleValues[ix].Remove(formula[ix]);
                    if (!SomewhereIncluded.Contains(formula[ix]))
                    {
                        SomewhereIncluded.Add(formula[ix]);
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// Get a random guess based on the previous guessing results <see cref="StoreResult"/>
    /// </summary>
    /// <returns>Guess</returns>
    /// <exception cref="InvalidOperationException">No solution could be found</exception>
    public string Guess()
    {
        // Try multiple times to find a random guess
        for (var tryCount = 0; tryCount < 100_000; tryCount++)
        {
            var formula = string.Create(5, (object?)null, (buf, _) =>
            {
                var rand = new Random();

                // Set known letters
                foreach (var (pv, ix) in PossibleValues.Select((l, i) => (l, i)).Where(pv => pv.l.Count == 1))
                {
                    buf[ix] = pv[0];
                }

                // Add somewhere included
                foreach (var inc in SomewhereIncluded)
                {
                    while (true)
                    {
                        var ix = rand.Next(5);
                        if (buf[ix] == '\0')
                        {
                            buf[ix] = inc;
                            break;
                        }
                    }
                }

                // Set other letter to random values
                for(var i = 0; i < buf.Length; i++)
                {
                    if (buf[i] == '\0')
                    {
                        buf[i] = PossibleValues[i][rand.Next(PossibleValues[i].Count)];
                    }
                }
            });

            try
            {
                // Verify that generated formula is valid
                if (IsValidFormula(formula))
                {
                    var result = Evaluate(formula);

                    // Check that result is an integer and equal to expected result
                    if (Math.Floor(result) == result && (int)result == ExpectedResult)
                    {
                        // Found a solution
                        return formula;
                    }
                }
            }
            catch (DivideByZeroException)
            { 
                // Ignore divisions by zero. In this case we just generate a new formula.
            }
        }

        throw new InvalidOperationException("Could not find result");
    }

    /// <summary>
    /// Calculate result of given formula
    /// </summary>
    /// <param name="formula">Formula to evaulate</param>
    /// <returns>Result of formula</returns>
    /// <exception cref="ArgumentException">Invalid formula</exception>
    public static float Evaluate(string formula)
    {
        if (!IsValidFormula(formula))
        {
            throw new ArgumentException("Invalid formula", nameof(formula));
        }

        int? left = null;
        char? op = null;
        var val = 0;
        foreach (var letter in formula)
        {
            switch (letter)
            {
                case >= '0' and <= '9':
                    val *= 10;
                    val += letter - '0';
                    break;
                case '+' or '-' or '/' or '*':
                    left = val;
                    op = letter;
                    val = 0;
                    break;
                default:
                    throw new InvalidOperationException("This should never happen");
            }
        }

        var right = val;
#pragma warning disable CS8629 // Op cannot be null, ensured by regex
        return op.Value switch
        {
            '+' => (float)left.Value + right,
            '-' => (float)left.Value - right,
            '*' => (float)left.Value * right,
            '/' => (float)left.Value / right,
            _ => throw new InvalidOperationException("This should never happen")
        };
#pragma warning restore CS8629
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
        return formula.Length == 5 && ValidationRegex.IsMatch(formula);
    }
}

