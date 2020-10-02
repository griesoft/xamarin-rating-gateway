using Foundation;

namespace Griesoft.Xamarin.RatingGateway.Helpers
{
    internal static partial class CacheHelpers
    {
        static string? PlatformAppDataDirectory
            => GetDirectory(NSSearchPathDirectory.LibraryDirectory);

        static string? GetDirectory(NSSearchPathDirectory directory)
        {
            var dirs = NSSearchPath.GetDirectories(directory, NSSearchPathDomain.User);
            if (dirs == null || dirs.Length == 0)
            {
                // this should never happen...
                return null;
            }
            return dirs[0];
        }
    }
}
