using Android.Content;
using Android.OS;
using Xamarin.Forms;
using notificationApp;
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Util;

namespace BackgroundTasks.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");
            wakeLock.Acquire();
            Constant.Instance.GetGeolocationAndExtrInfo();
            wakeLock.Release();
        }
    }
}