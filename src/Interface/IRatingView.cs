namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// The interface for a view which is responsible to prompt the user to review the application.
    /// </summary>
    /// <remarks>
    /// By default the rating gateway uses the default implementation <see cref="DefaultRatingView"/>, but you can also create
    /// your own implementation for one or multiple platforms that you are using this package on.
    /// </remarks>
    public interface IRatingView
    {
        /// <summary>
        /// Try to open the review page for your application.
        /// </summary>
        void TryOpenRatingPage();
    }
}
