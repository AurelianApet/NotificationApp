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
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadingNewsNotification();
            btnTest.Clicked += (s, e) => {
                Navigation.PushAsync(new MenuPage());
            };
        }

        public void LoadingNewsNotification()
        {
            
            //Task.Delay(5000);
        }
	}
}