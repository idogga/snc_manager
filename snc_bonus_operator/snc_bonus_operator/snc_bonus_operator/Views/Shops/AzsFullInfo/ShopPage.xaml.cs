
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace snc_bonus_operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShopPage : ContentPage
	{
        private ShopView _shop;

		public ShopPage (ShopView selected)
		{
            _shop = selected;
			InitializeComponent ();
            context.SetContext(selected);
            configuration.SetContext(selected);
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            configuration.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            configuration.OnDisappearing();
        }
    }
}