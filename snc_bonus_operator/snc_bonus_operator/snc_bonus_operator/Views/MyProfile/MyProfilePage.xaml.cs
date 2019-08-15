using Newtonsoft.Json;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class MyProfilePage : ContentPage
    {
        #region Конструктор
        public MyProfilePage()
        {
            try
            {
                InitializeComponent();

                StartLoading();
                PageLoad();

                if (MobileStaticVariables.UserAppSettings.IsDeviceBlock)
                {
                    DeleteToolBar();
                }
                else
                {
                    MakeToolBar();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
        #endregion

        #region Обработка событий
        private async void OnMyProfileButton_Clicked()
        {
            try
            {
                mainLayout.IsEnabled = false;
                try
                {
                    if (MobileStaticVariables.UserAppSettings.UseVibration)
                    {
                        Vibration.Vibrate();
                    }
                }
                catch { }
                Device.OpenUri(new Uri("http://www.sncard.ru"));

            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                await DisplayAlert("Внимание", "Некорректная ссылка на профиль", "Продолжить");
            }
            finally
            {
                mainLayout.IsEnabled = false;
            }
        }
        
        private async void OnchangeUser()
        {
            try
            {
                try
                {
                    if (MobileStaticVariables.UserAppSettings.UseVibration)
                    {
                        Vibration.Vibrate();
                    }
                }
                catch { }
                mainLayout.IsEnabled = false;
                if (IsVisible)
                {
                    bool answer = await DisplayAlert("Внимание", "Смена пользователя удалит все текущие личные данные.", "Продолжить", "Отмена");
                    if (answer)
                    {
                        //DeviceSettings.UserStatus = UserStatusEnum.Unregister;
                        MobileStaticVariables.UserStatus = UserStatusEnum.UnRegister;
                        MobileStaticVariables.UserInfo.DeleteInfo();
                        MobileStaticVariables.UserStatus = UserStatusEnum.UnRegister;
                        App.Current.MainPage = new NavigationPage(new Login.LoginPage());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                WriteUserRequisites();
            }
            catch
            {
                //cardNumberLabel.Text = "У Вас нет карт";
            }
        }
        #endregion

        #region Дополнительные функции

        private async void PageLoad()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await Task.Factory.StartNew(x => {
                    UserInfoLoad();
                },
                TaskCreationOptions.AttachedToParent,
                cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                EndLoading();
            }
        }

        private void UserInfoLoad()
        {
            string userInfo = "";
            try
            {
                userInfo = MobileStaticVariables.WebUtils.SendMobileInfoRequest("SellerProfile",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(MobileStaticVariables.UserInfo as MobileSeller))));
                if (userInfo == "")
                    throw new Exception("empty string");
                MobileStaticVariables.UserInfo.Parse(userInfo);
                if (MobileStaticVariables.UserInfo.ResultState == RequestResult.Results.Success)
                {
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.CardKeys, MobileStaticVariables.UserInfo.CardKeys);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.Email, MobileStaticVariables.UserInfo.Email);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.GraphicalNumbers, MobileStaticVariables.UserInfo.GraphicalNumbers);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.Hash, MobileStaticVariables.UserInfo.Hash);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.LastActionDateTime, MobileStaticVariables.UserInfo.LastActionDateTime.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.MobileDeviceKey, MobileStaticVariables.UserInfo.MobileDeviceKey.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.MobileUserKey, MobileStaticVariables.UserInfo.MobileUserKey.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.PassHash, MobileStaticVariables.UserInfo.PassHash);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.PhoneNumber, MobileStaticVariables.UserInfo.PhoneNumber);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.RegisterDateTime, MobileStaticVariables.UserInfo.RegisterDateTime.ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.UserName, MobileStaticVariables.UserInfo.UserNickName);
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.UserState, ((int)MobileStaticVariables.UserInfo.UserState).ToString());
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.UserType, ((int)MobileStaticVariables.UserInfo.UserType).ToString());
                    MobileStaticVariables.UserAppSettings.IsDeviceBlock = false;
                    MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsDeviceBlock, MobileStaticVariables.UserAppSettings.IsDeviceBlock.ToString());
                }
                else
                {
                    if (MobileStaticVariables.UserInfo.ResultState == RequestResult.Results.RegisterUserNotFound || MobileStaticVariables.UserInfo.ResultState == RequestResult.Results.WrongDeviceState)
                    {
                        MobileStaticVariables.UserAppSettings.IsDeviceBlock = true;
                        MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsDeviceBlock, MobileStaticVariables.UserAppSettings.IsDeviceBlock.ToString());
                    }
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Внимание", MobileStaticVariables.UserInfo.TranslateResult(MobileStaticVariables.UserInfo.ResultState), "Продолжить");
                    });
                }
                Logger.WriteLine("userInfo : " + userInfo);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Внимание", "Ошибка при выполнение запроса", "Повторить");
                });
                Logger.WriteError(ex);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (MobileStaticVariables.UserAppSettings.IsDeviceBlock)
                        {
                            DeleteToolBar();
                        }
                        else
                        {
                            MakeToolBar();
                        }
                        WriteUserRequisites();
                    }
                    catch { }

                });
            }
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

        void WriteUserRequisites()
        {

            if (MobileStaticVariables.UserInfo.UserNickName == "")
            {
                MobileStaticVariables.UserInfo.UserNickName = "Гость";
            }
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Имя пользователя : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = MobileStaticVariables.UserInfo.UserNickName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            fio.FormattedText = formatted;
            if (MobileStaticVariables.UserInfo.Email == "")
            {
                emailLabel.IsVisible = false;
            }
            else
            {
                emailLabel.IsVisible = true;
                formatted = new FormattedString();
                formatted.Spans.Add(new Span { Text = "Электронная почта : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                formatted.Spans.Add(new Span { Text = MobileStaticVariables.UserInfo.Email, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                emailLabel.FormattedText = formatted;
            }

            
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Электронная почта : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = MobileStaticVariables.UserInfo.Email, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            emailLabel.FormattedText = formatted;
            
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Дата регистрации : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = string.Format("{0:dd.MM.yyyy}", MobileStaticVariables.UserInfo.RegisterDateTime), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            registrationDateLabel.FormattedText = formatted;
            string type = "";
            if(MobileStaticVariables.UserInfo.UserType== UserTypes.Admin)
            {
                type = "Администратор";
            }
            else
            {
                type = "Менеджер";
            }
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Должность : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = type, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            typeLabel.FormattedText = formatted;
        }

        void MakeToolBar()
        {
            DeleteToolBar();
            var myProfileLink = new ToolbarItem
            {
                Text = "Перейти в личный кабинет",
                Priority = 0,
                Order = ToolbarItemOrder.Secondary,
                Command = new Command(this.OnMyProfileButton_Clicked)
            };
            this.ToolbarItems.Add(myProfileLink);

            if (MobileStaticVariables.UserInfo.UserType == UserTypes.Admin)
            {
                var changePassword = new ToolbarItem
                {
                    Text = "Сменить пароль",
                    Priority = 1,
                    Order = ToolbarItemOrder.Secondary,
                    Command = new Command(this.ChangePass)
                };
                this.ToolbarItems.Add(changePassword);
            }
            var logoutButton = new ToolbarItem
            {
                Text = "Выйти из профиля",
                Priority = 2,
                Order = ToolbarItemOrder.Secondary,
                Command = new Command(this.OnchangeUser)
            };
            this.ToolbarItems.Add(logoutButton);
        }

        void DeleteToolBar()
        {
            ToolbarItems.Clear();
        }

        async void ChangePass()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            await Task.Factory.StartNew(x =>
            {
                string userInfo = "";
                try
                {
                    userInfo = MobileStaticVariables.WebUtils.SendAuthRequest("SellerReloadPassword",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(MobileStaticVariables.UserInfo as UserRequisites))));
                    if (userInfo == "")
                        throw new Exception("empty string");
                    var answer = JsonConvert.DeserializeObject<RequestResult>(userInfo);
                    if (answer.ResultState == RequestResult.Results.Success)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Ура", string.Format("На почтовый адрес {0} был выслан новый пароль", MobileStaticVariables.UserInfo.Email), "Продолжить");
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
                            ChangePass();
                        }
                    });
                    Logger.WriteError(ex);
                }
            },
            TaskCreationOptions.AttachedToParent,
            cancellationTokenSource.Token);

            EndLoading();
        }

        #endregion
    }
}