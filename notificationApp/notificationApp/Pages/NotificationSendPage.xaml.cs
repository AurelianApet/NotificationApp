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
	public partial class NotificationSendPage : ContentPage
	{
        public bool clicked = false;

		public NotificationSendPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            InitUI();
		}

        public void InitUI()
        {
            Constant.Instance.LoadGroup();
            List<string> groupLst = new List<string>();
            foreach (Group item in Constant.Instance.groupLst)
                groupLst.Add(item.name);
            pickerGroups.ItemsSource = groupLst;
            pickerGroups.SelectedIndex = 0;
            btnBack.Clicked += (s, e) =>
            {
                if (clicked)
                    return;
                clicked = true;
                Navigation.PushAsync(new MenuPage());
                ClosePage();
                clicked = false;
            };
            btnSend.Clicked += (s, e) =>
            {
                string title = editorTitle.Text;
                string content = editorContent.Text;
                int groupId = Constant.Instance.groupLst.ElementAt(pickerGroups.SelectedIndex).id;

                if (Constant.Instance.SendNotification(title, content, groupId))
                {
                    DisplayAlert("Success", "Your notification is sent", "Cancel");
                    if (clicked)
                        return;
                    clicked = true;
                    Navigation.PushAsync(new MenuPage());
                    ClosePage();
                    clicked = false;
                }
                else
                {
                    DisplayAlert("Failed", "To send notification is failed", "Cancel");
                    editorTitle.Text = "";
                    editorContent.Text = "";
                }
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
                if (subPage.GetType() == typeof(NotificationSendPage))
                {
                    Application.Current.MainPage.Navigation.RemovePage(subPage);
                    break;
                }
            }
        }
    }
}