using snc_bonus_operator.Protocol;
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

        public override string ToString()
        {
            return $"#{Transaction.TransactionKey} СУММ [{BaseCost}] СКИДКА [{Discount}] Б.Начислено[{BonusIn}] Б.Списано[{BonusOut}]";
        }
    }
    
}
