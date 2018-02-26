using Newtonsoft.Json;
using Plugin.DeviceInfo;
using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace snc_bonus_operator
{
    public delegate void InfoLoadDelegate();
    public partial class LoadPage : ContentPage
    {
        public static event InfoLoadDelegate IssuersLoadEvent;
        public static event InfoLoadDelegate SupportLoadEvent;
        public static event InfoLoadDelegate CardLoadEvent;
        public static event InfoLoadDelegate AzsLoadEvent;

        bool isLimitationLoad = false, isIssueLoad = false, isAzsLoad = false;
        bool _isDatabaseLoad = false;
        bool needDisplayAlert = true;

        public LoadPage()
        {
            InitializeComponent();
            logoImage.WidthRequest = 200;
            _isDatabaseLoad = MobileStaticVariables.UserAppSettings.IsDataBaseLoad;
            PageLoad();
        }

        async void PageLoad()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            await Task.Factory.StartNew(async x =>
            {

                await LoadMainIssuer();
                await LoadLimitation();
            },
            TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 10), LoadAnotherData);
            LoadRoot();
        }

        bool LoadAnotherData()
        {
            Task.Factory.StartNew(() =>
            {
                if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] != null)
                {
                    LoadDeviceInfo();
                }
            }, CancellationToken.None);

            return false;
        }

        async Task LoadMainIssuer()
        {
            try
            {
                Logger.WriteLine("LoadIssues : start");

                Device.BeginInvokeOnMainThread(delegate
                {
                    descriptionLabel.Text = (_isDatabaseLoad) ? "Обновление данных о клубе" : "Загрузка данных о клубе";
                });

                string issuer = "";
                try
                {
                    issuer =
                        MobileStaticVariables.WebUtils.SendIssuerRequest("MainIssuerSeller", "ru");
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (needDisplayAlert)
                        {
                            needDisplayAlert = false;
                            await DisplayAlert("Внимание", "Не удалось загрузить данные о компании", "Повторить");
                        }
                    });
                    Logger.WriteError(ex);
                    throw ex;
                }
                Logger.WriteLine("issuer : " + issuer);
                if (issuer == "")
                    throw new Exception("Пустая строка");
                Issuer deserialized = new Issuer();
                if (issuer != "")
                    deserialized = JsonConvert.DeserializeObject<Issuer>(issuer);

                isIssueLoad = true;
                MobileStaticVariables.MainIssuer = deserialized;
                MobileStaticVariables.MainIssuer.LoadDictionary();

                needDisplayAlert = true;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.LastUpdate, DateTime.Now.ToString());
                Logger.WriteLine("LoadIssues : end");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                if (!_isDatabaseLoad)
                    await LoadMainIssuer();
            }
        }

        // TODO Сделать разделение по приложениям
        async Task LoadLimitation()
        {
            try
            {
                Logger.WriteLine("LoadLimitation : start");
                Device.BeginInvokeOnMainThread(delegate
                {
                    descriptionLabel.Text = "Загрузка ограничений";
                });
                string limit = "";

                try
                {
                    limit = MobileStaticVariables.WebUtils.SendSystemRequest("Limitations", "");
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                    throw ex;
                }
                Logger.WriteLine("limit : " + limit);
                if (limit == "")
                    throw new Exception("Не удалось получить limit");

                Limitation deserialized = new Limitation();
                if (limit != "")
                    deserialized = JsonConvert.DeserializeObject<Limitation>(limit);
                deserialized.LoadLimitation();
                isLimitationLoad = true;
                Logger.WriteLine("LoadLimitation : end");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                if (!_isDatabaseLoad)
                {
                    if(needDisplayAlert)
                    {
                        needDisplayAlert = false;
                        await DisplayAlert("Внимание", "Не удалось подключиться", "Продолжить");
                    }
                    await LoadLimitation();
                }
            }
        }

        void LoadDeviceInfo()
        {
            string deviceInfo = "";
            try
            {
                DeviceInfo info = new DeviceInfo();
                info.DeviceName = string.Format("{0} {1} {2}",
                    CrossDeviceInfo.Current.Model, CrossDeviceInfo.Current.Platform, CrossDeviceInfo.Current.Version);

                info.Imei = DependencyService.Get<IDevice>().GetIdentifier();
                info.AppVersion = DependencyService.Get<IDevice>().GetVersion();
                info.DeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                info.UserID = MobileStaticVariables.UserInfo.MobileUserKey;
                //info.NotificationToken = CrossFirebasePushNotification.Current.Token;
                deviceInfo = MobileStaticVariables.WebUtils.SendMobileInfoRequest("SetDevice",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(info))));
                var deviceInfo1 = new DeviceInfo();
                deviceInfo1 = JsonConvert.DeserializeObject<DeviceInfo>(deviceInfo);
                deviceInfo1.Parse(deviceInfo);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        void LoadRoot()
        {
            Logger.WriteLine("LoadRoot : start");
            if (isLimitationLoad)
            {
                MobileStaticVariables.UserAppSettings.IsDataBaseLoad = true;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsDataBaseLoad, true.ToString());
                MobileStaticVariables.UserAppSettings.LastUpdate = DateTime.Now;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.LastUpdate, MobileStaticVariables.UserAppSettings.LastUpdate.ToString());
            }
            Logger.WriteLine("LoadRoot : end");
            if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] != null)
            {
                MobileStaticVariables.UserStatus = UserStatusEnum.Unlogged;
                App.Current.MainPage = new RootPage();
            }
            else
            {
                MobileStaticVariables.UserStatus = UserStatusEnum.UnRegister;
                App.Current.MainPage = new NavigationPage(new Login.LoginPage());
            }
        }
    }
}