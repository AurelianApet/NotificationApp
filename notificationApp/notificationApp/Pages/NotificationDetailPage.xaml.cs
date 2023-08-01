using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace notificationApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationDetailPage : ContentPage
	{
        public bool clicked = false;

		public NotificationDetailPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            InitUI();
            LoadNotification();
		}

        public void LoadNotification()
        {
            Notification current = Constant.Instance.currentNotification;
            labTitle.Text = current.title;
            webContent.Source = new HtmlWebViewSource
            {
                Html = current.content,
                BaseUrl = Constant.Instance.serverDomain,
            };
        }

        public void WebView_Navigating(object sender, WebNavigatingEventArgs args)
        {
            if (args.Url.StartsWith("file://") || args.Url.Contains(Constant.Instance.serverDomain))
            {
                return;
            }

            Device.OpenUri(new Uri(args.Url));

            args.Cancel = true;
        }

        public void InitUI()
        {
            btnBack.Clicked += (s, e) => {
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new NotificationPage());
                ClosePage();
                clicked = false;
            };
            webContent.Navigating += WebView_Navigating;
        }

        protected override bool OnBackButtonPressed()
        {
            if (clicked)
                return true;
            clicked = true;
            Navigation.PushAsync(new NotificationPage());
            ClosePage();
            clicked = false;
            return true;
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
                if (subPage.GetType() == typeof(NotificationDetailPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }
    }
}