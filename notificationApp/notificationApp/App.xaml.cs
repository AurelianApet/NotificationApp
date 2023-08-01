using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.DeviceInfo;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace notificationApp
{
    public interface IDeviceInfo
    {
        string GetPhoneNumber();
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitGlobalVariable();
            if (Constant.Instance.CheckLogin())
            {
                Constant.Instance.LoadNotificationStack();
                if (Constant.Instance.currentIndex == -1)
                    MainPage = new NavigationPage(new Pages.MenuPage());
                else
                    MainPage = new NavigationPage(new Pages.NotificationConfirmPage());
            }
            else
                MainPage = new NavigationPage(new Pages.LoginPage());
        }

        public void InitGlobalVariable()
        {
            switch (CrossDeviceInfo.Current.Platform)
            {
                case Plugin.DeviceInfo.Abstractions.Platform.Android:
                    Constant.Instance.platform = "Android";
                    var deviceInfo = Xamarin.Forms.DependencyService.Get<notificationApp.IDeviceInfo>();
                    Constant.Instance.phoneNumber = deviceInfo.GetPhoneNumber();
                    break;
                case Plugin.DeviceInfo.Abstractions.Platform.iOS:
                    Constant.Instance.platform = "iOS";
                    Constant.Instance.phoneNumber = "";
                    break;
                default:
                    Constant.Instance.platform = "Unknown";
                    break;
            }
            Constant.Instance.model = CrossDeviceInfo.Current.Model;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
