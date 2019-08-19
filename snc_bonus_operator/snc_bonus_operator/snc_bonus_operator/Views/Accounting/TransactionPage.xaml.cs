using Newtonsoft.Json;
using snc_bonus_operator.Cash;
using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Accounting
{
    public partial class TransactionPage : ContentPage
    {
        ObservableCollection<DateGroupTransaction> Transactions { get; set; } = new ObservableCollection<DateGroupTransaction>();
        SellerTransactionInfo transactions = new SellerTransactionInfo()
        { From = new DateTime(2002, 10, 23), To = DateTime.Now,
         TransactionStatuses=new List<SellerTransactionInfo.SELLER_STATUS_ENUM>(){ SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted,
          SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted, SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept, SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration}
        };

        double _upFrame = .0;
        UserCard card = new UserCard();
        bool isLoading;
        bool needToLoad = true;
        const int _amount = 20;
        List<int> listOfPosKeys = new List<int>();
        bool isBillOpen = false;

        public TransactionPage()
        {
            foreach(var item in MobileStaticVariables.UserInfo.Stuff)
            {
                transactions.SellerList.Add(item.MobileManagerKey);
            }
            InitializeComponent();
            listTransaction.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
            listTransaction.ItemAppearing += async (sender, e) =>
            {
                if (isLoading || Transactions.Count == 0)
                    return;
                if (e.Item == Transactions.LastOrDefault().LastOrDefault())
                {
                    if (needToLoad)
                    {
                        StartLoading();
                        await Task.Factory.StartNew(() =>
                        {
                            var count = 0;
                            foreach (var group in Transactions)
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
            FilterPage.EndEvent += LoadFilter;
            {
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!isBillOpen)
            {
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
            else
            {
                isBillOpen = false;
            }
        }

        void LoadFilter(SellerTransactionInfo info)
        {
            transactions.From = info.From;
            transactions.To = info.To;
            transactions.Amount = _amount;
            transactions.Offset = 0;
            transactions.TransactionStatuses = info.TransactionStatuses;
            transactions.SellerList = info.SellerList;
        }

        async void LoadPage()
        {
            StartLoading();
            Transactions.Clear();
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
                transactionsInfo = MobileStaticVariables.WebUtils.SendCardRequest("AllTransactionsSeller", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(transactions))));

                Logger.WriteLine("transactionsInfo : " + transactionsInfo);
                if (transactionsInfo == "")
                    throw new Exception("Pustaya stroka");
                transactions = JsonConvert.DeserializeObject<SellerTransactionInfo>(transactionsInfo);
                if (transactions.ResultState == RequestResult.Results.Success)
                {
                    if (offset == 0 && transactions.Transactions.Count == 0)
                    {
                        Device.BeginInvokeOnMainThread(() => HideList());
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            foreach (var item in transactions.Transactions)
                            {
                                var transactionView = new AllTransactionView(item);
                                var group = Transactions.FirstOrDefault(x => (x.ComleteDate.Year == item.CompleteDatetime.Year) && (x.ComleteDate.DayOfYear == item.CompleteDatetime.DayOfYear));
                                if (group != null)
                                {
                                    group.Add(transactionView);
                                }
                                else
                                {
                                    group = new DateGroupTransaction(item.CompleteDatetime);
                                    group.Add(transactionView);
                                    Transactions.Add(group);
                                }
                                needToLoad = transactions.Transactions.Count == _amount;
                                if (IsVisible)
                                {
                                    listTransaction.ItemsSource = null;
                                    listTransaction.ItemsSource = Transactions;
                                }
                            }
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
            listTransaction.IsVisible = false;
            emptyFrame.IsVisible = true;
            statusTransaction.IsVisible = false;
        }

        private async void ToolbarFilterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FilterPage(transactions));
        }

        private async void listTransaction_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var selectTransaction = (AllTransactionView)e.SelectedItem;
            isBillOpen = true;
            await Navigation.PushModalAsync(new BillPage(selectTransaction.Transaction));
            listTransaction.SelectedItem = null;
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
            emptyFrame.IsVisible = false;
            listTransaction.IsVisible = true;
            statusTransaction.IsVisible = false;
            _upFrame = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2;
            LoadPage();
        }
    }
}