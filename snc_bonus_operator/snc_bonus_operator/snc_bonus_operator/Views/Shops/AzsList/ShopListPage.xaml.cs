
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class ShopListPage : ContentPage
    {
        public ShopListPage()
        {
            InitializeComponent();
            context.PropertyChanged += ContextPropertyChanged;
        }

        private void ContextPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(context.IsLoadVisible))
            {
                backgroundDark.IsVisible = context.IsLoadVisible;
                mainLayout.IsEnabled = !context.IsLoadVisible;
            }
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var selected = e.SelectedItem as ShopView;
            Logger.WriteLine($"Выбранна АЗС[{selected.ShopModel.ShopKey}] {selected.AzsName}");
            //Navigation.PushAsync(new ShopPage(selected));
            listView.SelectedItem = null;
        }

    }
}