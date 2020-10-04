using System;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A default rating view implementation for .NET Standard.
    /// </summary>
    /// <remarks>
    /// If the package is not installed on each platform that it is used on, this will throw a <see cref="NotImplementedException"/>.
    /// </remarks>
    public sealed partial class DefaultRatingView
    {
        internal static void PlatformTryOpenRatingPage() =>
            throw new NotImplementedException("Install this package on each platform project that you intent to use this package with.");
    }
}
