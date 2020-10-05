using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A default rating view implementation for Android.
    /// </summary>
    public sealed partial class DefaultRatingView
    {
        private const string MarketUri = "market://details?id=";

        internal static void PlatformTryOpenRatingPage(System.Func<bool>? runBeforeOpen = default)
        {
            if (runBeforeOpen != null && !runBeforeOpen())
            {
                return;
            }

            NavigateToAppStorePage();
        }

        internal static async Task PlatformTryOpenRatingPageAsync(System.Func<Task<bool>>? runBeforeOpenAsync = default)
        {
            if (runBeforeOpenAsync != null && !await runBeforeOpenAsync())
            {
                return;
            }

            NavigateToAppStorePage();
        }

        private static void NavigateToAppStorePage()
        {
            if (Application.Context == null)
            {
                return;
            }

            var intent = new Intent(Intent.ActionView, Uri.Parse($"{MarketUri}{Application.Context.PackageName}"));

            var flags = ActivityFlags.ClearTop | ActivityFlags.NewTask;

            if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.N)
            {
                flags |= ActivityFlags.LaunchAdjacent;
            }

            intent.SetFlags(flags);

            try
            {
                Application.Context.StartActivity(intent);
            }
            catch
            {
            }
        }
    }
}
