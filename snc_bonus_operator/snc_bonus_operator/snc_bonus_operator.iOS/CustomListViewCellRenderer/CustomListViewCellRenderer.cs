using snc_bonus_operator.Ios.DependencyServices;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using snc_bonus_operator.DepServices;

[assembly: ExportRenderer(typeof(CustomListViewCell), typeof(CustomListViewCellRenderer))]
namespace snc_bonus_operator.Ios.DependencyServices
{
    public class CustomListViewCellRenderer : ViewCellRenderer
    {
#warning Метод не работает
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            //UITableViewCell cell = new UITableViewCell();
            return reusableCell;
        }

#warning Метод не работает
        void OnNativeCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }
    }
}