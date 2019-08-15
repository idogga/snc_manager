using Newtonsoft.Json;
using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using DeviceInfo = snc_bonus_operator.Protocol.DeviceInfo;

namespace snc_bonus_operator
{
    public delegate void InfoLoadDelegate();
    public partial class LoadPage : ContentPage
    {

        bool isLimitationLoad = false;
        bool _isDatabaseLoad = false;
        bool needDisplayAlert = true;
#if DEBUGARTYOM
        int numberOfConnection = 0;
#endif
        public LoadPage()
        {
            InitializeComponent();
            logoImage.WidthRequest = 200;
            _isDatabaseLoad = MobileStaticVariables.UserAppSettings.IsDataBaseLoad;
            PageLoad();
        }

        async void PageLoad()
        {
            await Task.Factory.StartNew(async () =>
            {

                await LoadMainIssuer();
                await LoadLimitation();
            });
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
                            await DisplayAlert("Внимание", "Не удалось загрузить данные о клубе", "Повторить");
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
                {
#if DEBUGARTYOM
                    numberOfConnection++;
                    switch (numberOfConnection)
                    {
                        case 1:
                            MobileStaticVariables.ConectSettings.DebugPort = 2581;
                            break;
                        case 2:
                            MobileStaticVariables.ConectSettings.DebugPort = 2580;
                            break;
                        case 3:
                            MobileStaticVariables.ConectSettings.DebugPort = 2582;
                            break;
                        case 4:
                            MobileStaticVariables.ConectSettings.DebugPort = 2585;
                            break;
                        default:
                            numberOfConnection = 0;
                            break;
                    }
#endif
                    await LoadMainIssuer();
                }
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