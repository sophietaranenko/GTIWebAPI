using GTIWebAPI.Filters;
using GTIWebAPI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/Notifications")]
    public class NotificationsController : ApiController
    {
        private NotificationHub _notificationHub;
        private IIdentityHelper helper;
        public NotificationsController()
        {
            _notificationHub = NotificationHub.Instance;
            helper = new IdentityHelper();

        }

        [GTIFilter]
        [HttpPost]
        [Route("NewUser")]
        public IHttpActionResult AddNewNotificationUser()
        {

            string f = "{0} and {1} and {2}";
            string dd = String.Format(f, "000", "111111111111", "22");

            SendMessage("ssssssssrwwrwrss");
            //_notificationHub.OnConnected();
            //_notificationHub.Connect();
            ////_notificationHub.AddUser();
            return Ok();
        }

        

        private void SendMessage(string message)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            List<string> userIds = new List<string>();
            userIds.Add(helper.GetUserId(User));
            //// отправляем сообщение
            context.Clients.Clients(userIds).displayMessage(message);


            //context.Clients.All.displayMessage(message);
            //NotificationHub hub = NotificationHub.Instance;
            //hub.AddUser();
            //hub.SendMessage();
        }

    }
}
