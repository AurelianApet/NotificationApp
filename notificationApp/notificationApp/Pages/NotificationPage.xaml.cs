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
	public partial class NotificationPage : ContentPage
	{
        public string webViewHtml { get; set; }
        public bool clicked = false;

        public NotificationPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            InitUI();
            LoadNotification();
        }

        public void LoadNotification()
        {
            Constant.Instance.LoadNotifications();
            foreach (Notification item in Constant.Instance.notificationLst)
            {
                StackLayout layout = BuildRowNotification(item);
                notificationContainer.Children.Add(layout);
            }
        }

        public StackLayout BuildRowNotification(Notification item)
        {
            StackLayout layout = new StackLayout {
                Margin = new Thickness(10,10,10,10),
                MinimumHeightRequest = 200,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
            };
            TapGestureRecognizer layoutGestureRecognizer = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 2,
            };
            layoutGestureRecognizer.Tapped += (s, e) => {
                Label obj = (Label)s;
                int id = int.Parse(obj.StyleId);
                Notification notification = null;
                foreach (Notification itemNotification in Constant.Instance.notificationLst)
                {
                    if (itemNotification.id == id)
                        notification = itemNotification;
                }
                Constant.Instance.currentNotification = notification;
                Navigation.PushAsync(new Pages.NotificationDetailPage());
                ClosePage();
            };
            //layout.GestureRecognizers.Add(layoutGestureRecognizer);
            Label dateLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(10, 10, 10, 0),
                TextColor= Color.White,
                Text = item.date,
                FontSize = 18
            };
            StackLayout titleContentLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 0,            
            };
            Image icon = new Image
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(20, 0, 0, 0),
                HeightRequest = 70,                
                Source= "userChat.png",
                WidthRequest = 70,
            };
            Image arrow = new Image
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Source = "arrow.png",
                HeightRequest = 35,
                Margin = new Thickness(0,20,-20,0),                
                WidthRequest = 45,
            };
            Label titleContentLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Text = item.title,
                TextColor = Color.Black,
                BackgroundColor = Color.White,
                FontSize = 16,
                WidthRequest = 320,
                HeightRequest = 150,
                StyleId = item.id.ToString(),
            };
            titleContentLabel.GestureRecognizers.Add(layoutGestureRecognizer);
            titleContentLayout.Children.Add(icon);
            titleContentLayout.Children.Add(arrow);
            titleContentLayout.Children.Add(titleContentLabel);
            layout.Children.Add(dateLabel);
            layout.Children.Add(titleContentLayout);
            return layout;
        }

        public void InitUI()
        {
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
                if (subPage.GetType() == typeof(NotificationPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }
    }
}