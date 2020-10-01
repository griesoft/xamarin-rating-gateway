using Griesoft.Xamarin.RatingGateway.Conditions;
using Xunit;

namespace Griesoft.Xamarin.RatingGateway.Tests.Conditions
{
    public class CountRatingConditionTests
    {
        [Fact]
        public void ManipulateState_Should_BumpCurrentStateByOne()
        {
            // Arrange
            var condition = new CountRatingCondition(0, 10);

            // Act
            condition.ManipulateState();

            // Assert
            Assert.Equal(1, condition.CurrentState);
        }

        [Theory]
        [InlineData(0, 1, null, true)]
        [InlineData(0, 2, null, false)]
        [InlineData(0, 4, 4, true)]
        [InlineData(0, 5, 8, true)]
        [InlineData(4, 5, null, true)]
        [InlineData(8, 5, 4, false)]
        public void IsConditionMet_EvaluatesTo_ExpectedResult(int initial, int goal, int? param, bool expected)
        {
            // Arrange
            var condition = new CountRatingCondition(initial, goal);

            // Act
            if (param == null)
            {
                condition.ManipulateState();
            }
            else
            {
                condition.ManipulateState(param);
            }

            // Assert
            Assert.Equal(expected, condition.IsConditionMet);
        }
    }
}
