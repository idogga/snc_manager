using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class DateGroupTransaction : List<AllTransactionView>
    {
        public string Heading { get; set; } = "";
        public Thickness Frame { get; set; } = new Thickness(5, 40, 10, 5);
        public DateTime ComleteDate { get; set; } = DateTime.MinValue;
        public List<AllTransactionView> ListCell { get; set; } = new List<AllTransactionView>();

        public DateGroupTransaction(DateTime completeDateTime)
        {
            ComleteDate = completeDateTime;
            Heading = $"{completeDateTime.ToString("dd.MM")}, {TranslateDayOfWeek(completeDateTime.DayOfWeek)}";
            Frame = new Thickness(5, 10, 10, 5);
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
