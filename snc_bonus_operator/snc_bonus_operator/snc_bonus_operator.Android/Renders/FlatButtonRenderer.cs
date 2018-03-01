using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using Android.Content;

[assembly: ExportRenderer(typeof(Button), typeof(snc_bonus_operator.Droid.Renders.FlatButtonRenderer))]

namespace snc_bonus_operator.Droid.Renders
{
    public class FlatButtonRenderer : ButtonRenderer
    {
        public FlatButtonRenderer(Context context) : base(context)
        {

        }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
        }
    }
}