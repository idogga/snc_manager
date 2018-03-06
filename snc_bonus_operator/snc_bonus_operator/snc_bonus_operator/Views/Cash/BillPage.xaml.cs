using snc_bonus_operator.Protocol;
using System;

using Xamarin.Forms;

namespace snc_bonus_operator.Cash
{
    public partial class BillPage : ContentPage
    {
        public BillPage(ShopBasket basket, bool showShortData = true)
        {
            InitializeComponent();
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Время покупки : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.TimeComplete.ToString("HH:MM:ss"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            dateLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Номер карты : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.GraphicalNumber, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            grapicalNumber.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Цена без учета скидок и бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.TotalPrice.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            summLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Программа : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.UserProgrammName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            tariffLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Списано бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.BonusCountOut.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            bonusCountOutLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Начислено бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.BonusCountIn.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            bonusCountInLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Скидка : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.Discount.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = " "+MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            discountLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "ИТОГО : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = basket.FinalPrice.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = " "+MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            finalCashLabel.FormattedText = formatted;
        }

        public BillPage(SellerTransaction transaction)
        {
            InitializeComponent();
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Время покупки : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.CompleteDatetime.ToString("HH:MM:ss dd.mm.yyyy"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            dateLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Номер карты : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.GraphicalNumber, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            grapicalNumber.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Цена без учета скидок и бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.ShopBaseCost.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            summLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Программа : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.UserProgrammName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            tariffLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Списано бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.BonusOut.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            bonusCountOutLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Начислено бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.BonusIn.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            bonusCountInLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Скидка : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.Discount.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            discountLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "ИТОГО : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            formatted.Spans.Add(new Span { Text = transaction.PersonCost.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
            formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
            finalCashLabel.FormattedText = formatted;
            switch (transaction.StatusTransaction)
                {
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted:
                    acceptedLabel.IsVisible = true;
                    formatted = new FormattedString();
                    formatted.Spans.Add(new Span { Text = "Акцептовал : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                    formatted.Spans.Add(new Span { Text = transaction.AcceptedSellerName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                    formatted.Spans.Add(new Span { Text = " (" + transaction.AcceptedDate + ")", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(false) });
                    acceptedLabel.FormattedText = formatted;
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted:
                    acceptedLabel.IsVisible = true;
                    formatted = new FormattedString();
                    formatted.Spans.Add(new Span { Text = "Начисление бонусов отклонено", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                    acceptedLabel.FormattedText = formatted;
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration:
                    acceptedLabel.IsVisible = true;
                    formatted = new FormattedString();
                    formatted.Spans.Add(new Span { Text = "Начисление бонусов находится на рассмотрении", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold, FontFamily = MobileStaticVariables.UserAppSettings.CurrenFont(true) });
                    acceptedLabel.FormattedText = formatted;
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept:
                default:
                    acceptedLabel.IsVisible = false;
                    break;
            }
        }

        private async void OnGoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}