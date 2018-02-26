using Xamarin.Forms;

namespace snc_bonus_operator.DepServices
{
    public class ExtendedViewCell : ViewCell
    {
        //public static readonly BindableProperty SelectedBackgroundColorProperty =
        //    BindableProperty.Create("SelectedBackgroundColor",
        //                            typeof(Color),
        //                            typeof(ExtendedViewCell),
        //                            Color.Default);

        //public Color SelectedBackgroundColor
        //{
        //    get { return (Color)GetValue(SelectedBackgroundColorProperty); }
        //    set { SetValue(SelectedBackgroundColorProperty, value); }
        //}
        public ExtendedViewCell():base()
        {
            RelativeLayout layout = new RelativeLayout();
            layout.SetBinding(Layout.BackgroundColorProperty, new Binding("BackgroundColor"));

            View = layout;
        }

    }
}
