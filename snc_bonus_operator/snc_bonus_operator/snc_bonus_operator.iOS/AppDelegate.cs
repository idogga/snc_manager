using FFImageLoading;
using Foundation;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace snc_bonus_operator.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        private const string HockeyAppId = "8f30ffee2d6b4708a1bdf9dee57d3414";

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            //Xamarin.FormsGoogleMaps.Init("AIzaSyC0lIZYXAdCsDvdb7c4SSCetdM6SDFoJlY");
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
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

            FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
        {
                new NotificationUserCategory("message",new List<NotificationUserAction> {
                    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
                }),
                new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept"),
                    new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
                })

        });

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Logger.WriteLine(granted.ToString());
                });
                UNUserNotificationCenter.Current.Delegate = this;
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge;
                // register for remote notifications
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
            else
            {
                // iOS 9 <=
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
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

            Forms.Init();
            MobileStaticVariables.UserAppSettings.LoadSettings();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        #region notification
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            Logger.WriteLine($"ошибка при получение уведомления: {error.DebugDescription}");
        }
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);
            Logger.WriteLine("Колличество : " + userInfo["gcm.message_id"]);
            Logger.WriteLine("Перечисление : " + userInfo["aps"]);
            Logger.WriteLine("Значения : " + userInfo["alert"]);
            Logger.WriteLine("Тело : " + NSDictionary.FromObjectAndKey(NSObject.FromObject("фыв"), NSObject.FromObject("ФЫВ")));
            completionHandler(UIBackgroundFetchResult.NewData);
        }
        #endregion

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
            base.WillTerminate(application);
        }
    }
}
