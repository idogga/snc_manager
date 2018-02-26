using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading;
using FFImageLoading.Forms.Droid;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using System;


namespace snc_bonus_operator.Droid
{
    [Activity(Label = "snc_bonus_operator", Icon = "@drawable/ic_main_rect", Theme = "@style/MyTheme", 
        ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const string HockeyAppId = "72666cd78a4d4f02ac37fb4186e4f627";

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            
            try
            {
                CrashManager.Register(this, HockeyAppId);
                MetricsManager.Register(Application, HockeyAppId);
                FeedbackManager.Register(this, HockeyAppId, null);
            }
            catch { }
            global::Xamarin.Forms.Forms.Init(this, bundle);
            MobileStaticVariables.UserAppSettings.LoadSettings();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            CachedImageRenderer.Init();
            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };
            ImageService.Instance.Initialize(config);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            //Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnConfigurationChanged(global::Android.Content.Res.Configuration newConfig)
        {
            Logger.WriteLine("Экран повeрнулся");
            base.OnConfigurationChanged(newConfig);
        }

        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Logger.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Logger.WriteError(new Exception(errorMessage));
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }
    }
}

