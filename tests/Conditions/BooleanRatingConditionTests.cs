using System;
using System.Collections.Generic;
using System.Text;
using Griesoft.Xamarin.RatingGateway.Conditions;
using Xunit;

namespace RatingGateway.Tests.Conditions
{
    public class BooleanRatingConditionTests
    {
        [Fact]
        public void ManipulateState_Should_RevertCurrentState()
        {
            // Arrange
            var condition = new BooleanRatingCondition();

            // Act
            condition.ManipulateState();

            // Assert
            Assert.True(condition.CurrentState);
        }


        [Fact]
        public void IsConditionMet_EvaluatesTo_ExpectedResult()
        {
            // Arrange
            var condition = new BooleanRatingCondition();

            // Act
            condition.ManipulateState();

            // Assert
            Assert.True(condition.IsConditionMet);
        }
    }
}
