using FFImageLoading;
using FFImageLoading.Forms.Touch;
using Foundation;
using Plugin.Toasts;
using System;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace snc_bonus_operator.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const string HockeyAppId = "8f30ffee2d6b4708a1bdf9dee57d3414";

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Logger.WriteLine("FinishedLaunching start");
            //Подписывание HockeyApp
            //         var manager = BITHockeyManager.SharedHockeyManager;
            //manager.Configure(HockeyAppId);
            //manager.DisableMetricsManager = true;
            //manager.StartManager();
            //manager.Authenticator.AuthenticateInstallation();

            //Подписывание считыватель пальцев
            //CrossFingerprint.AllowReuse = false;
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
            // Загрузка уведомлений
            DependencyService.Register<ToastNotification>(); // Register your dependency
            ToastNotification.Init();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request Permissions
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
                {
                    // Do something if needed
                });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                 UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                    );

                app.RegisterUserNotificationSettings(notificationSettings);
            }
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            global::Xamarin.Forms.Forms.Init();
            MobileStaticVariables.UserAppSettings.LoadSettings();
            LoadApplication(new App());
            Logger.WriteLine("FinishedLaunching end");
            return base.FinishedLaunching(app, options);
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {

            // Local Notifications are received here
        }

        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }


        public override void OnResignActivation(UIApplication application)
        {
            Logger.WriteLine("OnResignActivation");
        }

        public override void DidEnterBackground(UIApplication application)
        {
            //FirebasePushNotificationManager.Disconnect();
            Logger.WriteLine("OnResignActivation");
        }

        public override void WillEnterForeground(UIApplication application)
        {
            Logger.WriteLine("OnResignActivation");
        }

        public override void OnActivated(UIApplication application)
        {
            //FirebasePushNotificationManager.Connect();
            Logger.WriteLine("OnResignActivation");
        }

        public override void WillTerminate(UIApplication application)
        {
            Logger.WriteLine("OnResignActivation");
        }
    }
}
