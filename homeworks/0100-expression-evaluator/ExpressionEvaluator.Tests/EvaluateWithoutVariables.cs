using Xunit;

namespace ExpressionEvaluator.Tests
{
    public class EvaluateWithoutVariables
    {
        [Theory]
        [InlineData(0, "")]
        [InlineData(42, "42")]
        [InlineData(42, "21+21")]
        [InlineData(42, "63-21")]
        [InlineData(42, "10+50+2-20")]
        public void EmptyStringResultsInZero(int result, string expression)
        {
            Assert.Equal(result, new ExpressionEvaluation.ExpressionEvaluator().Evaluate(expression));
        }
    }
}