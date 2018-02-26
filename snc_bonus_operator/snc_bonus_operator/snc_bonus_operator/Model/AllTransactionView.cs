using snc_bonus_operator.Protocol;
using System;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class AllTransactionView
    {
        public SellerTransaction Transaction { get; set; } = new SellerTransaction();
        public string CardNumber { get; set; } = "";
        public string TopLabel { get; set; } = "";
        public string BottomLabel { get; set; } = "";
        public string CompleteDate { get; set; } = "";
        public Color BackColor { get; set; } = Color.Transparent;
        public FormattedString BottomFormattedString { get; set; } = new FormattedString();
        public int ID { get; set; } = 0;
        public FormattedString Accepted { get; set; } = new FormattedString();

        public AllTransactionView(SellerTransaction transaction, string opinion = "")
        {
            ID = transaction.TransactionKey;
            TopLabel = transaction.ShopName;
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = "Картa №", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            formatted.Spans.Add(new Span { Text = transaction.GraphicalNumber, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = ", покупка на сумму ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            formatted.Spans.Add(new Span { Text = transaction.PersonCost.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = " " + MobileStaticVariables.MainIssuer.Currency + ", начислено ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            formatted.Spans.Add(new Span { Text = transaction.BonusIn.ToString("0.00"), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
            formatted.Spans.Add(new Span { Text = " БОНУСОВ.", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
            BottomFormattedString = formatted;
            CompleteDate = string.Format("{0:HH:mm dd.MM.yyyy}", transaction.CompleteDatetime);
            CardNumber = string.Format("Оператор : {0}", transaction.SellerName);
            BottomFormattedString = formatted;
            formatted = new FormattedString();
            switch (transaction.StatusTransaction)
            {
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted:
                    BackColor = Color.FromHex("#F0FFF0");
                    formatted.Spans.Add(new Span { Text = "Транзакция ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
                    formatted.Spans.Add(new Span { Text = "ОДОБРЕНА ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
                    formatted.Spans.Add(new Span { Text = opinion, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted:
                    BackColor = Color.FromHex("#FFF5EE");
                    formatted.Spans.Add(new Span { Text = "Транзакция ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
                    formatted.Spans.Add(new Span { Text = "ОТКЛОНЕНА ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["MainColor"], FontAttributes = FontAttributes.Bold });
                    formatted.Spans.Add(new Span { Text = opinion, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration:
                    BackColor = Color.FromHex("#FFFFF0");
                    formatted.Spans.Add(new Span { Text = "(Транзакция находится на расмотрении)", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), ForegroundColor = (Color)App.Current.Resources["LetterColor"], FontAttributes = FontAttributes.None });
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept:
                default:
                    BackColor = Color.FromHex("#FFFFFF");
                    break;
            }
            Accepted = formatted;
        }
    }
}
