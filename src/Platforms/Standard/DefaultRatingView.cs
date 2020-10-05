using System;
using System.Threading.Tasks;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "We need to match the signature in each platform project, even that the parameter would never be used.")]
        internal static void PlatformTryOpenRatingPage(Func<bool>? runBeforeOpen = default) =>
            throw new NotImplementedException("Install this package on each platform project that you intent to use this package with.");


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "We need to match the signature in each platform project, even that the parameter would never be used.")]
        internal static Task PlatformTryOpenRatingPageAsync(Func<Task<bool>>? runBeforeOpenAsync = default) =>
            throw new NotImplementedException("Install this package on each platform project that you intent to use this package with.");
    }
}
