using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data;
using System.Data.SqlClient;
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
                UnitOfWork unitOfWork = new UnitOfWork(new DbContextFactory());
                SqlParameter parAction = new SqlParameter
                {
                    ParameterName = "@ActionName",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    Value = aName
                };
                SqlParameter parController = new SqlParameter
                {
                    ParameterName = "@ControllerName",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    Value = cName
                };
                SqlParameter parUser = new SqlParameter
                {
                    ParameterName = "@UserId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    Value = userId
                };
                allow = unitOfWork.SQLQuery<bool>("exec WebAPIFilter @UserId, @ActionName, @ControllerName", parUser, parAction, parController).FirstOrDefault();
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