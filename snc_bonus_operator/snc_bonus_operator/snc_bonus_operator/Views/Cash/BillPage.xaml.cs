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
            formatted.Spans.Add(new Span { Text = "Время покупки : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            if (showShortData)
            {
                formatted.Spans.Add(new Span { Text = basket.TimeComplete.ToString("HH:MM:ss"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            }
            else
            {
                formatted.Spans.Add(new Span { Text = basket.TimeComplete.ToString("HH:MM:ss dd.mm.yyyy"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            }
            dateLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Номер карты : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.GraphicalNumber, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            grapicalNumber.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Цена без учета скидок и бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.TotalPrice.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            summLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Программа : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.UserProgrammName, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            tariffLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Списано бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.BonusCountOut.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            bonusCountOutLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Начислено бонусов : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.BonusCountIn.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            bonusCountInLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Скидка : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.Discount.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            formatted.Spans.Add(new Span { Text = " "+MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            discountLabel.FormattedText = formatted;
            formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "ИТОГО : ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = basket.FinalPrice.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            formatted.Spans.Add(new Span { Text = " "+MobileStaticVariables.MainIssuer.Currency, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            finalCashLabel.FormattedText = formatted;
        }

        private async void OnGoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}