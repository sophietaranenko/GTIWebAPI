using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GTIWebAPI.Filters
{
    public class GTIOfficeFilter : Attribute, IAuthorizationFilter
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

            string query = actionContext.Request.RequestUri.Query;
            List<int> queryOfficeIdRights = QueryParser.Parse("officeIds", query, ',');


            string userId = actionContext.RequestContext.Principal.Identity.GetUserId();
            try
            {
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                List<UserRight> controllerActionRights = user.UserRights.Where(r => r.Controller.Name == cName && r.Action.Name == aName).ToList();
                List<int> officeIdRights = controllerActionRights.Select(c => c.OfficeId).Distinct().ToList();

                int queryCount = queryOfficeIdRights.Count;
                int queryAllowCount = queryOfficeIdRights.Where(q => officeIdRights.Contains(q)).Count();

                if (queryCount == queryAllowCount)
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
