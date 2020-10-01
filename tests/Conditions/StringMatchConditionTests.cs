using Griesoft.Xamarin.RatingGateway.Conditions;
using Xunit;

namespace RatingGateway.Tests.Conditions
{
    public class StringMatchConditionTests
    {
        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "", false)]
        [InlineData("", null, true)] // Initially meets condition, empty goal should be avoided
        [InlineData("test", "tes", false)]
        public void IsConditionMet_EvaluatesTo_ExpectedResult(string goal, string? param, bool expected)
        {
            // Arrange
            var condition = new StringMatchCondition(goal);

            // Act
            if (param != null)
            {
                condition.ManipulateState(param);
            }

            // Assert
            Assert.Equal(expected, condition.IsConditionMet);
        }
    }
}
