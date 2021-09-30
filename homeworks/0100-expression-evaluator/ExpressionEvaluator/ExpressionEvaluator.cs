namespace ExpressionEvaluation;

internal interface IExpressionEvaluator
{
    int Evaluate(ReadOnlySpan<char> expressionString, IReadOnlyDictionary<string, int>? variables = null);
}

internal class ExpressionEvaluator : IExpressionEvaluator
{
    public int Evaluate(ReadOnlySpan<char> expressionString, IReadOnlyDictionary<string, int>? variables = null)
    {
        var result = 0;
        while (expressionString.Length > 0)
        {
            var operation = 1;
            if (expressionString[0] is '+' or '-')
            {
                operation = expressionString[0] switch
                {
                    '+' => 1,
                    '-' => -1,
                    _ => throw new InvalidOperationException()
                };

                expressionString = expressionString[1..];
            }

            var nextOperator = expressionString.IndexOfAny("+-");
            if (nextOperator == -1)
            {
                nextOperator = expressionString.Length;
            }

            var valueString = expressionString[..nextOperator];
            if (int.TryParse(valueString, out var parsedValue))
            {
                result += parsedValue * operation;
            }
            else
            {
                var variableName = valueString.ToString();
                if (variables == null || !variables.ContainsKey(variableName))
                {
                    throw new ArgumentException($"Variable {valueString} not found", nameof(expressionString));
                }

                result += variables[variableName] * operation;
            }

            expressionString = expressionString[nextOperator..];
        }

        return result;
    }
}