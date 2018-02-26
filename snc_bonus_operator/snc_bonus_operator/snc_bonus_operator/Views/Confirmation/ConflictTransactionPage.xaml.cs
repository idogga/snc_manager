using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace snc_bonus_operator.Confirmation
{
    public partial class ConflictTransactionPage : ContentPage
    {
        ObservableCollection<AllTransactionView> Transactions { get; set; } = new ObservableCollection<AllTransactionView>();

        public ConflictTransactionPage(List<SellerTransaction> list, bool answer)
        {
            InitializeComponent();
            string managerOpinion = (answer) ? "(Вы выбрали ОДОБРИТЬ)" : "(Вы выбрали ОТКЛОНИТЬ)";
            foreach (var item in list)
            {
                var transactionView = new AllTransactionView(item, managerOpinion);
                Transactions.Add(transactionView);
            }
            listTransaction.ItemsSource = Transactions;
        }

        private async void continueButtton_CLicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}