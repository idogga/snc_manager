using System.ComponentModel;
using Xamarin.Forms;
using CheckboxCustomRenderer.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using snc_bonus_operator;
using System;
using Android.Content.Res;
using Android.Support.V4.Widget;
using Android.Content;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]
namespace CheckboxCustomRenderer.Droid
{
    public class CheckboxRenderer : ViewRenderer<Checkbox, CheckBox>
    {
        private CheckBox checkBox;
        public CheckboxRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);
            var model = e.NewElement;
            checkBox = new CheckBox(Context);
            checkBox.Tag = this;
            CheckboxPropertyChanged(model, null);
            checkBox.SetOnClickListener(new ClickListener(model));
            SetNativeControl(checkBox);
        }

        private void CheckboxPropertyChanged(Checkbox model, String propertyName)
        {
            if (propertyName == null || Checkbox.IsCheckedProperty.PropertyName == propertyName)
            {
                checkBox.Checked = model.IsChecked;
            }

            if (propertyName == null || Checkbox.ColorProperty.PropertyName == propertyName)
            {
                int[][] states =
                    {
                        new int[] { Android.Resource.Attribute.StateEnabled}, // enabled
                        new int[] { -Android.Resource.Attribute.StateEnabled }, // disabled
                        new int[] {Android.Resource.Attribute.StateChecked}, // unchecked
                        new int[] { Android.Resource.Attribute.StatePressed }  // pressed
                    };

                var checkBoxColor = (int)model.Color.ToAndroid();
                int[] colors = {
                        checkBoxColor,
                        checkBoxColor,
                        checkBoxColor,
                        checkBoxColor
                };
                CompoundButtonCompat.SetButtonTintList(checkBox, new ColorStateList(states, colors));
            }

            if (propertyName == null || Checkbox.ScaleCheckboxProperty.PropertyName == propertyName)
            {
                checkBox.ScaleX = (float)model.ScaleCheckbox;
                checkBox.ScaleY = (float)model.ScaleCheckbox;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (checkBox != null)
            {
                base.OnElementPropertyChanged(sender, e);

                CheckboxPropertyChanged((Checkbox)sender, e.PropertyName);
            }
        }

        public class ClickListener : Java.Lang.Object, IOnClickListener
        {
            private Checkbox _myCheckbox;
            public ClickListener(Checkbox myCheckbox)
            {
                this._myCheckbox = myCheckbox;
            }
            public void OnClick(global::Android.Views.View v)
            {
                _myCheckbox.IsChecked = !_myCheckbox.IsChecked;
            }
        }
    }
}