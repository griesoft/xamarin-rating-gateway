using System;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <inheritdoc/>
    public sealed partial class DefaultRatingView : IRatingView
    {
        /// <inheritdoc/>
        public void TryOpenRatingPage() => PlatformTryOpenRatingPage();

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
