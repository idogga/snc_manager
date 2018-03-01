using snc_bonus_operator.Protocol;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class CellModel
    {
        public string BonusLabel { get; set; } = "";
        public string Cost { get; set; } = "";
        public string CompleteDate { get; set; } = "";
        public int ID { get; set; } = 0;
        public string Time { get; set; } = "";
        public string TopLabel { get; set; } = "";
        public bool IsChecked { get; set; } = false;
        public SellerTransaction Transaction { get; set; }

        public CellModel(SellerTransaction transaction)
        {
            Transaction = transaction;
            ID = transaction.TransactionKey;
            TopLabel = transaction.SellerName;
            BonusLabel = transaction.BonusIn.ToString("0.00");
            Cost = transaction.ShopBaseCost.ToString("0.00");
            Time= string.Format("{0:HH:mm}", transaction.CompleteDatetime);
        }

        private string GetTopLabel(string name)
        {
            var result = name;
            if (name.Length > 13)
            {
                result = name.Substring(0, 10);
                result += "...";
            }
            return result;
        }
    }

    public class DateGroup : List<CellModel>
    {
        public string Heading { get; set; } = "";
        public Thickness Frame { get; set; } = new Thickness(5, 40, 10, 5);
        public DateTime ComleteDate { get; set; } = DateTime.MinValue;
        public List<CellModel> ListCell { get; set; } = new List<CellModel>();

        public DateGroup(DateTime completeDateTime, double UpFrame, bool isFirst)
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
            switch(dayOfWeek)
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
