using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class AllTransactionView
    {
        public SellerTransaction Transaction { get; set; } = new SellerTransaction();
        public string BaseCost { get; set; } = "";
        public string Discount { get; set; } = "";
        public string BonusOut { get; set; } = "";
        public string Cost { get; set; } = "";
        public string Time { get; set; } = "";
        public string BonusIn { get; set; } = "";
        public Color BackColor { get; set; } = Color.Transparent;
        public int ID { get; set; } = 0;

        public AllTransactionView(SellerTransaction transaction, string opinion = "")
        {
            Transaction = transaction;
            ID = transaction.TransactionKey;
            BaseCost = transaction.ShopBaseCost.ToString("0.00");
            Discount = transaction.Discount.ToString("0.00");
            BonusOut = transaction.BonusOut.ToString("0.00");
            BonusIn = transaction.BonusIn.ToString("0.00");
            Cost = transaction.PersonCost.ToString("0.00");
            Time = string.Format("{0:HH:mm}", transaction.CompleteDatetime);
            switch (transaction.StatusTransaction)
            {
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Accepted:
                    BackColor = (Color)App.Current.Resources["AcceptColor"];
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Not_Accepted:
                    BackColor = (Color)App.Current.Resources["DeclineColor"];
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Under_Consideration:
                    BackColor = Color.FromHex("#ffffc0");
                    break;
                case (int)SellerTransactionInfo.SELLER_STATUS_ENUM.Not_NeedAccept:
                default:
                    BackColor = (Color)App.Current.Resources["ObjectBackgroundColor"];
                    break;
            }
        }
    }
    public class DateGroupTransaction : List<AllTransactionView>
    {
        public string Heading { get; set; } = "";
        public Thickness Frame { get; set; } = new Thickness(5, 40, 10, 5);
        public DateTime ComleteDate { get; set; } = DateTime.MinValue;
        public List<AllTransactionView> ListCell { get; set; } = new List<AllTransactionView>();

        public DateGroupTransaction(DateTime completeDateTime, double UpFrame, bool isFirst)
        {
            ComleteDate = completeDateTime;
            Heading = string.Format("{0:dd.MM}, {1}", completeDateTime, TranslateDayOfWeek(completeDateTime.DayOfWeek));
            if (isFirst)
            {
                Frame = new Thickness(5, 10, 10, 5);
            }
            else
            {
                Frame = new Thickness(5, UpFrame, 10, 5);
            }
        }

        private string TranslateDayOfWeek(DayOfWeek dayOfWeek)
        {
            var result = "";
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = "Понедельник";
                    break;
                case DayOfWeek.Friday:
                    result = "Пятница";
                    break;
                case DayOfWeek.Saturday:
                    result = "Суббота";
                    break;
                case DayOfWeek.Sunday:
                    result = "Воскресенье";
                    break;
                case DayOfWeek.Thursday:
                    result = "Четверг";
                    break;
                case DayOfWeek.Tuesday:
                    result = "Вторник";
                    break;
                case DayOfWeek.Wednesday:
                    result = "Среда";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
    }
}
