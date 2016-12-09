using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GTIWebAPI.Filters
{
    public class GTIFilter : Attribute, IAuthorizationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            bool allow = false;
            string cName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string aName = actionContext.ActionDescriptor.ActionName;
            string userId = actionContext.RequestContext.Principal.Identity.GetUserId();
            try
            {
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                UserRight right = user.UserRights.Where(r => r.Controller.Name == cName && r.Action.Name == aName).FirstOrDefault();
                if (right != null)
                {
                    allow = true;
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
            }
            if (allow == false)
            {
                return Task.FromResult<HttpResponseMessage>(
                       actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }
            else
            {
                return continuation();
            }
        }
    }
}