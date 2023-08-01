using Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using notificationApp.Controls;
using Android.Content;
using Android.OS;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;

[assembly: ExportRenderer(typeof(xEntry), typeof(xEntryRender))]
namespace Mobile.Droid.Renderers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class xEntryRender : EntryRenderer
    {
        public xEntryRender(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}