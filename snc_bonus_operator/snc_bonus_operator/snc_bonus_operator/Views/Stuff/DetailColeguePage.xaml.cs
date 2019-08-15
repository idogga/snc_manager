using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace snc_bonus_operator.Stuff
{
    public partial class DetailColeguePage : ContentPage
    {
        Colegue _colegue;
        public DetailColeguePage(Colegue colegue)
        {
            _colegue = colegue;
            InitializeComponent();
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Имя : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = colegue.Name, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            nameLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Адрес электронной почты : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = colegue.Email, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = "  (нажмите чтобы написать)", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            emailLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Магазин : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = colegue.ShopName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            shopLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Подробности : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = colegue.Position, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            detailLabel.FormattedText = formatted;
            var posision = "";
            if (colegue.AdminUserKey != 0)
            {
                formatted = new FormattedString();
                formatted.Spans.Add(new Span { Text = "Курирует : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                formatted.Spans.Add(new Span { Text = colegue.AdminName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                adminLabel.FormattedText = formatted;
                posision = "Оператор";
                blockButton.IsVisible = true;
                if (colegue.UserState == UserStates.Blocked)
                {
                    blockButton.Text = "Разблокировать";
                }
                else
                {
                    blockButton.Text = "Заблокировать";
                }
            }
            else
            {
                adminLabel.IsVisible = false;
                posision = "Администратор";
                blockButton.IsVisible = false;
                
            }
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Должность : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = posision, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            positionLabel.FormattedText = formatted;
        }

        private async void editManager(object sender, System.EventArgs e)
        {
            if (_colegue.AdminUserKey != 0 && MobileStaticVariables.UserInfo.UserType == UserTypes.Admin)
            {
                Logger.WriteLine("Попытка изменить данные о пользователе");
                await Navigation.PushAsync(new NewColleguePage(_colegue));
            }
        }

        private async void sendEmail(object sender, System.EventArgs e)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Письмо от " + _colegue.Name,
                    Body = "",
                    To = new List<string>() { _colegue.Email },
                };
                await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                await DisplayAlert("Ошибка!", $"Не удалось отправить электронное сообщение [{ex.Message}]", "Продолжить" );
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

        private async void blockButton_Clicked(object sender, EventArgs e)
        {
            StartLoading();
            var stuff = new StuffModel();
            stuff.MobileDevicKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
            stuff.ColegueList.Add(_colegue);
            var cancellationTokenSource = new CancellationTokenSource();
            await Task.Factory.StartNew(x =>
            {
                try
                {
                    string registration = "";
                    registration = MobileStaticVariables.WebUtils.SendAuthRequest("BlockManager", stuff);
                    Logger.WriteLine("BlockManager : " + registration);
                    if (registration == "")
                        throw new Exception("Получена пустая строка");
                    stuff.ParseJson(registration);
                    if (stuff.ResultState == RequestResult.Results.Success)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Ура", "Оператор заблокирован", "Продолжить");
                            EndLoading();
                            await Navigation.PopToRootAsync();
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Внимание", stuff.TranslateResult(stuff.ResultState), "Продолжить");
                            EndLoading();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        EndLoading();
                        await DisplayAlert("Внимание", "Не удается соедениться с сервером", "Повторить");
                    });
                }
            },
            TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
        }
    }
}