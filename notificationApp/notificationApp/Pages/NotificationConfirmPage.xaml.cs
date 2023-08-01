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
	public partial class NotificationConfirmPage : ContentPage
	{
        public NotificationHistory content = new NotificationHistory();
        public bool clicked = false;

		public NotificationConfirmPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            content = Constant.Instance.notificationHistoryLst.ElementAt(Constant.Instance.currentIndex);
            InitUI();
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
            labTitle.Text = content.notification.title;
            webContent.Source = new HtmlWebViewSource
            {
                Html = content.notification.content,
                BaseUrl = Constant.Instance.serverDomain,
            };
            webContent.Navigating += WebView_Navigating;
            btnSkip.Clicked += (s, e) => 
            {
                NavigationPages();
            };
            btnConfirm.Clicked += (s, e) =>
            {
                if (Constant.Instance.ConfirmNotification(content.recordId))
                    DisplayAlert("Success","You confirmed this notification.","Cancel");
                else
                    DisplayAlert("Fail", "This notification isn't confirmed", "Cancel");
                NavigationPages();
            };
        }

        public void NavigationPages()
        {
            int index = Constant.Instance.currentIndex;
            if (Constant.Instance.notificationHistoryLst.Count - 1 > index)
            {
                index++;
                content = Constant.Instance.notificationHistoryLst.ElementAt(index);
                labTitle.Text = content.notification.title;
                webContent.Source = new HtmlWebViewSource
                {
                    Html = content.notification.content,
                    BaseUrl = Constant.Instance.serverDomain,

                };
                Constant.Instance.currentIndex = index;
            }
            else
            {
                Navigation.PushAsync(new MenuPage());
                ClosePage();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (clicked)
                return true;
            clicked = true;
            Navigation.PushAsync(new MenuPage());
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
                if (subPage.GetType() == typeof(NotificationConfirmPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }


    }
}