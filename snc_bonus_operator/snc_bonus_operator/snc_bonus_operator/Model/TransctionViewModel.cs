using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Model
{
    //[PropertyChanged.ImplementPropertyChanged]
    public class TransctionViewModel
    {
        public FormattedString BottomFormattedString { get; set; } = new FormattedString();
        public string CompleteDate { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public int ID { get; set; } = 0;
        public string TopLabel { get; set; } = "";

        public TransctionViewModel(FormattedString bottomFormattedString, string completeDate, string cardNumber, string topLabel, int iD)
            {
            this.BottomFormattedString = bottomFormattedString;
            this.CompleteDate = completeDate;
            this.CardNumber = cardNumber;
            this.TopLabel = topLabel;
            this.ID = iD;
        }
    }
}
