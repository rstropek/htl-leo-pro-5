using Xunit;

namespace ExpressionEvaluator.Tests
{
    public class EvaluateWithVariables
    {
        [Theory]
        [InlineData(0, "", "")]
        [InlineData(42, "x", "x=42")]
        [InlineData(42, "21+x", "x=21")]
        [InlineData(42, "x-21", "x=63")]
        [InlineData(42, "10+x+2-y", "x=50,y=20")]
        public void EmptyStringResultsInZero(int result, string expression, string variableStrings)
        {
            var variables = variableStrings.Split(',')
                .Where(v => !string.IsNullOrEmpty(v))
                .Select(v => v.Split('=').ToArray())
                .ToDictionary(v => v[0], v => int.Parse(v[1]));

            Assert.Equal(result, new ExpressionEvaluation.ExpressionEvaluator().Evaluate(expression, variables));
        }

        [Fact]
        public void UnknownVariable()
        {
            Assert.Throws<ArgumentException>(() => new ExpressionEvaluation.ExpressionEvaluator().Evaluate("x", new Dictionary<string, int>()));
        }
    }
}