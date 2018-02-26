using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Login;
using snc_bonus_operator.Settings;
using System;

using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class App : Application
    {
        bool isNeedToContinuePing = true;
        public App()
        {
            InitializeComponent();
            Logger.WriteLine("App InitializeComponent");
            try
            {
                MobileStaticVariables.MainIssuer.LoadDictionary();
                if ((MobileStaticVariables.UserAppSettings.LastUpdate.DayOfYear == DateTime.Now.DayOfYear) && (MobileStaticVariables.UserAppSettings.LastUpdate.Year == DateTime.Now.Year))
                {
                    if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] != null)
                    {
                        MobileStaticVariables.UserStatus = UserStatusEnum.Unlogged;
                        MainPage = new RootPage();
                    }
                    else
                    {
                        MobileStaticVariables.UserStatus = UserStatusEnum.UnRegister;
                        MainPage = new NavigationPage(new LoginPage());
                    }
                }
                else
                {
                    MainPage = new NavigationPage(new LoadPage());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                MainPage = new NavigationPage(new LoadPage());
            }
        }

        protected override void OnStart()
        {
            Logger.WriteLine("App : Выполнение OnStart");
            base.OnStart();
            isNeedToContinuePing = true;
            Device.StartTimer(new TimeSpan(0, 0, 0, 5), MakePing);
        }

        protected override void OnSleep()
        {
            Logger.WriteLine("App : Выполнение OnSleep");
            base.OnSleep();
            MobileStaticVariables.UserInfo.LastAuthorise = DateTime.Now;
            isNeedToContinuePing = false;
        }

        protected override void OnResume()
        {
            Logger.WriteLine("App : Выполнение OnResume");
            base.OnResume();
            if (MobileStaticVariables.UserInfo.LastAuthorise < DateTime.Now.AddMinutes(-10) && MobileStaticVariables.UserStatus == UserStatusEnum.Logged)
            {
                MobileStaticVariables.UserStatus = UserStatusEnum.Unlogged;
                //MainPage = new NavigationPage(new AuthPage(AuthPageState.Authorization));
            }
            isNeedToContinuePing = true;
            Device.StartTimer(new TimeSpan(0, 0, 0, 5), MakePing);
        }

        bool MakePing()
        {
            MobileStaticVariables.UserAppSettings.IsInetAvaliable =  DependencyService.Get<INetUtils>().IsServerPing(MobileStaticVariables.ConectSettings.Certificates[0].IP);
            return isNeedToContinuePing;
        }
    }
}
