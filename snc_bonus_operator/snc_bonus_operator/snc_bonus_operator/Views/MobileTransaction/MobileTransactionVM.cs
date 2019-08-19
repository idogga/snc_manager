using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class MobileTransactionVM : CommonVM
    {
        public ObservableCollection<DateGroupTransaction> Transactions { get; set; } = new ObservableCollection<DateGroupTransaction>();
        private SellerTransactionInfo transactions = new SellerTransactionInfo()
        {
            From = new DateTime(2002, 10, 23),
            To = DateTime.Now,
            TransactionStatuses = new List<SellerTransactionInfo.SELLER_STATUS_ENUM>(){ SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted,
          SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted, SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept, SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration}
        };

        public Task LoadTransactions(int offset)
        {
            return Task.Factory.StartNew(() =>
{
    try
    {
        IsLoadVisible = true;
        transactions.Amount = 10;
        transactions.DeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
        transactions.Offset = offset;
        Logger.WriteLine($"Загрузка мобильных покупок в колличестве {transactions.Amount}, задержка {transactions.Offset}");
        transactions = MobileStaticVariables.WebUtils.SendMobileRequest<SellerTransactionInfo>(RequestTagEnum.AllMobileTransactions, transactions);
        if (transactions.ResultState == RequestResult.Results.Success)
        {
            if (offset == 0 && transactions.Transactions.Count == 0)
            {
                Transactions.Clear();
            }
        }
        else
        {
            Transactions.Clear();
        }
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
            }
            Logger.WriteLine($"Загружено {transactions.SellerList.Count} покупок. Итого {Transactions.Sum(x=>x.Count)} покупок");
            OnPropertyChanged("Transactions");
        });
    }
    catch (Exception ex)
    {
        Logger.WriteError(ex);
        Device.BeginInvokeOnMainThread(() =>
        {
            Transactions.Clear();
            Device.StartTimer(new TimeSpan(0, 0, 1), WaitInternetConnection);
        });
    }
    finally
    {
        IsLoadVisible = false;
    }
});
        }
        private bool WaitInternetConnection()
        {
            var result = true;
            if (MobileStaticVariables.UserAppSettings.IsInetAvaliable == Settings.InternetStatus.Online)
            {
                result = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Transactions.Clear();
                    LoadTransactions(0);
                });
            }
            return result;
        }
    }
}
