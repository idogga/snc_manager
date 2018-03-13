using Newtonsoft.Json;
using Plugin.Vibrate;
using snc_bonus_operator.CommonViews;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Cash
{
    public partial class CashbackPage : ContentPage
    {
        double totalPrice = 0;
        private ShopBasket shopBasket;
        string priceValue = "";
        public CashbackPage()
        {
            InitializeComponent();
            priceLabel.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) * 2;
            MobileStaticVariables.QRClient.ResetQRInfoAddedEvent();
            MobileStaticVariables.QRClient.QRInfoAddedEvent += FillOrderRequest;
            MobileStaticVariables.QRClient.BarInfoAddedEvent += FillBarRequest;
            MobileStaticVariables.QRClient.SetQrRead(false);
            MakeDefaultValues();
            operatorLabel.Text = "Оператор : " + MobileStaticVariables.UserInfo.UserNickName;
            int radius = (int)((App.Current.MainPage.Width - 36) / 6);
            foreach (var item in passKeyboardGrid.Children)
            {
                ((Button)item).BorderRadius = radius;
            }
        }

        void FillOrderRequest()
        {
            Logger.WriteLine("Считано : " + MobileStaticVariables.QRClient.Price.ToString());
            LoadScheme();
        }

        void FillBarRequest()
        {
            Logger.WriteLine("Считано : " + MobileStaticVariables.QRClient.ValueEAN13);
            StartLoading();
        }

        private async void qrReadButton_Clicked(object sender, System.EventArgs e)
        {
            if (MobileStaticVariables.UserAppSettings.IsInetAvaliable == InternetStatus.Online)
            {
                mainLayout.IsEnabled = false;
                Logger.WriteLine("Пользователь вызвал чтение QR-кода");
                try
                {
                    if (MobileStaticVariables.UserAppSettings.UseVibration)
                    {
                        var v = CrossVibrate.Current;
                        v.Vibration(TimeSpan.FromMilliseconds(50));
                    }
                }
                catch { }
                await Navigation.PushAsync(new QRCodeReader("Наведите чтобы считать",
                "QR-код клиента", QRCodeReader.TypeQRReadAnswer.ClientQR));
                mainLayout.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Внимание", "У Вас отсутсвует подключение к интернету.", "Продолжить");
            }
        }

        private async void barReadButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                Logger.WriteLine("Пользователь вызвал чтение QR-кода");
                try
                {
                    if (MobileStaticVariables.UserAppSettings.UseVibration)
                    {
                        var v = CrossVibrate.Current;
                        v.Vibration(TimeSpan.FromMilliseconds(50));
                    }
                }
                catch { }
                await Navigation.PushAsync(new QRCodeReader("Наведите чтобы считать",
                      "Здесь очень важно внимание", QRCodeReader.TypeQRReadAnswer.ClientBarCode));
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

        ~CashbackPage()
        {
            MobileStaticVariables.QRClient.BarInfoAddedEvent -= FillBarRequest;
            MobileStaticVariables.QRClient.QRInfoAddedEvent -= FillOrderRequest;
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

        async void LoadScheme()
        {
            StartLoading();
            try
            {
                await Task.Factory.StartNew(() => {
                    DecryptString();
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        void DecryptString()
        {
            string decryptInfo = "";
            try
            {
                var decryptObject = new DecryptInfo();
                decryptObject.Info = MobileStaticVariables.QRClient.Price.Info;
                decryptObject.DeviceKey = MobileStaticVariables.QRClient.Price.DeviceKey;
                decryptObject.MobileDeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                decryptObject.TotalPrice = totalPrice;
                decryptInfo = MobileStaticVariables.WebUtils.SendSystemRequest("CalculateSale",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(decryptObject))));
                if (decryptInfo == "")
                    throw new Exception("empty string");
                shopBasket = JsonConvert.DeserializeObject<ShopBasket>(decryptInfo);
                if (shopBasket.ResultState == RequestResult.Results.Success)
                {
                    Logger.WriteLine(shopBasket.MobileDeviceKey + " " + shopBasket.ResultState);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        priceLayout.IsVisible = false;
                        keyboardLayout.IsVisible = false;
                        acceptLayout.IsVisible = true;
                        var formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "Общая цена : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.TotalPrice.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        summLabel.FormattedText = formatted;
                        formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "Программа : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.UserProgrammName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        tariffLabel.FormattedText = formatted;
                        formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "Предложено для списания бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.BonusCountOut.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        bonusCountOutLabel.FormattedText = formatted;
                        formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "Будет списано бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.BonusCountOut.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        bonusCountOutLabel.FormattedText = formatted;
                        formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "Будет начислено бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.BonusCountIn.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        bonusCountInLabel.FormattedText = formatted;
                        formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "Скидка : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.Discount.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        formatted.Spans.Add(new Span { Text = " "+MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        discountLabel.FormattedText = formatted;
                        formatted = new FormattedString();
                        formatted.Spans.Add(new Span { Text = "ИТОГО : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        formatted.Spans.Add(new Span { Text = shopBasket.FinalPrice.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                        formatted.Spans.Add(new Span { Text = " "+MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                        finalCashLabel.FormattedText = formatted;
                    });
                    MobileStaticVariables.UserAppSettings.IsDeviceBlock = false;
                    MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsDeviceBlock, MobileStaticVariables.UserAppSettings.IsDeviceBlock.ToString());
                }
                else
                {
                    if (shopBasket.ResultState == RequestResult.Results.RegisterUserNotFound || shopBasket.ResultState == RequestResult.Results.WrongDeviceState)
                    {
                        MobileStaticVariables.UserAppSettings.IsDeviceBlock = true;
                        MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsDeviceBlock, MobileStaticVariables.UserAppSettings.IsDeviceBlock.ToString());
                    }
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (IsVisible)
                            await DisplayAlert("Внимание", MobileStaticVariables.UserInfo.TranslateResult(shopBasket.ResultState), "Продолжить");
                    });
                }
                Logger.WriteLine("userInfo : " + decryptInfo);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (IsVisible)
                        await DisplayAlert("Внимание", "Ошибка при выполнение запроса", "Повторить");
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
        }

        private async void acceptButton_Clicked(object sender, EventArgs e)
        {
            StartLoading();
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await Task.Factory.StartNew(x=> 
                {
                    string acceptInfo = "";
                    try
                    {
                        shopBasket.MobileDeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                        acceptInfo = MobileStaticVariables.WebUtils.SendSystemRequest("WriteSale",
                            Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shopBasket))));
                        if (acceptInfo == "")
                            throw new Exception("empty string");
                        shopBasket = JsonConvert.DeserializeObject<ShopBasket>(acceptInfo);
                        if (shopBasket.ResultState == RequestResult.Results.Success)
                        {
                            MobileStaticVariables.UserAppSettings.IsDeviceBlock = false;
                            MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.IsDeviceBlock, MobileStaticVariables.UserAppSettings.IsDeviceBlock.ToString());
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                try
                                {
                                    await Navigation.PushModalAsync(new BillPage(shopBasket));
                                }
                                catch(Exception ex)
                                {
                                    Logger.WriteError(ex);
                                }
                                MakeDefaultValues();
                            });
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
                                if (IsVisible)
                                    await DisplayAlert("Внимание", MobileStaticVariables.UserInfo.TranslateResult(MobileStaticVariables.UserInfo.ResultState), "Продолжить");
                            });
                        }
                        Logger.WriteLine("userInfo : " + acceptInfo);
                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (IsVisible)
                                await DisplayAlert("Внимание", "Ошибка при выполнение запроса", "Повторить");
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
                },
                TaskCreationOptions.AttachedToParent,
                cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void declineButton_Clicked(object sender, EventArgs e)
        {
            MakeDefaultValues();
        }
        
        void MakeDefaultValues()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                priceLayout.IsVisible = true;
                acceptLayout.IsVisible = false;
                keyboardLayout.IsVisible = true;
                priceValue = "0";
                DisplayPrice(priceValue);
            });
        }

        private void OnBackspaceButtonClicked(object sender, EventArgs e)
        {
            if (priceValue.Length > 0)
            {
                try
                {
                    if (MobileStaticVariables.UserAppSettings.UseVibration)
                    {
                        var v = CrossVibrate.Current;
                        v.Vibration(TimeSpan.FromMilliseconds(50));
                    }
                }
                catch { }
                priceValue = priceValue.Substring(0, priceValue.Length - 1);
                if(priceValue.Length > 0)
                {
                    backspaceButton.IsEnabled = true;
                }
                else
                {
                    priceValue = "0";
                    backspaceButton.IsEnabled = false;
                }
                dotButton.IsEnabled = !priceValue.Contains(",");
                if (IsValidNumber(priceValue))
                    DisplayPrice(priceValue);
            }
        }

        private void OnDigitButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (MobileStaticVariables.UserAppSettings.UseVibration)
                {
                    var v = CrossVibrate.Current;
                    v.Vibration(TimeSpan.FromMilliseconds(50));
                }
            }
            catch { }
            var button = (Button)sender;
            var newPriceValue = priceValue + button.StyleId;
            backspaceButton.IsEnabled = true;
            if (IsValidNumber(newPriceValue))
                DisplayPrice(priceValue);
        }

        private void OnSpecialButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (MobileStaticVariables.UserAppSettings.UseVibration)
                {
                    var v = CrossVibrate.Current;
                    v.Vibration(TimeSpan.FromMilliseconds(50));
                }
            }
            catch { }
            priceValue += ",";
            dotButton.IsEnabled = false;
            if(IsValidNumber(priceValue))
                DisplayPrice(priceValue);
        }

        void DisplayPrice(string value)
        {
            priceLabel.Text = value;
            qrReadButton.IsEnabled = priceLabel.Text != "0";
        }

        bool IsValidNumber(string value)
        {
            if (value.Contains(","))
            {
                var subvalue = value.Substring(value.IndexOf(","));
                if (subvalue.Length > 3) return false;
            }
            if (double.TryParse(value, out totalPrice))
            {
                if (totalPrice > 999999.99)
                {
                    totalPrice = 999999.99;
                }
                priceValue = totalPrice.ToString();
                if(value.EndsWith(",0"))
                {
                    priceValue += ",0";
                }
                else if (value.EndsWith(",00"))
                {
                    priceValue += ",00";
                }
                else if (value.EndsWith(",") )
                {
                    priceValue += ",";
                }
                return true;
            }
            else
                return false;
        }
    }
}