using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using notificationApp;

namespace notificationApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public bool clicked = false;

		public LoginPage ()
		{
			InitializeComponent ();
            InitializeUI();
		}

        public void ShowLoadingPopup()
        {
            popupImageView.IsVisible = true;
            activityIndicator.IsRunning = true;
        }

        public void CloseLoadingPopup()
        {
            popupImageView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private void btnLoginClicked(object sender,EventArgs e)
        {
            if (clicked)
                return;
            clicked = true;
            ShowLoadingPopup();
            string userName = entryUserName.Text;
            string pwd = entryPwd.Text;
            if (entryUserName.Text == null || entryPwd.Text == null)
            {
                userName = "";
                pwd = "";
            }
            if (userName == "" || pwd == "")
            {
                DisplayAlert("Warning", "Please input username or password in correctly.", "Cancel");
                CloseLoadingPopup();
                clicked = false;
                return;
            }

            bool loginResult = false;
            loginResult = Constant.Instance.CheckLogin(userName, pwd);
            Task.Delay(5000);
            CloseLoadingPopup();
            if (loginResult)
            {
                if (!Constant.Instance.SendNotificationKey())
                    DisplayAlert("Google Notification", "Please check Google notification setting.", "Cancel");
                Constant.Instance.LoadNotificationStack();
                if (Constant.Instance.currentIndex == -1)
                {
                    Navigation.PushAsync(new MenuPage());
                    ClosePage();
                    clicked = false;
                }
                else
                {
                    Navigation.PushAsync(new NotificationConfirmPage());
                    ClosePage();
                    clicked = false;
                }
            }
            else
            {
                string alertMsg = "";
                switch (Constant.Instance.loginStatus)
                {
                    case LoginStatus.LoginFailed:
                        alertMsg = "Please check your username or password";
                        break;
                    case LoginStatus.UnAllowed:
                        alertMsg = "You aren't unallowed by admin manager.Please contact to him.";
                        break;
                    case LoginStatus.UnKnow:
                        alertMsg = "Please check internet connection.";
                        break;
                    default:
                        alertMsg = "Please check internet connection.";
                        break;
                }
                DisplayAlert("Login Failed", alertMsg, "Cancel");
                clicked = false;
            }
        }

        public void InitializeUI()
        {
            NavigationPage.SetHasNavigationBar(this,false);
            //btnLogin.Clicked += (s,e) => {
                
            //};
        }

        protected override bool OnBackButtonPressed()
        {
            if (clicked)
                return true;
            clicked = true;
            Application.Current.Quit();
            clicked = false;
            return false;
        }

        public async void ClosePage()
        {
            MessagingCenter.Send<object>(this, "Release");
            //if (Navigation.ModalStack.Count != 0 && Navigation.ModalStack.Last().GetType() == typeof(AlarmPage))
            //{
            //    await Navigation.PopAsync(false);
            //}

            foreach (Page subPage in Navigation.NavigationStack)
            {
                if (subPage.GetType() == typeof(LoginPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }


    }
}