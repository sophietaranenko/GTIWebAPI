﻿
using GTIWebAPI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace GTIWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
          //  Session["NotificationStore"] = new NotificationStore();
          //  NotificationManager notificationManager = new NotificationManager(Session["NotificationStore"] as NotificationStore);
          //  GlobalConfiguration.Configuration.DependencyResolver = new WebApiDependencyResolver(notificationManager);
        //}

    }
}
