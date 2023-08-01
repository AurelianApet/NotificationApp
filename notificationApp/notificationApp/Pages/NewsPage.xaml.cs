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
	public partial class NewsPage : ContentPage
	{
        public bool clicked = false;

		public NewsPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            InitUI();
            LoadNews();
        }

        public void InitUI()
        {

        }

        public void LoadNews()
        {
            Constant.Instance.LoadNews();
            foreach (News item in Constant.Instance.newsLst)
            {
                StackLayout layout = BuildRowNews(item);
                newsContainer.Children.Add(layout);
            }
        }

        public StackLayout BuildRowNews(News item)
        {
            StackLayout layout = new StackLayout
            {
                Margin = new Thickness(10, 10, 10, 10),
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
                News news = null;
                foreach (News itemNews in Constant.Instance.newsLst)
                {
                    if (itemNews.id == id)
                        news = itemNews;
                }
                Constant.Instance.currentNews = news;
                Navigation.PushAsync(new Pages.NewsDetailPage());
                ClosePage();
            };
            Label dateLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(10, 10, 10, 0),
                TextColor = Color.White,
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
                Source = "userChat.png",
                WidthRequest = 70,
            };
            Image arrow = new Image
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Source = "arrow.png",
                HeightRequest = 35,
                Margin = new Thickness(0, 20, -20, 0),
                WidthRequest = 45,
            };

            Label titleContentLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Text = item.title,
                FontSize = 16,
                WidthRequest = 320,
                BackgroundColor = Color.White,
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
                if (subPage.GetType() == typeof(NewsPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }
    }
}