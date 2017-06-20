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
using Microsoft.Owin;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Context;
using System.Data.SqlClient;
using System.Data;

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
            bool allow = false;
            string cName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string aName = actionContext.ActionDescriptor.ActionName;

            string query = actionContext.Request.RequestUri.Query;
            List<int> queryOfficeIdRights = QueryParser.Parse("officeIds", query, ',');


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

                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("Value");
                foreach (var id in queryOfficeIdRights)
                {
                    DataRow row = dataTable.NewRow();
                    row["Value"] = id;
                    dataTable.Rows.Add(row);
                }
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeIds",
                    TypeName = "ut_IntList",
                    Value = dataTable,
                    SqlDbType = SqlDbType.Structured
                };
                allow = unitOfWork.SQLQuery<bool>("exec WebAPIFilterOffice @UserId, @ActionName, @ControllerName, @OfficeIds", parUser, parAction, parController, parOffice).FirstOrDefault();
            }

            //try
            //{
            //    ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
            //    List<UserRight> controllerActionRights = user.UserRights.Where(r => r.Action.Controller.Name == cName && r.Action.Name == aName).ToList();
            //    List<int> officeIdRights = controllerActionRights.Select(c => c.OfficeId).Distinct().ToList();

            //    int queryCount = queryOfficeIdRights.Count;
            //    int queryAllowCount = queryOfficeIdRights.Where(q => officeIdRights.Contains(q)).Count();

            //    if (queryCount == queryAllowCount)
            //    {
            //        allow = true;
            //    }
            //}
            catch (Exception e)
            {
                string message = e.Message;
                allow = false;
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
