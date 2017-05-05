using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Security;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using System.Data.SqlClient;
using System.Data;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for user rights
    /// </summary>
    [RoutePrefix("api/UserRights")]
    public class UserRightsController : ApiController
    {
        private IDbContextFactory factory;

        public UserRightsController()
        {
            factory = new DbContextFactory();
        }

        public UserRightsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public UserRightTreeView ToTreeView(List<UserRightOfficeDTO> rights)
        {
            UserRightTreeView userRightTreeView = new UserRightTreeView();
            //он все равно один
            userRightTreeView.UserId = rights.Where(d => d.UserId != null).Select(d => d.UserId).FirstOrDefault();
            //равно как и офис
            userRightTreeView.OfficeId = rights.Where(d => d.OfficeId != null).Select(d => d.OfficeId).FirstOrDefault();
            string office = rights.Where(d => d.OfficeShortName != null).Select(d => d.OfficeShortName).FirstOrDefault();
            userRightTreeView.Office = new OfficeDTO
            {
                Id = userRightTreeView.OfficeId.GetValueOrDefault(),
                ShortName = office
            };

            List<RightControllerBoxDTO> boxes = rights.Select(d => new RightControllerBoxDTO
            {
                Id = d.BoxId,
                Name = d.BoxName,
                LongName = d.BoxLongName
            }).Distinct().ToList();

            foreach (var box in boxes)
            {
                List<RightControllerDTO> controllers = rights.Where(d => d.BoxId == box.Id)
                    .Where(d => d.BoxId != null)
                    .Select(d => new RightControllerDTO
                    {
                        Id = d.ControllerId.GetValueOrDefault(),
                        Name = d.ControllerName,
                        LongName = d.ControllerLongName
                    }).Distinct().ToList();
                foreach (var controller in controllers)
                {
                    List<RightControllerActionDTO> actions = rights.Where(d => d.ControllerId == controller.Id)
                        .Select(d => new RightControllerActionDTO
                        {
                            Id = d.ActionId.GetValueOrDefault(),
                            Name = d.ActionName,
                            LongName = d.ActionLongName,
                            Value = d.Value.GetValueOrDefault()
                        }).Distinct().ToList();
                    controller.Actions = actions;
                }
                box.Controllers = controllers;
            }
            
            userRightTreeView.Boxes = boxes;
            return userRightTreeView;
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        [ResponseType(typeof(List<UserRightTreeView>))]
        public IHttpActionResult GetUserRightsByUser(string userId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            SqlParameter pUserId = new SqlParameter()
            {
                DbType = System.Data.DbType.String,
                Value = userId,
                ParameterName = "@UserId"
            };
            IEnumerable<int> officeIds = unitOfWork.SQLQuery<int>("exec GetAspNetUserRightsByOfficeId @UserId", pUserId);
            List<UserRightTreeView> trees = new List<UserRightTreeView>();
            foreach (int officeId in officeIds)
            {
                SqlParameter pUserIdIn = new SqlParameter()
                {
                    DbType = System.Data.DbType.String,
                    Value = userId,
                    ParameterName = "@UserId"
                };
                SqlParameter pOfficeIdIn = new SqlParameter()
                {
                    DbType = System.Data.DbType.Int32,
                    Value = officeId,
                    ParameterName = "@OfficeId"
                };
                List<UserRightOfficeDTO> rights = 
                    unitOfWork.SQLQuery<UserRightOfficeDTO>("exec GetAspNetUSerRights @UserId, @OfficeId", pUserIdIn, pOfficeIdIn).ToList();
                trees.Add(ToTreeView(rights));
            }
            return Ok(trees);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOffice")]
        [ResponseType(typeof(List<UserRightDTO>))]
        public IHttpActionResult GetUserRightsByUserAndOffice(string userId, int officeId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);

            SqlParameter pUserIdIn = new SqlParameter()
            {
                DbType = System.Data.DbType.String,
                Value = userId,
                ParameterName = "@UserId"
            };
            SqlParameter pOfficeIdIn = new SqlParameter()
            {
                DbType = System.Data.DbType.Int32,
                Value = officeId,
                ParameterName = "@OfficeId"
            };
            List<UserRightOfficeDTO> rights =
                unitOfWork.SQLQuery<UserRightOfficeDTO>("exec GetAspNetUSerRights @UserId, @OfficeId", pUserIdIn, pOfficeIdIn).ToList();

            return Ok(ToTreeView(rights));
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(UserRightTreeView))]
        public IHttpActionResult PutUserRights(string userId, int officeId, UserRightTreeView tree)
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);

            SqlParameter pUserIdIn = new SqlParameter()
            {
                DbType = System.Data.DbType.String,
                Value = userId,
                ParameterName = "@UserId"
            };
            SqlParameter pOfficeIdIn = new SqlParameter()
            {
                DbType = System.Data.DbType.Int32,
                Value = officeId,
                ParameterName = "@OfficeId"
            };

            IEnumerable<int> actionIds = new List<int>();
            if (tree.Boxes != null)
            { 
                actionIds = tree.Boxes.SelectMany(d => d.Controllers.SelectMany(c => c.Actions.Select(a => a.Id)));
            }
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("ActionId");

            foreach (var actionId in actionIds)
            {
                DataRow row = dataTable.NewRow();
                row["ActionId"] = actionId;
                dataTable.Rows.Add(row);
            }
            SqlParameter actions = new SqlParameter
            {
                ParameterName = "@Actions",
                TypeName = "ut_MaskRight",
                Value = dataTable,
                SqlDbType = SqlDbType.Structured
            };

            List<UserRightOfficeDTO> rights =
                unitOfWork.SQLQuery<UserRightOfficeDTO>("exec UpdateAspNetUserRights @UserId, @OfficeId, @Actions", pUserIdIn, pOfficeIdIn, actions).ToList();
            return Ok(ToTreeView(rights));
        }

    }
}
