﻿using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GTIWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        //private void SendMessage(string message)
        //{
        //    // Получаем контекст хаба
        //    var context =
        //        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        //    // отправляем сообщение
        //    context.Clients.All.displayMessage(message);
        //}
    }
}
