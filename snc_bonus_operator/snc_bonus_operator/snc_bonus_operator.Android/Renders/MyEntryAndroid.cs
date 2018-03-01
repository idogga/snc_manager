using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryAndroid))]
public class MyEntryAndroid : EntryRenderer
{
    public MyEntryAndroid(Context context) : base(context)
    {

    }
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);
        if (e.OldElement == null)
        {
            var nativeEditText = (global::Android.Widget.EditText)Control;
            nativeEditText.SetSelectAllOnFocus(true);
        }
    }
}