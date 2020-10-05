using System;

namespace Griesoft.Xamarin.RatingGateway
{
    public sealed partial class DefaultRatingView : IRatingView
    {
        private readonly Func<bool>? _runBeforeOpen;

        /// <summary>
        /// 
        /// </summary>
        public DefaultRatingView() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runBeforeOpen">A last chance function that be used to intercept navigating to the review page of your app.</param>
        /// <remarks>
        /// <paramref name="runBeforeOpen"/> will be only called if the In-App Review process is not available, i.e. just before we would open the Store page of your app.
        /// This can be useful to first prompt the user to review your app by opening a pop-up. Not doing so can have a negative impact on user experience.
        /// </remarks>
        public DefaultRatingView(Func<bool> runBeforeOpen)
        {
            _runBeforeOpen = runBeforeOpen;
        }

        /// <inheritdoc/>
        public void TryOpenRatingPage() => PlatformTryOpenRatingPage(_runBeforeOpen);

        internal static Version ParseVersion(string versionString)
        {
            if (Version.TryParse(versionString, out var version))
            {
                return version;
            }

            if (int.TryParse(versionString, out var major))
            {
                return new Version(major, 0);
            }

            return new Version(0, 0);
        }
    }
}
