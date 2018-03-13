using Newtonsoft.Json;
using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Stuff
{
    public partial class NewColleguePage : ContentPage
    {
        //List<AzsInfo> azsList = new List<AzsInfo>();
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        Dictionary<string, int> dictTO = new Dictionary<string, int>();
        Colegue _colegue=new Colegue();

        public NewColleguePage()
        {
            InitializeComponent();
            //PageLoad();
            if(MobileStaticVariables.UserInfo.ShopList.Count==1)
            {
                selectShopButton.Text = MobileStaticVariables.UserInfo.ShopList[0].Name;
                selectShopButton.IsEnabled = false;
            }
        }

        //async void PageLoad()
        //{
        //    //StartLoading();
        //    //var cancellationTokenSource = new CancellationTokenSource();
        //    //await Task.Factory.StartNew(x =>
        //    //{
        //    //    LoadShops();
        //    //},
        //    //TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
        //}

        public NewColleguePage(Colegue colegue)
        {
            InitializeComponent();
            _colegue = colegue;
            nameEntry.Text = _colegue.Name;
            loginEntry.Text = _colegue.Email;
            positionEntry.Text = _colegue.Position;
            selectShopButton.Text = _colegue.ShopName;
            continueButton.Text = "Изменить данные";
            if (MobileStaticVariables.UserInfo.ShopList.Count == 1)
            {
                //selectShopButton.Text = MobileStaticVariables.UserInfo.ShopList[0].Name;
                selectShopButton.IsEnabled = false;
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

        private async void selectShopTapped(object sender, EventArgs e)
        {
            dictTO.Clear();

            foreach (var item in MobileStaticVariables.UserInfo.ShopList)
            {
                if (item.Name != "")
                {
                    dictTO.Add(string.Format("ТО №{0} {1}", item.ShopKey, item.Name), item.ShopKey);
                }
                else
                {
                    dictTO.Add("ТО №" + item.ShopKey, item.ShopKey);
                }
            }
            var answer = await DisplayActionSheet("Выберите магазин", "Отмена", "", dictTO.Keys.ToArray());
            if (answer != null && answer != "Отмена")
            {
                selectShopButton.Text = answer;
            }
        }

        private async void continueButton_Clicked(object sender, EventArgs e)
        {
            string login = (Regex.IsMatch(loginEntry.Text, emailRegex)) ? loginEntry.Text : "";
            string name = nameEntry.Text.Trim();
            string position = positionEntry.Text.Trim();
            string Shop = selectShopButton.Text;
            if (name != "")
            {
                if (login != "")
                {
                    if ((Shop != "Магазин не выбран" && MobileStaticVariables.UserInfo.ShopList.Count != 1) || MobileStaticVariables.UserInfo.ShopList.Count == 1)
                    {
                        if (_colegue == null)
                        {
                            _colegue = new Colegue();
                        }
                        _colegue.Email = login;
                        _colegue.Name = name;
                        _colegue.Position = position;
                        if (MobileStaticVariables.UserInfo.ShopList.Count == 1)
                        {
                            _colegue.ShopKey = MobileStaticVariables.UserInfo.ShopList.FirstOrDefault().ShopKey;
                        }
                        else
                        {
                            if (dictTO.TryGetValue(Shop, out int shopKey))
                            {
                                _colegue.ShopKey = shopKey;
                            }
                        }

                        var stuff = new StuffModel();
                        stuff.MobileDevicKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                        stuff.ColegueList.Clear();
                        stuff.ColegueList.Add(_colegue);
                        var cancellationTokenSource = new CancellationTokenSource();
                        try
                        {
                            StartLoading();
                            await Task.Factory.StartNew(x =>
                            {
                                try
                                {
                                    string registration = "";
                                    registration = MobileStaticVariables.WebUtils.SendAuthRequest("RegManager", stuff);
                                    Logger.WriteLine("registration : " + registration);
                                    if (registration == "")
                                        throw new Exception("Получена пустая строка");
                                    stuff.ParseJson(registration);
                                    if (stuff.ResultState == RequestResult.Results.Success)
                                    {
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            await DisplayAlert("Ура", "Добавление прошло успешно", "Продолжить");
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
                        catch (Exception ex)
                        {
                            Logger.WriteError(ex);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Внимание", "Введите магазин, в котором будет работать Ваш будущий коллега", "Хорошо");
                        positionEntry.Focus();
                    }
                }
                else
                {
                    await DisplayAlert("Внимание", "Введите почту, чтобы мы могли связаться с Вашим будущим колегой", "Хорошо");
                    loginEntry.Focus();
                }
            }
            else
            {
                await DisplayAlert("Внимание", "Введите имя Вашего будущего коллеги", "Хорошо");
                nameEntry.Focus();
            }
        }

        private void nameEntry_Completed(object sender, EventArgs e)
        {
            loginEntry.Focus();
        }

        private void loginEntry_Completed(object sender, EventArgs e)
        {
            positionEntry.Focus();
        }

        private void positionEntry_Completed(object sender, EventArgs e)
        {
            positionEntry.Unfocus();
        }
    }
}