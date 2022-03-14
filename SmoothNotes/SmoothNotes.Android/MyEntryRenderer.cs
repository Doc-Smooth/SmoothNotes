using Xamarin.Forms.Platform.Android;
using SmoothNotes.Droid;
using SmoothNotes.Renders;
using Xamarin.Forms;
using Android.Content;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace SmoothNotes.Droid
{
    class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(global::Android.Graphics.Color.White);
            }
        }
    }
}