using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;
using System.Threading.Tasks;
using notificationApp.Droid;
using BackgroundTasks.Droid;
using Xamarin.Forms;
using Android.Telephony;

[assembly: Xamarin.Forms.Dependency(typeof(notificationApp.Droid.DeviceInfo))]
namespace notificationApp.Droid
{
    //[Activity(Label = "notificationApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "notificationApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Constant.Instance.token = FirebaseInstanceId.Instance.Token;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            IsPlayServicesAvailable();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            //background service start
            var alarmIntent = new Intent(this, typeof(BackgroundReceiver));

            var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            long timeInMillis = Java.Util.Calendar.Instance.TimeInMillis;
            alarmManager.SetRepeating(AlarmType.RtcWakeup, timeInMillis,1000,pending);
            //
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public bool IsPlayServicesAvailable()
        {
            string str = string.Empty;
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    str = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    str = "This device is not supported";
                    Finish();
                }
                Toast.MakeText(this, str, ToastLength.Long);
                Log.Debug("Notification", str);
                return false;
            }
            else
            {
                str = "Google Play Services is available.";
                Toast.MakeText(this, str, ToastLength.Long);
                Log.Debug("Notification", str);
                return true;
            }
        }

    }

    public class DeviceInfo : IDeviceInfo
    {
        public string GetPhoneNumber()
        {
            try
            {
                var tMgr = (TelephonyManager)Forms.Context.ApplicationContext.GetSystemService(Android.Content.Context.TelephonyService);
                return tMgr.Line1Number;
            }
            catch (Exception err)
            {
                return "";
            }
        }
    }

}