using Android.Graphics;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("SNCEffects")]
[assembly: ExportEffect(typeof(snc_bonus_operator.Droid.Renders.SimpleSwitchEffectAndroid), "SimpleSwitchEffect")]
namespace snc_bonus_operator.Droid.Renders
{
    public class SimpleSwitchEffectAndroid : PlatformEffect
    {
        Xamarin.Forms.Color xfMainColor = (Xamarin.Forms.Color)snc_bonus_operator.App.Current.Resources["MainColor"];
        Xamarin.Forms.Color xfSelectColor = (Xamarin.Forms.Color)snc_bonus_operator.App.Current.Resources["SelectionColor"];
        Xamarin.Forms.Color xfBackgroundColor = (Xamarin.Forms.Color)snc_bonus_operator.App.Current.Resources["ObjectBackgroundColor"];

        private global::Android.Support.V7.Widget.SwitchCompat _thisSwitch;

        public SimpleSwitchEffectAndroid()
        { }

        protected override void OnAttached()
        {
            try
            {
                _thisSwitch = Control as global::Android.Support.V7.Widget.SwitchCompat;
                if (_thisSwitch != null)
                {
                    ChangeColor();
                    _thisSwitch.CheckedChange += MySwitch_CheckedChange;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void MySwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                ((Xamarin.Forms.Switch)Element).IsToggled = _thisSwitch.Checked;
                ChangeColor();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        protected override void OnDetached()
        {
            try
            {
                _thisSwitch.CheckedChange -= MySwitch_CheckedChange;
                _thisSwitch = null;
            }
            catch { }
        }

        private void ChangeColor()
        {
            if (_thisSwitch.Checked)
            {
                _thisSwitch.ThumbDrawable.SetColorFilter(xfMainColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                _thisSwitch.TrackDrawable.SetColorFilter(xfSelectColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
            else
            {
                _thisSwitch.TrackDrawable.SetColorFilter(global::Android.Graphics.Color.LightGray, PorterDuff.Mode.SrcAtop);
                _thisSwitch.ThumbDrawable.SetColorFilter(global::Android.Graphics.Color.Gray, PorterDuff.Mode.SrcAtop);
            }
        }
    }
}