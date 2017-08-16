using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Http;
using GTIWebAPI.Controllers;
using System.Threading.Tasks;

namespace GTIWebAPI.Notifications
{

    public class NotificationHub : Hub
    {
        private static List<NotificationUser> users;

        private IIdentityHelper helper;

        private static NotificationHub instance;

        public static NotificationHub Instance
        {
            get
            {
                if (instance == null)
                    instance = new NotificationHub();
                return instance;
            }
        }

        private NotificationHub()
        {
            users = new List<NotificationUser>();
            helper = new IdentityHelper();
        }

        public void Notify(string message)
        {
            Clients.All.displayMessage(message);
        }


        public override Task OnConnected()
        {
            var dd = Context.ConnectionId;
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var dd = Context.ConnectionId;
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var dd = Context.ConnectionId;
            return base.OnReconnected();
        }

        public void SendMessage()
        {
            Clients.All.displayMessage("sdfq");
        }

        public void Connect()
        {
           //// var id = Context.ConnectionId;

           // IHubContext context = GlobalHost.ConnectionManager.GetHubContext("NotificationHub");

           // //var ii = context.Clients.Connection
           // // var iii = context.Connection;
           // var id = Context.ConnectionId;
           // if (!Users.Any(x => x.ConnectionId == id))
           // {
           //     Users.Add(new User { ConnectionId = id, Name = userName });

           //     // Посылаем сообщение текущему пользователю
           //     Clients.Caller.onConnected(id, userName, Users);

           //     // Посылаем сообщение всем пользователям, кроме текущего
           //     Clients.AllExcept(id).onNewUserConnected(id, userName);
           // }
        }


        public void AddUser()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext("NotificationHub");
            var id = Context.ConnectionId;
            if (users.Any(d => d.EmployeeId == helper.GetUserTableId(Context.User)))
            { 
                NotificationUser user = users.Where(d => d.EmployeeId == helper.GetUserTableId(Context.User)).FirstOrDefault();
                if (user.ConnectionId != id)
                {
                    users.Where(d => d.EmployeeId == helper.GetUserTableId(Context.User)).FirstOrDefault().ConnectionId = id;
                } 
            }
            if (!users.Any(x => x.ConnectionId == id))
            {
                users.Add(
                        new NotificationUser
                        {
                            ConnectionId = id,
                            EmployeeId = helper.GetUserTableId(Context.User)
                        });
            }
        }

        public void SendMessage(int?[] employeesId, Notification notification)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext("NotificationHub");
            foreach(int id in employeesId)
            {
                context.Clients.User(users.Where(d => d.EmployeeId == id).Select(d => d.ConnectionId).FirstOrDefault()).pushNewMessage(notification);
            }
        }

        public NotificationUser CurrentUser()
        {
            return users.Where(d => d.ConnectionId == Context.ConnectionId).FirstOrDefault();
        }

    }
}