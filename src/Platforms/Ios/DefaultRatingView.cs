using System;
using Foundation;
using StoreKit;
using UIKit;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A default rating view implementation for iOS.
    /// </summary>
    public sealed partial class DefaultRatingView
    {
        private const string AppStoreReviewUrl = "itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=";
        private const string AppStoreReviewUrlIOS7 = "itms-apps://itunes.apple.com/app/id";
        private const string AppStoreReviewUrlIOS8 = "itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id=YOUR_APP_ID&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software";

        internal static void PlatformTryOpenRatingPage(Func<bool>? runBeforeOpen = default)
        {
            var systemVersion = ParseVersion(UIDevice.CurrentDevice.SystemVersion);

            if (systemVersion >= new Version(10, 3))
            {
                SKStoreReviewController.RequestReview();
            }
            else if (runBeforeOpen != null && !runBeforeOpen())
            {
                // Don't do anything if the runBeforeOpen function returned false
            }
            else if (systemVersion >= new Version(8, 0))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(AppStoreReviewUrlIOS8.Replace("YOUR_APP_ID", NSBundle.MainBundle.BundleIdentifier)));
            }
            else if (systemVersion >= new Version(7, 0))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"{AppStoreReviewUrlIOS7}{NSBundle.MainBundle.BundleIdentifier}"));
            }
            else
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"{AppStoreReviewUrl}{NSBundle.MainBundle.BundleIdentifier}"));
            }
        }
    }
}
