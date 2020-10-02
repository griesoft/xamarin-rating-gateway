using Android.App;

namespace Griesoft.Xamarin.RatingGateway.Helpers
{
    internal static partial class CacheHelpers
    {
        static string? PlatformAppDataDirectory
            => Application.Context.FilesDir?.AbsolutePath;
    }
}
