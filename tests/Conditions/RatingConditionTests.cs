using Griesoft.Xamarin.RatingGateway.Conditions;
using Xunit;
namespace RatingGateway.Tests.Conditions
{
    public class RatingConditionTests
    {
        [Fact]
        public void ManipulateState_InvalidCast_IsIgnored()
        {
            // Arrange
            var condition = new RatingCondition<int>(0, i => i > 10);

            // Act
            condition.ManipulateState("test");

            // Assert
            Assert.Equal(0, condition.CurrentState);
        }

        [Fact]
        public void ManipulateState_Cast_IsSuccessful()
        {
            // Arrange
            var condition = new RatingCondition<int>(0, i => i > 10);

            // Act
            condition.ManipulateState(3);

            // Assert
            Assert.Equal(3, condition.CurrentState);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void Reset_IsEpxpectedValue(int initial)
        {
            // Arrange
            var condition = new RatingCondition<int>(initial, i => i > 10);

            // Act
            condition.ManipulateState(5);
            condition.Reset();

            // Assert
            Assert.Equal(initial, condition.CurrentState);
        }
    }
}
