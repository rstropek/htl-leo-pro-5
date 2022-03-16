using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Mathler.Solver.Tests")]

namespace Mathler.Solver;

public class PuzzleSolverFactory : IPuzzleSolverFactory
{
    private readonly IFormulaEvaluator formulaEvaluator;

    public PuzzleSolverFactory(IFormulaEvaluator formulaEvaluator)
    {
        this.formulaEvaluator = formulaEvaluator;
    }

    /// <inheritdoc />
    public IPuzzleSolver Create(int expectedResult) => new PuzzleSolver(expectedResult, formulaEvaluator);
}

public class PuzzleSolver : IPuzzleSolver
{
    private readonly List<char>[] PossibleValues;
    private readonly List<char> SomewhereIncluded = new();
    private readonly int ExpectedResult;
    private readonly IFormulaEvaluator formulaEvaluator;
    private static readonly Regex ResultValidationRegex = new(@"^[\?GY]{5}$", RegexOptions.Compiled);

    public PuzzleSolver(int expectedResult, IFormulaEvaluator formulaEvaluator)
    {
        var digitsWithoutZero = Enumerable.Range(1, 9).Select(n => (char)('0' + n));
        var digits = new[] { '0' }.Concat(digitsWithoutZero);
        var digitsAndOperators = digits.Concat(new[] { '+', '-', '*', '/' });
        PossibleValues = new[]
        {
            digitsWithoutZero.ToList(),
            digitsAndOperators.ToList(),
            digitsAndOperators.ToList(),
            digitsAndOperators.ToList(),
            digitsAndOperators.ToList(),
        };
        ExpectedResult = expectedResult;
        this.formulaEvaluator = formulaEvaluator;
    }

    /// <inheritdoc />
    public void StoreResult(string formula, string result)
    {
        if (!formulaEvaluator.IsValidFormula(formula))
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
                    foreach (var pv in PossibleValues.Where(pv => pv.Count > 1))
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

    /// <inheritdoc />
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
                for (var i = 0; i < buf.Length; i++)
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
                if (formulaEvaluator.IsValidFormula(formula))
                {
                    var result = formulaEvaluator.Evaluate(formula);

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
}
