using ObjCRuntime;
using snc_bonus_operator.Ios.DependencyServices;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRender))]
namespace snc_bonus_operator.Ios.DependencyServices
{
    public class MyEntryRender : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var nativeTextField = (UITextField)Control;
                nativeTextField.EditingDidBegin += (object sender, EventArgs eIos) =>
                {
                    nativeTextField.PerformSelector(new Selector("selectAll"), null, 0.0f);
                };
            }
        }
    }
}
