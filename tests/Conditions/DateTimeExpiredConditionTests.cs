using System;
using Xunit;

namespace Griesoft.Xamarin.RatingGateway.Tests.Conditions
{
    public class DateTimeExpiredConditionTests
    {
        [Theory]
        [InlineData(-1, true)]
        [InlineData(1, false)]
        public void IsConditionMet_EvaluatesTo_ExpectedResult(int days, bool expected)
        {
            // Arrange
            var condition = new DateTimeExpiredCondition(TimeSpan.FromDays(days));

            // Act


            // Assert
            Assert.Equal(expected, condition.IsConditionMet);
        }
    }
}
