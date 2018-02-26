using System;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class Checkbox : View
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Checkbox), Color.Red);
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create("IsChecked", typeof(bool), typeof(Checkbox), false/*, propertyChanged:(s, o, n)=> { (s as Checkbox).OnChecked(new EventArgs()); }*/);
        public static readonly BindableProperty ScaleCheckboxProperty = BindableProperty.Create("ScaleCheckbox", typeof(double), typeof(Checkbox), 1.0);

        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public double ScaleCheckbox
        {
            get
            {
                return (double)GetValue(ScaleCheckboxProperty);
            }
            set
            {
                SetValue(ScaleCheckboxProperty, value);
            }
        }

        public event EventHandler Checked;

        protected virtual void OnChecked(EventArgs e)
        {
            Checked?.Invoke(this, e);
        }
    }
}
