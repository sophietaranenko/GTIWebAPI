using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using GTIWebAPI.Filters;
using System.Web.Http.Cors;

namespace GTIWebAPI
{
    /// <summary>
    /// Class with WebAPI configuration 
    /// </summary>
    public static class WebApiConfig
    {
        //public static string UrlPrefix { get { return "api"; } }
        //public static string UrlPrefixRelative { get { return "~/api"; } }

        /// <summary>
        /// Register roures method
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Filters.Add(new RequireHttpsAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();


            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            //config.Routes.MapHttpRoute(
            //    name: "ActionApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { action="GetAll", id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }

            );
        }
    }

    
}
