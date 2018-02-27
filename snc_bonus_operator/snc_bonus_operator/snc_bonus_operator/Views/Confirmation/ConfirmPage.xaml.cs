using Newtonsoft.Json;
using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Confirmation
{
    public partial class ConfirmPage : ContentPage
    {
        ObservableCollection<DateGroup> Transactions { get; set; } = new ObservableCollection<DateGroup>();
        SellerTransactionInfo transactions = new SellerTransactionInfo() { From = new DateTime(2002, 10, 23), To = DateTime.Now };
        List<int> MeassageList = new List<int>();
        double _upFrame = .0;
        bool isLoading;
        bool needToLoad = true;
        const int _amount = 20;

        #region Конструктор
        public ConfirmPage()
        {
            InitializeComponent();
            listTransaction.ItemAppearing += async (sender, e) =>
            {
                Logger.WriteLine("ItemAppearing");
                if (isLoading || Transactions.Count == 0)
                    return;
                if (listTransaction.ItemsSource == null)
                    return;
                if (e.Item == Transactions.LastOrDefault().LastOrDefault())
                {
                    if (needToLoad)
                    {
                        StartLoading();
                        await Task.Factory.StartNew(()=>
                        {
                            var count = 0;
                            foreach(var group in Transactions)
                            {
                                count += group.Count;
                            }
                            LoadTransactions(count);
                        });
                        EndLoading();
                    }
                    else
                    {
                        statusTransaction.Text = "Вся история загружена";
                        statusTransaction.IsVisible = true;
                    }
                }
            };
        }
        #endregion

        #region События
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (MobileStaticVariables.UserAppSettings.IsInetAvaliable == Settings.InternetStatus.Online)
            {
                LoadMainScreen();
            }
            else
            {
                noConnectionLayout.IsVisible = true;
                Device.StartTimer(new TimeSpan(0, 0, 1), WaitInternetConnection);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        private void transactionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var selectTransaction = (CellModel)e.SelectedItem;
            if (selectTransaction.IsChecked)
            {
                MeassageList.Remove(selectTransaction.ID);
                selectTransaction.IsChecked = false;
            }
            else
            {
                MeassageList.Add(selectTransaction.ID);
                selectTransaction.IsChecked = true;
            }
            listTransaction.ItemsSource = null;

            if (MeassageList.Count == 0)
            {
                sendList.IsVisible = false;
                Title = "Акцептирование";
                this.ToolbarItems[0].Text = "Выбрать все";
            }
            else
            {
                sendList.IsVisible = true;
                this.ToolbarItems[0].Text = "Снять все";
                Title = string.Format("Выбрано: {0}", MeassageList.Count);
            }
            Logger.WriteLine("6");
            listTransaction.ItemsSource = Transactions;
            listTransaction.SelectedItem = null;
        }

        private async void acceptButton_Clicked(object sender, EventArgs e)
        {
            StartLoading();
            var cancellationTokenSource = new CancellationTokenSource();
            await Task.Factory.StartNew(x =>
            {
                var transactionsInfo = "";
                try
                {
                    var acceptSelling = new AcceptSelling() { Accept = true, TransactionKey = MeassageList.ToList(), AcceptSellerName = MobileStaticVariables.UserInfo.UserNickName };
                    transactionsInfo = MobileStaticVariables.WebUtils.SendVerifyTransaction("Accept", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(acceptSelling))));
                    var conflictTransactions = JsonConvert.DeserializeObject<ConflictTransactions>(transactionsInfo);
                    if (conflictTransactions.ResultState == RequestResult.Results.Success)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            EndLoading();
                            if (IsVisible)
                                await DisplayAlert("", "Акцептование транзакций прошло успешно", "Продолжить");
                            LoadPage();
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            EndLoading();
                            if (conflictTransactions.ResultState == RequestResult.Results.AlreadyExists)
                            {
                                await Navigation.PushAsync(new ConflictTransactionPage(conflictTransactions.TransactionKeys, acceptSelling.Accept));
                            }
                            else if (IsVisible)
                            {
                                await DisplayAlert("Внимание", conflictTransactions.TranslateResult(conflictTransactions.ResultState), "Продолжить");
                            }
                            LoadPage();
                        });
                    }
                }
                catch
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        EndLoading();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            EndLoading();
                            if (IsVisible)
                            {
                                HideList();
                                noConnectionLayout.IsVisible = true;
                                Device.StartTimer(new TimeSpan(0, 0, 1), WaitInternetConnection);
                            }
                        });
                    });
                }
            },
                TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
        }

        private async void declineButton_Clicked(object sender, EventArgs e)
        {
            StartLoading();
            var cancellationTokenSource = new CancellationTokenSource();
            await Task.Factory.StartNew(x =>
            {
                var transactionsInfo = "";
                try
                {
                    var acceptSelling = new AcceptSelling() { Accept = false, TransactionKey = MeassageList.ToList() };
                    transactionsInfo = MobileStaticVariables.WebUtils.SendVerifyTransaction("Accept", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(acceptSelling))));
                    var conflictTransactions = JsonConvert.DeserializeObject<ConflictTransactions>(transactionsInfo);
                    if (conflictTransactions.ResultState == RequestResult.Results.Success)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            EndLoading();
                            if (IsVisible)
                                await DisplayAlert("Ура", "Отклонение транзакций прошло успешно", "Продолжить");
                            LoadPage();
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            EndLoading();
                            if (conflictTransactions.ResultState == RequestResult.Results.AlreadyExists)
                            {
                                await Navigation.PushAsync(new ConflictTransactionPage(conflictTransactions.TransactionKeys, acceptSelling.Accept));
                            }
                            else if (IsVisible)
                            {
                                await DisplayAlert("Внимание", conflictTransactions.TranslateResult(conflictTransactions.ResultState), "Продолжить");
                            }
                            LoadPage();
                        });
                    }
                }
                catch
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        EndLoading();
                        if (IsVisible)
                        {
                            HideList();
                            noConnectionLayout.IsVisible = true;
                            Device.StartTimer(new TimeSpan(0, 0, 1), WaitInternetConnection);
                        }
                    });
                }
            },
                TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
        }

        private void selectAll_clicked()
        {
            if (MeassageList.Count != 0)
            {
                foreach (var group in Transactions)
                {
                    foreach (var item in group)
                    {
                        item.IsChecked = false;
                    }
                }
                MeassageList.Clear();
                sendList.IsVisible = false;
                this.Title = "Акцептирование";
                listTransaction.ItemsSource = null;
                listTransaction.ItemsSource = Transactions;
                this.ToolbarItems[0].Text = "Выбрать все";
            }
            else
            {
                foreach (var group in Transactions)
                {
                    foreach (var item in group)
                    {
                        item.IsChecked = true;
                        if (!MeassageList.Contains(item.ID))
                        {
                            MeassageList.Add(item.ID);
                        }
                    }
                }
                sendList.IsVisible = true;
                Title = string.Format("Выбрано: {0}", MeassageList.Count);
                listTransaction.ItemsSource = null;
                listTransaction.ItemsSource = Transactions;
                this.ToolbarItems[0].Text = "Снять все";
            }
        }

        private void listTransaction_Refreshing(object sender, EventArgs e)
        {
            Logger.WriteLine("7");
            listTransaction.BeginRefresh();
            Transactions.Clear();
            MeassageList.Clear();
            sendList.IsVisible = false;
            this.Title = "Выберите транзакцию";
            LoadTransactions(0);
            listTransaction.EndRefresh();
        }
        #endregion

        #region Методы
        void LoadFilter(SellerTransactionInfo info)
        {
            transactions.From = info.From;
            transactions.To = info.To;
            transactions.Amount = _amount;
            transactions.Offset = 0;
        }

        async void LoadPage()
        {
            StartLoading();
            Transactions.Clear();
            MeassageList.Clear();
            sendList.IsVisible = false;
            this.Title = "Акцептирование";
            this.ToolbarItems[0].Text = "Выбрать все";
            await Task.Factory.StartNew(() =>
            {
                LoadTransactions(0);
            });
            EndLoading();
        }

        void LoadTransactions(int offset)
        {
            try
            {
                isLoading = true;
                string transactionsInfo = "";
                transactions.Amount = _amount;
                transactions.DeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                transactions.Offset = offset;
                transactionsInfo = MobileStaticVariables.WebUtils.SendCardRequest("TransactionsSeller", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(transactions))));

                Logger.WriteLine("transactionsInfo : " + transactionsInfo);
                if (transactionsInfo == "")
                    throw new Exception("Pustaya stroka");
                transactions = JsonConvert.DeserializeObject<SellerTransactionInfo>(transactionsInfo);
                if (transactions.ResultState == RequestResult.Results.Success)
                {
                    if (offset == 0 && transactions.Transactions.Count == 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            HideList();
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (IsVisible)
                        {
                            await DisplayAlert("Внимание", transactions.TranslateResult(transactions.ResultState), "Продолжить");
                            HideList();
                        }
                    });
                }
                
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in transactions.Transactions)
                    {
                        var transactionCellView = new CellModel(item);
                        var group = Transactions.FirstOrDefault(x => (x.ComleteDate.Year == item.CompleteDatetime.Year) && (x.ComleteDate.DayOfYear == item.CompleteDatetime.DayOfYear));
                        if (group != null)
                        {
                            group.Add(transactionCellView);
                        }
                        else
                        {
                            group = new DateGroup(item.CompleteDatetime, _upFrame);
                            group.Add(transactionCellView);
                            Transactions.Add(group);
                        }
                    }
                    needToLoad = transactions.Transactions.Count == _amount;
                    if (IsVisible)
                    {
                        listTransaction.ItemsSource = null;
                        listTransaction.ItemsSource = Transactions;
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (IsVisible)
                    {
                        HideList();
                        noConnectionLayout.IsVisible = true;
                        Device.StartTimer(new TimeSpan(0, 0, 1), WaitInternetConnection);
                    }
                });
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    EndLoading();
                });
                isLoading = false;
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

        void HideList()
        {
            this.ToolbarItems.Clear();
            listTransaction.IsVisible = false;
            emptyFrame.IsVisible = true;
            statusTransaction.IsVisible = false;
            sendList.IsVisible = false;
        }


        private bool WaitInternetConnection()
        {
            var result = true;
            if (MobileStaticVariables.UserAppSettings.IsInetAvaliable == Settings.InternetStatus.Online)
            {
                result = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    noConnectionLayout.IsVisible = false;
                    LoadMainScreen();
                });
            }
            return result;
        }

        private void LoadMainScreen()
        {
            Transactions.Clear();
            MeassageList.Clear();
            emptyFrame.IsVisible = false;
            listTransaction.IsVisible = true;
            statusTransaction.IsVisible = false;
            _upFrame = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
            if (this.ToolbarItems.Count == 0)
            {
                var tool = new ToolbarItem() { Text = "Выбрать все", Command = new Command(this.selectAll_clicked) };
                this.ToolbarItems.Add(tool);
            }
            LoadPage();
        }
        #endregion
    }
}