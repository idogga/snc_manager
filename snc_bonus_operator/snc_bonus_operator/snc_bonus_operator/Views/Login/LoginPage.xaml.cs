using Newtonsoft.Json;
using Plugin.DeviceInfo;
using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Login
{
    public partial class LoginPage : ContentPage
    {
        #region Конструктор
        List<string> LoggedPerson = new List<string>();
        SettingsDB settingsDB = new SettingsDB();
        bool _isUserRegister = false;
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public LoginPage()
        {
            InitializeComponent();
            
            LoggedPerson = settingsDB.GetLoggedPerson();
            if (LoggedPerson.Count > 0)
            {
                alreadyLogged.IsVisible = true;
                loginEntry.Text = LoggedPerson.LastOrDefault();
                passEntry.Focus();
            }
            else
            {
                loginEntry.Focus();
            }
        }
        #endregion

        #region Обработка событий

        private void continueButtonCLicked(object sender, EventArgs e)
        {
            LoginManager();
        } 

        private void passEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void loginEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private async void ForgetPass_Tapped(object sender, EventArgs e)
        {

            if (loginEntry.Text.Length == 0)
            {
                await DisplayAlert("Внимание", "Введите сначала свой емэйл", "Ввести");
                loginEntry.Focus();
            }
            else
            {
                StartLoading();
                await Task.Factory.StartNew(() =>
                {
                    string userInfo = "";
                    try
                    {
                        var requsites = new UserRequisites() { Email = loginEntry.Text };
                        userInfo = MobileStaticVariables.WebUtils.SendAuthRequest("SellerReloadPassword",
                            Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requsites))));
                        if (userInfo == "")
                            throw new Exception("empty string");
                        var answer = JsonConvert.DeserializeObject<RequestResult>(userInfo);
                        if (answer.ResultState == RequestResult.Results.Success)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await DisplayAlert("Ура", string.Format("На почтовый адрес {0} был выслан новый пароль", requsites.Email), "Продолжить");
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await DisplayAlert("Внимание", answer.TranslateResult(answer.ResultState), "Продолжить");
                            });
                        }
                        Logger.WriteLine("userInfo : " + userInfo);
                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (IsVisible)
                            {
                                await DisplayAlert("Внимание", "Ошибка при выполнение запроса", "Повторить");
                                ForgetPass_Tapped(sender, e);
                            }
                        });
                        Logger.WriteError(ex);
                    }
                    finally
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            EndLoading();
                        });
                    }
                });
            }
        }

        private async void alreadyLogged_tapped(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Выберите логин : ", "Отмена", "", LoggedPerson.ToArray());
            if (answer != null && answer != "Отмена")
            {
                loginEntry.Text = answer;
            }
        }

        private void loginEntry_Completed(object sender, EventArgs e)
        {
            passEntry.Focus();
        }

        private void passEntry_Completed(object sender, EventArgs e)
        {
            LoginManager();
        }
        #endregion

        #region Методы 

        async void LoginManager()
        {
            if (loginEntry.Text.Length == 0)
            {
                await DisplayAlert("Внимание", "Вы не ввели логин", "Ввести");
                loginEntry.Focus();
                return;
            }
            if (!Regex.IsMatch(loginEntry.Text, emailRegex))
            {
                await DisplayAlert("Внимание", "Введите именно почту", "Ввести");
                loginEntry.Focus();
                return;
            }
            if (passEntry.Text.Length == 0)
            {
                await DisplayAlert("Внимание", "Вы не ввели пароль", "Ввести");
                passEntry.Focus();
                return;
            }
            StartLoading();
            LoadData();
        }

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

        async void LoadData()
        {
            StartLoading();
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await Task.Factory.StartNew(async x => {
                    await LoadUserInfo();
                },
                TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        async Task LoadUserInfo()
        {
            string confirmInfo = "";
            RegManager userInfo = new RegManager();
            userInfo.Email = loginEntry.Text.Trim();
            userInfo.Password = passEntry.Text.Trim();
            userInfo.DeviceName = string.Format("{0} {1} {2}",
                    CrossDeviceInfo.Current.Model, CrossDeviceInfo.Current.Platform, CrossDeviceInfo.Current.Version);
            userInfo.Imei = DependencyService.Get<IDevice>().GetIdentifier();
            userInfo.AppVersion = DependencyService.Get<IDevice>().GetVersion();
            userInfo.AppKey = RegisterInfo.AppKeys.BonusSeller;
            try
            {
                confirmInfo = MobileStaticVariables.WebUtils.SendAuthRequest("AuthManager", userInfo);
                if (confirmInfo == "")
                    throw new Exception("Пустая строка");
                userInfo = JsonConvert.DeserializeObject<RegManager>(confirmInfo);
                Logger.WriteLine("confirmInfo : " + confirmInfo);

                if (userInfo.ResultState == RequestResult.Results.Success)
                {
                    _isUserRegister = true;
                    MobileStaticVariables.UserInfo.MobileDeviceKey = userInfo.MobileDeviceKey;
                    MobileStaticVariables.UserInfo.MobileUserKey = userInfo.MobileUserKey;
                    MobileStaticVariables.UserInfo.UserNickName = userInfo.UserNickName;
                    MobileStaticVariables.UserInfo.NotificationUserTopic = userInfo.NotificationTopic;
                    MobileStaticVariables.UserInfo.UserType = userInfo.UserType;
                    MobileStaticVariables.UserInfo.ShopList = userInfo.ShopList;
                    MobileStaticVariables.UserInfo.Stuff = userInfo.Stuff;
                    MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] = new CertificateKey()
                    {
                        Certificate = userInfo.PublicKey,
                        PrivateKey = userInfo.PrivateKey,
                        Port = MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.BASE_ISSUER].Port,
                        IP = MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.BASE_ISSUER].IP
                    };
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.Email, MobileStaticVariables.UserInfo.Email);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.MobileDeviceKey, MobileStaticVariables.UserInfo.MobileDeviceKey.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.MobileUserKey, MobileStaticVariables.UserInfo.MobileUserKey.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.UserNickName, MobileStaticVariables.UserInfo.UserNickName);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.UserType, ((int)MobileStaticVariables.UserInfo.UserType).ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.PRIVATE_USER_PRIVATE_CERTIFICATE, MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].PrivateKey);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.PRIVATE_USER_CERTIFICATE, MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].Certificate);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.PRIVATE_USER_PRIVATE_IP, MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].IP);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.PRIVATE_USER_PRIVATE_PORT, MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].Port.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.ShopList, JsonConvert.SerializeObject(MobileStaticVariables.UserInfo.ShopList));
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.Stuff, JsonConvert.SerializeObject(MobileStaticVariables.UserInfo.Stuff));
                }

                else
                {
                    _isUserRegister = false;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (IsVisible)
                        {
                            await DisplayAlert("Внимание", userInfo.TranslateResult(userInfo.ResultState),
                                  "Хорошо");
                            EndLoading();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                _isUserRegister = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (IsVisible)
                    {
                        var result = await DisplayAlert("Внимание", "Неудалось загрузить необходимые данные", "Повторить регистрацию", "Отмена");
                        if (result)
                        {
                            LoadData();
                        }
                        else
                        {
                            EndLoading();
                        }
                    }
                });
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (_isUserRegister)
                    {
                        settingsDB.AddLoggedPerson(loginEntry.Text);
                        App.Current.MainPage = new RootPage();
                    }
                    EndLoading();
                });
            }
        }
        #endregion
    }
}