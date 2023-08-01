using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace notificationApp
{
    public enum LoginStatus
    {
        Logined,
        LoginFailed,
        UnAllowed,
        UnKnow,
    }

    public class Menu
    {
        public string menuName { get; set; }
        public string menuLink { get; set; }
        public Menu()
        {
            menuName = "";
            menuLink = "";
        }
    }

    public class Notification
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public string fromUser { get; set; }
        public string notificationToken { get; set; }
        public Notification()
        {
            id = 0;
            title = "";
            content = "";
            date = "";
            fromUser = "";
        }
    }

    public class NotificationHistory
    {
        public Notification notification { get; set; }
        public string recordId { get; set; }
        public NotificationHistory()
        {
            notification = null;
            recordId = "";
        }
    }

    public class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public News()
        {
            id = 0;
            title = "";
            content = "";
            date = "";
        }
    }

    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public Group()
        {
            id = 0;
            name = "";
        }
    }

    public class Constant
    {
        public string userName = string.Empty;
        public string pwd = string.Empty;
        private static Constant _instance = null;
        public HttpClient client = new HttpClient();
        public LoginStatus loginStatus;
        public List<Notification> notificationLst = new List<Notification>();
        public List<News> newsLst = new List<News>();
        public List<Group> groupLst = new List<Group>();
        public List<NotificationHistory> notificationHistoryLst = new List<NotificationHistory>();
        public List<Menu> menuLst = new List<Menu>();
        //public string serverDomain = "http://172.17.100.2:8000/";
        public string serverDomain = "http://45.32.193.117/";
        public Notification currentNotification = new Notification();
        public News currentNews = new News();
        public string token { get; set; }
        public int currentIndex = -1;

        public string longitude { get; set; }
        public string latitude { get; set; }
        public string platform { get; set; }
        public string model { get; set; }
        public string phoneNumber { get; set; }

        public static Constant Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Constant();
                return _instance;
            }
        }

        public Constant()
        {
            SetUerAugent();
            loginStatus = LoginStatus.LoginFailed;
            latitude = "";
            longitude = "";
        }

        public void SetUerAugent()
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

        }

        public void LoadNotifications()
        {
            notificationLst.Clear();
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/notification?userName={0}&pwd={1}", userName, pwd)).Result;
                JArray resultArray = (JArray)JsonConvert.DeserializeObject(response);
                foreach (JObject notification in resultArray)
                {
                    Notification item = new Notification();
                    item.id = int.Parse(notification["id"].ToString());
                    item.title = notification["title"].ToString();
                    item.content = notification["notification_content"].ToString();
                    item.date = notification["date"].ToString();
                    notificationLst.Add(item);
                }
            }
            catch (Exception err)
            {

            }
        }

        public void LoadNotificationStack()
        {
            notificationHistoryLst.Clear();
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/notification_history?userName={0}&pwd={1}", userName, pwd)).Result;
                JObject resultArray = (JObject)JsonConvert.DeserializeObject(response);
                string resultStatus = resultArray["result"].ToString();
                if (resultStatus.Equals("success"))
                {
                    JArray notifications = (JArray)resultArray["data"];
                    JArray recordIds = (JArray)resultArray["recordIds"];
                    for (int i = 0; i < notifications.Count; i++)
                    {
                        JObject notification = (JObject)notifications[i];
                        string recordId = recordIds[i].ToString();
                        Notification item = new Notification();
                        item.id = int.Parse(notification["id"].ToString());
                        item.title = notification["title"].ToString();
                        item.content = notification["notification_content"].ToString();
                        item.date = notification["date"].ToString();
                        NotificationHistory history = new NotificationHistory();
                        history.notification = item;
                        history.recordId = recordId;
                        notificationHistoryLst.Add(history);
                    }
                    if (notificationHistoryLst.Count > 0)
                        currentIndex = 0;
                    else
                        currentIndex = -1;
                }
            }
            catch (Exception err)
            {

            }
        }

        public bool ConfirmNotification(string recordId)
        {
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/confirm?userName={0}&pwd={1}&recordId={2}", userName, pwd, recordId)).Result;
                JObject resultArray = (JObject)JsonConvert.DeserializeObject(response);
                var resultStatus = resultArray["result"];
                if (resultStatus.ToString().Equals("success"))
                    return true;
                else
                    return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        public bool GetGeolocationAndExtrInfo()
        {
            Location location = null;
            try
            {
                location = Geolocation.GetLastKnownLocationAsync().Result;
            }
            catch (Exception err)
            {

            }
            if (location != null)
            {
                latitude = location.Latitude.ToString();
                longitude = location.Longitude.ToString();
            }
            if (userName == null || pwd == null|| userName == "" || pwd == "")
                return false;
            string response = client.GetStringAsync(serverDomain + string.Format("api/geolocation?userName={0}&pwd={1}&lon={2}&lat={3}&platform={4}&model={5}&number={6}", userName, pwd, Constant.Instance.longitude, Constant.Instance.latitude,Constant.Instance.platform,Constant.Instance.model,Constant.Instance.phoneNumber)).Result;
            JObject result = (JObject)JsonConvert.DeserializeObject(response);
            if (result["result"].ToString().Equals("success"))
                return true;
            else
                return false;
        }

        public bool SendNotificationKey ()
        {
            try
            {
                string token = "";
                try
                {
                    token = Application.Current.Properties["token"].ToString();
                }
                catch (Exception err)
                {
                    return false;
                }
                if (token == null || token == "")
                    return false;
                string response = client.GetStringAsync(serverDomain + string.Format("api/token?userName={0}&pwd={1}&token={2}", userName, pwd, token)).Result;
                JObject result = (JObject)JsonConvert.DeserializeObject(response);
                if (result["result"].ToString().Equals("success"))
                    return true;
                else
                    return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        public void LoadNews()
        {
            newsLst.Clear();
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/news?userName={0}&pwd={1}", userName, pwd)).Result;
                JArray resultArry = (JArray)JsonConvert.DeserializeObject(response);
                foreach (JObject news in resultArry)
                {
                    News item = new News();
                    item.id = int.Parse(news["id"].ToString());
                    item.title = news["title"].ToString();
                    item.content = news["news_content"].ToString();
                    item.date = news["date"].ToString();
                    newsLst.Add(item);
                }
            }
            catch (Exception err)
            {

            }
        }

        public void LoadGroup()
        {
            groupLst.Clear();
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/groups?userName={0}&pwd={1}", userName, pwd)).Result;
                JArray resultArry = (JArray)JsonConvert.DeserializeObject(response);
                foreach (JObject group in resultArry)
                {
                    Group item = new Group();
                    item.id = int.Parse(group["id"].ToString());
                    item.name = group["name"].ToString();
                    groupLst.Add(item);
                }
            }
            catch (Exception err)
            {

            }
        }

        public List<Menu> GetMenuList()
        {
            menuLst.Clear();
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/menu?userName={0}&pwd={1}", userName, pwd)).Result;
                JObject result = (JObject)JsonConvert.DeserializeObject(response);
                if (result["result"].ToString().Equals("success"))
                {
                    JArray menuList = (JArray)result["data"];
                    foreach (var menuItem in menuList)
                    {
                        Menu menu = new Menu() { menuName = menuItem["menu_text"].ToString(), menuLink = menuItem["url"].ToString() };
                        menuLst.Add(menu);
                    }
                    return menuLst;
                }
                else
                    return new List<Menu>();
            }
            catch (Exception err)
            {
                return new List<Menu>();
            }
        }

        public bool SendNotification(string title,string content,int groupId)
        {
            try
            {
                string response = client.GetStringAsync(serverDomain + string.Format("api/send_notification?userName={0}&pwd={1}&title={2}" +
                "&content={3}&groupId={4}", userName, pwd, title, content, groupId)).Result;
                JObject result = (JObject)JsonConvert.DeserializeObject(response);
                if (result["result"].ToString().Equals("success"))
                    return true;
                else
                    return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        public bool CheckLogin(string userName="",string pwd="")
        {
            if (userName.Equals(""))
            {
                try
                {
                    userName = Application.Current.Properties["UserName"].ToString();
                    pwd = Application.Current.Properties["Password"].ToString();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            try
            {
                client.Timeout = TimeSpan.FromSeconds(100);
                string response = client.GetStringAsync(serverDomain + string.Format("api/login?userName={0}&pwd={1}", userName, pwd)).Result;
                JObject result = (JObject)JsonConvert.DeserializeObject(response);
                if (result["result"].ToString().Equals("success"))
                {
                    Application.Current.Properties["UserName"] = userName;
                    Application.Current.Properties["Password"] = pwd;
                    Constant.Instance.userName = userName;
                    Constant.Instance.pwd = pwd;
                    if (result["allow"].ToString().Equals("true"))
                    {
                        loginStatus = LoginStatus.Logined;
                        SendNotificationKey();
                        GetGeolocationAndExtrInfo();
                        return true;
                    }
                    else
                    {
                        loginStatus = LoginStatus.UnAllowed;
                        return false;
                    }
                }
                else
                {
                    if (result["result"].ToString().Equals("fail"))
                    {
                        loginStatus = LoginStatus.LoginFailed;
                        return false;
                    }
                }
            }
            catch (Exception err)
            {

            }
            
            loginStatus = LoginStatus.UnKnow;
            return false;
        }
    }
}

