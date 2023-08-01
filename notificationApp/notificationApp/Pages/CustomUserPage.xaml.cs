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
	public partial class CustomUserPage : ContentPage
	{
        public bool clicked = false;
        public Menu content { get; set; }
		public CustomUserPage (Menu menu)
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            content = menu;
            InitUI();

        }

        public void InitUI()
        {
            labTitle.Text = content.menuName;
            webContent.Source = content.menuLink;
            btnBack.Clicked += (s, e) =>{
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new MenuPage());
                ClosePage();
                clicked = false;
            };
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
                if (subPage.GetType() == typeof(CustomUserPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }
    }
}