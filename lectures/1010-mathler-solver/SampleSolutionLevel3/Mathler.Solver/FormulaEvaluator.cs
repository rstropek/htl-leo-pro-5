using System.Text.RegularExpressions;

namespace Mathler.Solver;

public class FormulaEvaluator : IFormulaEvaluator
{
    private static readonly Regex ValidationRegex = new(@"^(0|([1-9][0-9]*))[\+\-\*\/](0|([1-9][0-9]*))$", RegexOptions.Compiled);

    /// <inheritdoc />
    public float Evaluate(string formula)
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

    /// <inheritdoc />
    public bool IsValidFormula(string formula)
    {
        return formula.Length == 5 && ValidationRegex.IsMatch(formula);
    }
}

