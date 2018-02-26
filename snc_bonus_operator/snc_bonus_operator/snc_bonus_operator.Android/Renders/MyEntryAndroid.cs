using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryAndroid))]
#pragma warning disable CS0618 // Type or member is obsolete
public class MyEntryAndroid : EntryRenderer
    {
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
#pragma warning restore CS0618 // Type or member is obsolete