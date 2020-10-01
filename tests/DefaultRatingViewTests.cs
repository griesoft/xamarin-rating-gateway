using System;
using Xunit;

namespace Griesoft.Xamarin.RatingGateway.Tests
{
    public class DefaultRatingViewTests
    {
        [Fact]
        public void TryOpenRatingPage_Throws_OnNetStandard()
        {
            // Arrange
            var ratingView = new DefaultRatingView();

            // Act


            // Assert
            Assert.Throws<NotImplementedException>(() => ratingView.TryOpenRatingPage());
        }
    }
}
