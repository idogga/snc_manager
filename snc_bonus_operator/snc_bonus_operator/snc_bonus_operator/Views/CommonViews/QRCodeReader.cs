using Plugin.Vibrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace snc_bonus_operator.CommonViews
{
    public class QRCodeReader : ContentPage
    {
        private bool needToReadCode = true;

        public enum TypeQRReadAnswer
        {
            /// <summary>
            /// Чтение кода с колонки АЗС при заявке на заправку
            /// </summary>
            Order,
            /// <summary>
            /// Чтение кода с листа регистрации пользователя
            /// </summary>
            Registration,
            /// <summary>
            /// Чтение qr-кода со смартфона 
            /// </summary>
            ClientQR,
            /// <summary>
            /// Чтение EAN13-кода со смартфона 
            /// </summary>
            ClientBarCode
        }

        ZXingScannerView scannerView;
        private string format = "";
        private string qr_value = "";
        private double _darkframe = 0;
        private bool isTorchOn = false;
        //private bool isVertical = false;

        StackLayout sideHorizontalStack = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.EndAndExpand,
            BackgroundColor = Color.Transparent,
            Padding = 0,
            Margin = 0,
        };

        StackLayout leftVerticalStack = new StackLayout()
        {
            Orientation = StackOrientation.Vertical,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.StartAndExpand,
            Padding = 0,
            Margin = 0,
        };
        StackLayout rightVerticalStack = new StackLayout()
        {
            Orientation = StackOrientation.Vertical,

            HorizontalOptions = LayoutOptions.EndAndExpand,

            Padding = 0,
            Margin = 0,
        };

        StackLayout horizontalStackTop = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Start,
            Padding = 0,
            Margin = 0,
        };

        StackLayout horizontalStackBottom = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.End,
            Padding = 0,
            Margin = 0,
        };

        //AzsQrInfo a { get; set; }

        public QRCodeReader(string _topTitle, string _bottomTitle, TypeQRReadAnswer _type)
        {
            Title = "Сканирование кода";
            scannerView = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,

            };

            ToolbarItem torchItem = new ToolbarItem()
            {
                Icon = "torch_off.png",
            };
            torchItem.Clicked += delegate
            {
                scannerView.ToggleTorch();
                isTorchOn = !isTorchOn;
                torchItem.Icon = (isTorchOn) ? "torch_on.png" : "torch_off.png";
            };
            ToolbarItems.Add(torchItem);
            var expectedFormat = ZXing.BarcodeFormat.QR_CODE;
            var expectedFormat2 = ZXing.BarcodeFormat.EAN_13;
            var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat> { expectedFormat, expectedFormat2 }
            };
            Logger.WriteLine("Сканирование " + expectedFormat);
            scannerView.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    scannerView.IsAnalyzing = false;
                    format = result?.BarcodeFormat.ToString() ?? string.Empty;
                    qr_value = result?.Text ?? string.Empty;
                    Logger.WriteLine(string.Format("Результат сканирования {0} \n {1}", format, qr_value));
                    bool res = false;
                    if (needToReadCode)
                    {
                        needToReadCode = false;
                        switch (_type)
                        {
                            //case TypeQRReadAnswer.Order:
                            //    Logger.WriteLine("Сосканирован тип : " + _type.ToString());
                            //    res = MobileStaticVariables.AzsQr.FromJson(qr_value);
                            //    break;
                            //case TypeQRReadAnswer.Registration:
                            //    Logger.WriteLine("Сосканирован тип : " + _type.ToString());
                            //    res = MobileStaticVariables.UserQR.FromJson(qr_value);
                            //    break;
                            case TypeQRReadAnswer.ClientQR:
                                Logger.WriteLine("Сосканирован тип : " + _type.ToString());
                                res = MobileStaticVariables.QRClient.FromJson(qr_value);
                                break;
                            case TypeQRReadAnswer.ClientBarCode:
                                Logger.WriteLine("Сосканирован тип : " + _type.ToString());
                                res = MobileStaticVariables.QRClient.FromString(qr_value);
                                break;
                        }
                        if (res)
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
                            if (_type == TypeQRReadAnswer.Registration)
                            {
                                //MobileStaticVariables.UserInfo.MobileUserKey = MobileStaticVariables.UserQR.UID;
                                //MobileStaticVariables.UserInfo.Hash = MobileStaticVariables.UserQR.HSH;
                                //await Navigation.PushAsync(new AuthPage(AuthPageState.Registration));
                            }
                            else
                            {
                                await Navigation.PopAsync();
                            }
                        }
                        else
                        {
                            Logger.WriteLine("Сканирование не засчитано");
                        }
                        needToReadCode = true;
                    }
                    //res = false;
                });
            };

            var semiTransparentColor = new Color(0, 0, 0, 0.5);


            var verticalStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0,
                Spacing = 0,
            };



            var topLabel = new Label
            {
                Text = _topTitle,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            horizontalStackTop.Children.Add(topLabel);

            var bottomLabel = new Label
            {
                Text = _bottomTitle,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            horizontalStackBottom.Children.Add(bottomLabel);

            //Задание цвета.
            {
                horizontalStackBottom.BackgroundColor = semiTransparentColor;
                leftVerticalStack.BackgroundColor = semiTransparentColor;
                rightVerticalStack.BackgroundColor = semiTransparentColor;
                horizontalStackTop.BackgroundColor = semiTransparentColor;
            }

            sideHorizontalStack.Children.Add(leftVerticalStack);
            sideHorizontalStack.Children.Add(rightVerticalStack);

            verticalStack.Children.Add(horizontalStackTop);
            verticalStack.Children.Add(sideHorizontalStack);
            verticalStack.Children.Add(horizontalStackBottom);

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0,
                RowSpacing = 0,
                ColumnSpacing = 0,
            };

            grid.Children.Add(scannerView);
            grid.Children.Add(verticalStack);
            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            scannerView.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            scannerView.IsScanning = false;
            base.OnDisappearing();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width < height)
            {
                _darkframe = Application.Current.MainPage.Width / 6;
                var heigt_horizontalStack = (Application.Current.MainPage.Height - (4 * _darkframe)) / 2;
                sideHorizontalStack.WidthRequest = 4 * _darkframe;
                leftVerticalStack.HeightRequest = 4 * _darkframe;
                leftVerticalStack.WidthRequest = _darkframe;
                rightVerticalStack.HeightRequest = 4 * _darkframe;
                rightVerticalStack.WidthRequest = _darkframe;
                horizontalStackTop.HeightRequest = heigt_horizontalStack;
                horizontalStackBottom.HeightRequest = heigt_horizontalStack;

            }
            else
            {
                _darkframe = Application.Current.MainPage.Height / 6;

                double widthVerticalStack = (Application.Current.MainPage.Width - 4 * _darkframe) / 2;
                leftVerticalStack.WidthRequest = widthVerticalStack;
                rightVerticalStack.WidthRequest = widthVerticalStack;
                sideHorizontalStack.WidthRequest = 4 * _darkframe;
                leftVerticalStack.HeightRequest = 4 * _darkframe;
                rightVerticalStack.HeightRequest = 4 * _darkframe;
                horizontalStackTop.HeightRequest = _darkframe;
                horizontalStackBottom.HeightRequest = _darkframe;
            }
        }
    }
}
