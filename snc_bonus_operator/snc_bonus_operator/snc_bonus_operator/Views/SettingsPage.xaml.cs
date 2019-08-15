using Newtonsoft.Json;
using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Text;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class SettingsPage : ContentPage
    {
        #region Конструктор
        public SettingsPage()
        {
            InitializeComponent();
            statusLanguageLabel.Text = "Руcский";
            Logger.WriteLine("настройки: уведомления " + MobileStaticVariables.UserAppSettings.ShowNotifications.ToString()
                + ", \nколличество запусков приложения: " + MobileStaticVariables.UserAppSettings.NumberOfLoadApp +
                "\nзагружена ли БД : " + MobileStaticVariables.UserAppSettings.IsDataBaseLoad);
            
            useVibrationSwitch.IsToggled = MobileStaticVariables.UserAppSettings.UseVibration;
#if DEBUG
            var connectionButton = new Button() { HorizontalOptions = LayoutOptions.FillAndExpand };
            connectionButton.Text = "Выберите подключение (" + MobileStaticVariables.ConectSettings.DebugPort + ")";
            connectionButton.Clicked += async (s, e) =>
             {
                 var connectionWay = new string[] { "Свой", "Артём(2582)", "Андрей(2580)", "Женя(2581)", "Сервер(2585)"};
                 var answer = await DisplayActionSheet("Выберите далеко подключаться", "Отмена", "", connectionWay);
                 if(answer != null)
                 {
                     if(answer== connectionWay[0])
                     {
                         var label = new Label() { Text = "Введите порт (нажмите подтвердить что бы продожить):" };
                         var entry = new Entry() { HorizontalOptions = LayoutOptions.FillAndExpand, Keyboard =Keyboard.Numeric, Placeholder="Порт" };
                         entry.Completed += (o, er) =>
                         {
                             MobileStaticVariables.ConectSettings.DebugPort = int.Parse(entry.Text);
                             connectionButton.Text = "Выберите подключение (" + MobileStaticVariables.ConectSettings.DebugPort + ")";
                         };
                         sectionHeaderPhoneLayout.Children.Add(label);
                         sectionHeaderPhoneLayout.Children.Add(entry);
                     }
                     if (answer == connectionWay[1])
                     {
                         MobileStaticVariables.ConectSettings.DebugPort = 2582;
                     }
                     if (answer == connectionWay[2])
                     {
                         MobileStaticVariables.ConectSettings.DebugPort = 2580;

                     }
                     if (answer == connectionWay[3])
                     {
                         MobileStaticVariables.ConectSettings.DebugPort = 2581;
                     }
                     if (answer == connectionWay[4])
                     {
                         MobileStaticVariables.ConectSettings.DebugPort = MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.BASE].Port;
                     }
                     connectionButton.Text = "Выберите подключение (" + MobileStaticVariables.ConectSettings.DebugPort + ")";
                 }
             };
            sectionHeaderPhoneLayout.Children.Add(connectionButton);
#endif
        }
        #endregion
        #region События
        protected override void OnAppearing()
        {
            sectionHeaderLayout.IsVisible = MobileStaticVariables.UserStatus == UserStatusEnum.Logged;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                //var actionChangeLanguage = await DisplayActionSheet(AppResources.SettingsPageChooseLanguage, AppResources.Cancel, "", Translate.Languages.Values.ToArray());

                //string cultureIdentifier = Translate.Languages.FirstOrDefault(x => x.Value == actionChangeLanguage).Key;
                //AppResources.Culture = new CultureInfo(cultureIdentifier);


                //statusLanguageLabel.Text = Translate.Languages[AppResources.Culture.Name];
                //MobileStaticVariables.UserAppSettings.CurrentLanguage = cultureIdentifier;
                //MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.CurrentLanguage, cultureIdentifier);
                MobileStaticVariables.UserAppSettings.LastUpdate = DateTime.MinValue;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.LastUpdate, DateTime.MinValue.ToString());
                MobileStaticVariables.UserAppSettings.LastUpdateSupportPack = DateTime.MinValue;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.LastUpdateSupportPack, DateTime.MinValue.ToString());

                //await DisplayAlert(AppResources.Warning, AppResources.SettingsPageChangeLanguageWarning, AppResources.Ok);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void useFingerprintSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                MobileStaticVariables.UserAppSettings.IsUseFingerprint = e.Value;
                //showFingerprintSwitch.IsEnabled = MobileStaticVariables.UserAppSettings.IsUseFingerprint;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsUseFingerprint, e.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void showFingerprintSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsShowFingerprint, e.Value.ToString());
        }

        private void useVibrationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                MobileStaticVariables.UserAppSettings.UseVibration = e.Value;
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.UseVibration, e.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void NotificationTypeClicked(object sender, EventArgs e)
        {
            backgroundDark.IsVisible = true;
            notificationFrameLarge.IsVisible = true;

            smsFrame.IsVisible = MobileStaticVariables.UserAppSettings.UseSMSIssuer;
            checkSms.IsChecked = MobileStaticVariables.UserAppSettings.UseSMSUser;
            checkEmail.IsChecked = MobileStaticVariables.UserAppSettings.UseEmailUser;
            checkNotification.IsChecked = MobileStaticVariables.UserAppSettings.ShowNotifications;
        }

        private void continueButton_Clicked(object sender, EventArgs e)
        {
            notificationFrameLarge.IsVisible = false;
            backgroundDark.IsVisible = false;
            StartLoading();

            try
            {
                DeviceInfo info = new DeviceInfo();
                string deviceInfo = "";
                try
                {
                    info.Imei = DependencyService.Get<IDevice>().GetIdentifier();
                    info.AppVersion = DependencyService.Get<IDevice>().GetVersion();
                    info.DeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                    info.UserID = MobileStaticVariables.UserInfo.MobileUserKey;
                    //info.NotificationToken = CrossFirebasePushNotification.Current.Token;
                    BitSetModes mode = BitSetModes.None;
                    if (MobileStaticVariables.UserAppSettings.UseSMSUser)
                        mode |= BitSetModes.PhoneMessage;
                    if (MobileStaticVariables.UserAppSettings.UseEmailUser)
                        mode |= BitSetModes.EmailMessage;
                    if (MobileStaticVariables.UserAppSettings.ShowNotifications)
                        mode |= BitSetModes.NotificationMessage;
                    info.BitSetting = (int)mode;
                    deviceInfo = MobileStaticVariables.WebUtils.SendMobileInfoRequest("SetDevice",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(info))));
                    Logger.WriteLine("deviceInfo : " + deviceInfo);
                    if (deviceInfo == "")
                        throw new Exception("device empty string");
                    var deviceInfo1 = new DeviceInfo();
                    deviceInfo1 = JsonConvert.DeserializeObject<DeviceInfo>(deviceInfo);
                    deviceInfo1.Parse(deviceInfo);

                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            EndLoading();
                        }
                        catch
                        { }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
        #endregion

        #region Методы

        void StartLoading()
        {
            backgroundDark.BackgroundColor = new Color(0, 0, 0, 0.5);
            backgroundDark.IsVisible = true;

            mainLayout.IsEnabled = false;
            IndicatorLayout.Start();
        }

        void EndLoading()
        {
            backgroundDark.IsVisible = false;

            mainLayout.IsEnabled = true;
            IndicatorLayout.Stop();
        }
        #endregion
    }
}