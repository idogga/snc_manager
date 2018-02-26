using snc_bonus_operator.Protocol;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace snc_bonus_operator.Accounting
{
    public delegate void FilterDelegate(SellerTransactionInfo info);
    public partial class FilterPage : ContentPage
    {
        public static event FilterDelegate EndEvent;
        SellerTransactionInfo filter;
        ObservableCollection<SellerInfo> Sellers { get; set; } = new ObservableCollection<SellerInfo>();

        #region Конструктор
        public FilterPage(SellerTransactionInfo _filter)
        {
            filter = _filter;
            InitializeComponent();
            dateFromPicker.Date = filter.From;
            dateToPicker.Date = filter.To;
            personButton.IsVisible = MobileStaticVariables.UserInfo.UserType == UserTypes.Admin;
        }

        #endregion

        #region События
        protected override void OnDisappearing()
        {
            EndEvent.Invoke(filter);
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            acceptCheckbox.IsChecked = filter.TransactionStatuses.Contains(SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted);
            notNeedAcceptCheckbox.IsChecked = filter.TransactionStatuses.Contains(SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept);
            notAcceptCheckbox.IsChecked = filter.TransactionStatuses.Contains(SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted);
            mobileCheckbox.IsChecked = filter.TransactionStatuses.Contains(SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration);
        }

        private void personButton_Clicked(object sender, EventArgs e)
        {
            sellerSelector.IsVisible = true;
            foreach(var item in MobileStaticVariables.UserInfo.Stuff)
            {
                Sellers.Add(new SellerInfo(item.MobileManagerKey, item.Name, item.Position, filter.SellerList.Contains(item.MobileManagerKey)));
            }
            listSellers.ItemsSource = Sellers;
        }

        private async void exitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void dateToPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            filter.To = dateToPicker.Date;
            dateFromPicker.MaximumDate = dateToPicker.Date;
        }

        private void dateFromPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            filter.From = dateFromPicker.Date;
            dateToPicker.MinimumDate = dateFromPicker.Date;
        }

        private void acceptstatustransaction(object sender, EventArgs e)
        {
            statusTransactionSelector.IsVisible = false;
            filter.TransactionStatuses.Clear();
            if (notNeedAcceptCheckbox.IsChecked)
            {
                filter.TransactionStatuses.Add(SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept);
            }
            if (acceptCheckbox.IsChecked)
            {
                filter.TransactionStatuses.Add(SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted);
            }
            if (notAcceptCheckbox.IsChecked)
            {
                filter.TransactionStatuses.Add(SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted);
            }
            if (mobileCheckbox.IsChecked)
            {
                filter.TransactionStatuses.Add(SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration);
            }
        }

        private void selectorOpen(object sender, EventArgs e)
        {
            statusTransactionSelector.BackgroundColor = new Color(0, 0, 0, 0.5);
            statusTransactionSelector.IsVisible = true;
        }


        #endregion

        private void acceptSellers(object sender, EventArgs e)
        {
            sellerSelector.IsVisible = false;
        }

        private void SellerSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var selectTransaction = (SellerInfo)e.SelectedItem;
            if (selectTransaction.IsChecked)
            {
                filter.SellerList.Remove(selectTransaction.ID);
                selectTransaction.IsChecked = false;
            }
            else
            {
                filter.SellerList.Add(selectTransaction.ID);
                selectTransaction.IsChecked = true;
            }
            listSellers.ItemsSource = Sellers;
            listSellers.SelectedItem = null;
        }

        class SellerInfo
        {
            public string OperNameLabel { get; set; } = "";
            public string OperDescLabel { get; set; } = "";
            public bool IsChecked { get; set; } = true;
            public int ID { get; set; } = 0;

            public SellerInfo(int id, string name, string description, bool containsInList)
            {
                ID = id;
                OperNameLabel = name;
                OperDescLabel = description;
                IsChecked = containsInList;
            }
        }
    }
}