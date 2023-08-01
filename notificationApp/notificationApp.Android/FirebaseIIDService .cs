using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Xamarin.Forms;

namespace notificationApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            string refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug("FirebaseIIDService ", "Refreshed token: " + refreshedToken);

            SaveRefreshedToken(refreshedToken);
        }

        private void SaveRefreshedToken(string refreshedToken)
        {
            if(refreshedToken != "")
                Xamarin.Forms.Application.Current.Properties["token"] = refreshedToken;
        }
    }
}