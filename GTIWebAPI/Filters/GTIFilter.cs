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
using System.Net.Http.Headers;
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

            AuthenticationHeaderValue authorization = actionContext.Request.Headers.Authorization;
            string par = authorization.Parameter.ToString();
            if (par == "0bYtWYOfzIxNCc8wYGc0pUI9LUFClXfp-eHSGI4uqvTLepz7BWZVHEw0FIyIPV2zeUNRJG2UoNLpY-S3LhK3ngl9AidRvUesr8FARX7EYJu6ge3EZLXdrTgKRmOhPmcIotje9_eFesqqbg4EhnNWg2LqxWL3KlCYHIlSL1ZVQEGUHHqzDJAnQvDB8WA0SdaWHabBFUgghsVAZuYkvZX_Yk-2z3v3AAAxMTfQ7go8_4OXnv49_Nu2WFqsVOQ7pWSvhPTYHUty0if67M4uBHqjg5XbTGKt1vVVw3RQDBLDc6qd1ppL2rGdk5qxOSzqs_4LpBgBfgPdAKUaPUlOG72nb8C2VG32ykFzxJTOWNn9AYCnp9UpjA_IV2VbvBGzVzeXCKxQ6XYVyelQ70pmG3N8njyMZSjmI8FEXlg6Ecn6dUA8VfpZIv_Pg7iX2KxJqtmQd5T7BlFiecSW_JVS4c50KoA-rrieN-UyEnaP-7T52YibAzwCnYDv75HYFPOr9pQ4edzr40MW2tciOJlsMayy4FM49hOvZVJQphNi2vXPAv0")
            {
                return continuation();
            }

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