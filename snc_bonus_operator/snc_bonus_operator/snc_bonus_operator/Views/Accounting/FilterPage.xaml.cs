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
        //ObservableCollection<SellerInfo> Sellers { get; set; } = new ObservableCollection<SellerInfo>();

        #region Конструктор
        public FilterPage(SellerTransactionInfo _filter)
        {
            filter = _filter;
            InitializeComponent();
            dateFromPicker.Date = filter.From;
            dateToPicker.Date = filter.To;
            personButton.IsVisible = MobileStaticVariables.UserInfo.UserType == UserTypes.Admin;
            foreach (var item in MobileStaticVariables.UserInfo.Stuff)
            {
                var frame = new Frame() { Style = (Style)App.Current.Resources["UsualFrameStyle"] };
                var stackHorizontal = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
                var stackVertical = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand };
                var oper_nam = new Label() { Text = item.Name, Style = (Style)App.Current.Resources["UsualLabelStyle"],  StyleId = item.MobileManagerKey.ToString() };
                stackVertical.Children.Add(oper_nam);
                var oper_pos = new Label() { Text = item.Position, Style = (Style)App.Current.Resources["SubLabelStyle"] };
                stackVertical.Children.Add(oper_pos);
                stackHorizontal.Children.Add(stackVertical);
                var checkbox = new Checkbox() { HorizontalOptions = LayoutOptions.EndAndExpand, IsEnabled = false, Color = (Color)App.Current.Resources["MainColor"], ScaleCheckbox = 2.0 };
                checkbox.IsChecked = filter.SellerList.Contains(int.Parse(oper_nam.StyleId));
                stackHorizontal.Children.Add(checkbox);
                frame.Content = stackHorizontal;
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    if(filter.SellerList.Contains(int.Parse(oper_nam.StyleId)))
                    {
                        checkbox.IsChecked = false;
                        filter.SellerList.Remove(int.Parse(oper_nam.StyleId));
                    }
                    else
                    {
                        checkbox.IsChecked = true;
                        filter.SellerList.Add(int.Parse(oper_nam.StyleId));
                    }
                };
                frame.GestureRecognizers.Add(tap);
                listSellers.Children.Add(frame);
            }
        }
        #endregion

        #region События
        protected override void OnDisappearing()
        {
            GetStatuses();
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
            sellerSelector.BackgroundColor = new Color(0, 0, 0, 0.5);
            sellerSelector.IsVisible = true;
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

        private void GetStatuses()
        {
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

        #endregion

        private void acceptSellers(object sender, EventArgs e)
        {
            sellerSelector.IsVisible = false;
        }
    }
}