using snc_bonus_operator.DepServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using System.ComponentModel;
using snc_bonus_operator.Droid.Renders;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomListViewCell), typeof(CustomListViewCellRenderer))]
namespace snc_bonus_operator.Droid.Renders
{
    public class CustomListViewCellRenderer : ViewCellRenderer
    {
        private global::Android.Views.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;
        Xamarin.Forms.Color xfSelectColor = (Xamarin.Forms.Color)snc_bonus_operator.App.Current.Resources["SelectionColor"];

        protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);

            _selected = false;
            _unselectedBackground = _cellCore.Background;
            _cellCore.DrawingCacheBackgroundColor = Color.Transparent.ToAndroid();
            return _cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                _selected = !_selected;

                if (_selected)
                {
                    var extendedViewCell = sender as CustomListViewCell;
                    _cellCore.SetBackgroundColor(xfSelectColor.ToAndroid());
                    _cellCore.DrawingCacheBackgroundColor = global::Android.Graphics.Color.Transparent;
                }
                else
                {
                    _cellCore.SetBackground(_unselectedBackground);
                }
            }
        }

    }
}