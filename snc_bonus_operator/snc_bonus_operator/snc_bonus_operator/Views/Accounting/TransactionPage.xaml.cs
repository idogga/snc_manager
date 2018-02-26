using Newtonsoft.Json;
using snc_bonus_operator.Cash;
using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Accounting
{
    public partial class TransactionPage : ContentPage
    {
        ObservableCollection<AllTransactionView> Transactions { get; set; } = new ObservableCollection<AllTransactionView>();
        SellerTransactionInfo transactions = new SellerTransactionInfo() { From = new DateTime(2002, 10, 23), To = DateTime.Now,
         TransactionStatuses=new List<SellerTransactionInfo.SELLER_STATUS_ENUM>(){ SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted,
          SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted, SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept, SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration}
        };

        UserCard card = new UserCard();
        bool isLoading;
        bool needToLoad = true;
        const int _amount = 20;
        List<int> listOfPosKeys = new List<int>();
        bool mayFirstLoad = false;
        bool needToReload = true;

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
                if ((e.Item == Transactions[Transactions.Count - 1]))
                {
                    if (needToLoad)
                    {
                        StartLoading();
                        var cancellationTokenSource = new CancellationTokenSource();
                        try
                        {
                            await Task.Factory.StartNew(x =>
                            {
                                LoadTransactions(Transactions.Count);
                            },
                            TaskCreationOptions.AttachedToParent, cancellationTokenSource.Token);
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
                    else
                    {
                        statusTransaction.Text = "Вся история загружена";
                        statusTransaction.IsVisible = true;
                    }
                }

            };
            FilterPage.EndEvent += LoadFilter;
            {
                mayFirstLoad = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (needToReload)
            {
                StartLoading();
                Transactions.Clear();
                listTransaction.IsVisible = true;
                statusTransaction.IsVisible = false;
                if (mayFirstLoad)
                    LoadPage();
            }
            else
            {
                needToReload = true;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        void LoadFilter(SellerTransactionInfo info)
        {
            transactions.From = info.From;
            transactions.To = info.To;
            transactions.Amount = _amount;
            transactions.Offset = 0;
            transactions.TransactionStatuses = info.TransactionStatuses;
        }

        async void LoadPage()
        {
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

                foreach (var item in transactions.Transactions)
                {
                    try
                    {
                        var transactionView = new AllTransactionView(item);
                        transactionView.Transaction = item;
                        Transactions.Add(transactionView);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(ex);
                    }
                }
                needToLoad = transactions.Transactions.Count == _amount;
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (IsVisible)
                        {
                            listTransaction.ItemsSource = Transactions;
                        }
                    }
                    catch
                    { }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                Device.BeginInvokeOnMainThread(() =>
                {
                    HideList();
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
            statusTransaction.Text = "Покупок еще не было";
            statusTransaction.IsVisible = true;
        }

        private async void ToolbarFilterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FilterPage(transactions));
        }

        private async void listTransaction_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var selected = (AllTransactionView)e.SelectedItem;
            
            var basket = new ShopBasket()
            {
                BonusCountIn = selected.Transaction.BonusIn,
                BonusCountOut = selected.Transaction.BonusOut,
                Discount = selected.Transaction.Discount,
                FinalPrice = selected.Transaction.PersonCost,
                TimeComplete = selected.Transaction.CompleteDatetime,
                TotalPrice = selected.Transaction.ShopBaseCost,
                UserProgrammName = selected.Transaction.UserProgrammName,
                GraphicalNumber=selected.Transaction.GraphicalNumber,

            };
            needToReload = false;
            await Navigation.PushModalAsync(new BillPage(basket, false));

            listTransaction.SelectedItem = null;
        }
    }
}