
using snc_bonus_operator.Cash;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace snc_bonus_operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MobileTansactionsPage : ContentPage
    {
        public MobileTansactionsPage()
        {
            InitializeComponent();
            listTransaction.ItemAppearing += (sender, e) =>
            {
                Logger.WriteLine($"Показывается {e.Item}");
                if (context.IsLoadVisible || context.Transactions.Count == 0)
                    return;
                var last = context.Transactions.LastOrDefault().LastOrDefault();
                if (last is AllTransactionView lastView)
                {
                    if (e.Item is AllTransactionView currentView)
                    {
                        if (lastView.ID == currentView.ID)
                        {
                            if (context.Transactions.Sum(x => x.Count) % 10 == 0)
                            {
                                context.LoadTransactions(context.Transactions.Sum(x => x.Count));
                            }
                            else
                            {
                                statusTransaction.Text = "Вся история загружена";
                                statusTransaction.IsVisible = true;
                            }
                        }
                    }
                    else if (e.Item is List<AllTransactionView> current)
                    {
                        if (lastView.ID == current.LastOrDefault().ID)
                        {
                            if (context.Transactions.Sum(x => x.Count) % 10 == 0)
                            {
                                context.LoadTransactions(context.Transactions.Sum(x => x.Count));
                            }
                            else
                            {
                                statusTransaction.Text = "Вся история загружена";
                                statusTransaction.IsVisible = true;
                            }
                        }
                    }
                }

            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            context.LoadTransactions(0);
        }

        private async void listTransaction_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var selectTransaction = (AllTransactionView)e.SelectedItem;
            await Navigation.PushModalAsync(new BillPage(selectTransaction.Transaction));
            ((ListView)sender).SelectedItem = null;
        }
    }
}