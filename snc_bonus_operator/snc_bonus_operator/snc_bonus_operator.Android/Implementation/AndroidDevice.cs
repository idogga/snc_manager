using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(snc_bonus_operator.Droid.Implementation.UniqueIdAndroid))]
namespace snc_bonus_operator.Droid.Implementation
{
    public class UniqueIdAndroid : snc_bonus_operator.Interfaces.IDevice
    {

        public string GetIdentifier()
        {
            var result = "";
            global::Android.Telephony.TelephonyManager tm;
            tm = (global::Android.Telephony.TelephonyManager)Android.App.Application.Context.GetSystemService(global::Android.Content.Context.TelephonyService);
#pragma warning disable CS0618 // Type or member is obsolete
            result = tm.DeviceId;
#pragma warning restore CS0618 // Type or member is obsolete
            if (result == null || result == "")
            {
                result = global::Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, global::Android.Provider.Settings.Secure.AndroidId);
            }
            return result;
        }

        public string GetVersion()
        {
            var result = "";
            var context = Android.App.Application.Context;
            result = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            return result;
        }

        public double CalculateWidth(string text)
        {
            Rect bounds = new Rect();
            TextView textView = new TextView(Android.App.Application.Context);
            textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
            var length = bounds.Width();
            return length / Resources.System.DisplayMetrics.ScaledDensity;
        }

        public void CloseApp()
        {
            Process.KillProcess(Process.MyPid());
        }

        public void SetMaxBrightness()
        {
            var layout = (Android.App.Application.Context as Activity).Window.Attributes;
            layout.ScreenBrightness = 1f;
            (Android.App.Application.Context as Activity).Window.Attributes = layout;
        }

        public void SetAutoBrightness()
        {
            var layout = (Android.App.Application.Context as Activity).Window.Attributes;
            layout.ScreenBrightness = -1f;
            (Android.App.Application.Context as Activity).Window.Attributes = layout;
        }
    }
}