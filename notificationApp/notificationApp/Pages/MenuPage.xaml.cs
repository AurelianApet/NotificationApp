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
	public partial class MenuPage : ContentPage
	{
        public bool clicked = false;

		public MenuPage ()
		{
			InitializeComponent ();
            InitUI();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void InitUI()
        {
            labUserName.Text = Constant.Instance.userName;
            btnNewsHistory.Clicked += (s, e) => {
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new NewsPage());
                ClosePage();
                clicked = false;
            };
            btnNotificationHistory.Clicked += (s, e) => {
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new NotificationPage());
                ClosePage();
                clicked = false;
            };

            btnSendNotification.Clicked += (s, e) => {
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new NotificationSendPage());
                ClosePage();
                clicked = false;
            };
            btnLogout.Clicked += (s, e) => {
                if (clicked)
                    return;
                clicked = true;
                //remove session
                Application.Current.Properties["UserName"] = null;
                Application.Current.Properties["Password"] = null;
                //
                Navigation.PushAsync(new LoginPage());
                ClosePage();
                clicked = false;
            };
            List<Menu> menuBtns = Constant.Instance.GetMenuList();
            foreach (Menu btn in menuBtns)
            {
                MenuBtnAdd(btn);
            }
        }

        public void MenuBtnAdd(Menu btnInfo)
        {
            StackLayout btnContainer = new StackLayout() { HorizontalOptions = LayoutOptions.CenterAndExpand,VerticalOptions = LayoutOptions.CenterAndExpand,Orientation = StackOrientation.Vertical};
            ImageButton imageBtn = new ImageButton() {HorizontalOptions = LayoutOptions.CenterAndExpand,VerticalOptions = LayoutOptions.CenterAndExpand,WidthRequest = 80,HeightRequest = 80,Source = "sendBtn.png",BackgroundColor=Color.Transparent};
            imageBtn.Clicked += (s, e) => {
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new CustomUserPage(btnInfo));
                ClosePage();
                clicked = false;
            };
            Label btnLabel = new Label() { HorizontalOptions = LayoutOptions.CenterAndExpand,VerticalOptions = LayoutOptions.CenterAndExpand,TextColor = Color.White,Text = btnInfo.menuName};
            btnContainer.Children.Add(imageBtn);
            btnContainer.Children.Add(btnLabel);
            menuBntContainer.Spacing = 20;
            menuBntContainer.Children.Add(btnContainer);
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
                if (subPage.GetType() == typeof(MenuPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }

    }
}