using System.Threading.Tasks;
using Android.Support.Design.Widget;
using Android.Views;
using Griesoft.Xamarin.RatingGateway;

namespace RatingGatewaySamples.Droid
{
    public class SampleRatingView : IRatingView
    {
        private readonly View _view;

        public SampleRatingView(View view)
        {
            _view = view;
        }

        public void TryOpenRatingPage()
        {
            Snackbar.Make(_view, "Opened rating page.", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public Task TryOpenRatingPageAsync()
        {
            Snackbar.Make(_view, "Opened rating page async.", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();

            return Task.CompletedTask;
        }
    }
}
