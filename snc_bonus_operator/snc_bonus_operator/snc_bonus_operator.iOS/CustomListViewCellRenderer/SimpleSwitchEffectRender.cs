using Xamarin.Forms;
using UIKit;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("SNCEffects")]
[assembly: ExportEffect(typeof(snc_bonus_operator.iOS.CustomListViewCellRenderer.SimpleSwitchEffect), "SimpleSwitchEffect")]
namespace snc_bonus_operator.iOS.CustomListViewCellRenderer
{
    public class SimpleSwitchEffect : PlatformEffect
    {
        Xamarin.Forms.Color xfMainColor = (Xamarin.Forms.Color)snc_bonus_operator.App.Current.Resources["MainColor"];

        protected override void OnAttached()
        {
            (Control as UISwitch).OnTintColor = xfMainColor.ToUIColor();
        }

        protected override void OnDetached()
        {
        }
    }
}
