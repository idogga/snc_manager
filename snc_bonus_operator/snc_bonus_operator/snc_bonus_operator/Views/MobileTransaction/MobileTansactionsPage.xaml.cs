
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace snc_bonus_operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MobileTansactionsPage : ContentPage
    {
        public MobileTansactionsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            context.LoadTransactions(0);
        }

        private void listTransaction_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}