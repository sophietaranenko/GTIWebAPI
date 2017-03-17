﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GTIWebAPI.Filters
{

    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            }
            else
            {
                //X509Certificate2 cert = actionContext.Request.GetClientCertificate();
                //if (cert == null)
                //{
                //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                //    {
                //        ReasonPhrase = "Client Certificate Required"
                //    };

                //}
                base.OnAuthorization(actionContext);
            }
        }
    }
    

}