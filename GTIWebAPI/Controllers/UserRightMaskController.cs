using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Account;
using System.Data.SqlClient;
using System.Data;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/UserRightMasks")]
    public class UserRightMaskController : ApiController
    {
        private IDbContextFactory factory;

        public UserRightMaskController()
        {
            this.factory = new DbContextFactory();
        }

        public UserRightMaskController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public UserRightMaskTreeView ToTreeView(IEnumerable<UserRightMaskDTO> officeUserRightMask)
        {
            UserRightMaskTreeView mask = officeUserRightMask.Where(d => d.CreatorId != null && d.Id != null && d.Name != null && d.OfficeId != null).Select(d => new UserRightMaskTreeView
            {
                CreatorId = d.CreatorId,
                Id = d.Id,
                Name = d.Name,
                OfficeId = d.OfficeId
            }).FirstOrDefault();

            if (mask == null)
            {
                mask = new UserRightMaskTreeView();
            }

            string office = officeUserRightMask.Where(d => d.OfficeShortName != null).Select(d => d.OfficeShortName).FirstOrDefault();
            mask.Office = new Models.Dictionary.OfficeDTO()
            {
                Id = mask.OfficeId.GetValueOrDefault(),
                ShortName = office
            };
            if (mask == null)
            {
                mask = new UserRightMaskTreeView();
            }
            //when IEnumerable<> - can't change elements of collection inside foreach statement 
            //that's why we do List<>, which allows to change elements in collection 
            List<RightControllerBoxDTO> boxes = officeUserRightMask.Select(d => new RightControllerBoxDTO { Id = d.BoxId, Name = d.BoxName, LongName = d.BoxLongName }).Distinct().ToList();
            foreach (var box in boxes)
            {
                List<RightControllerDTO> controllers = officeUserRightMask.Where(d => d.BoxId == box.Id)
                    .Where(d => d.BoxId != null)
                    .Select(d => new RightControllerDTO
                    {
                        Id = d.ControllerId.GetValueOrDefault(),
                        Name = d.ControllerName,
                        LongName = d.ControllerLongName
                    }).Distinct().ToList();
                foreach (var controller in controllers)
                {
                    List<RightControllerActionDTO> actions = officeUserRightMask.Where(d => d.ControllerId == controller.Id)
                        .Select(d => new RightControllerActionDTO
                        {
                            Id = d.ActionId.GetValueOrDefault(),
                            Name = d.ActionName,
                            LongName = d.ActionLongName,
                            Value = d.Value
                        }).Distinct().ToList();
                    controller.Actions = actions;
                }
                box.Controllers = controllers;
            }
            mask.Boxes = boxes;
            return mask;
        }

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetByOfficeIds")]
        [ResponseType(typeof(IEnumerable<UserRightMaskTreeView>))]
        public IHttpActionResult GetUserRightMaskByOfficeIds(string officeIds)
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("Value");
                foreach (var id in OfficeIds)
                {
                    DataRow row = dataTable.NewRow();
                    row["Value"] = id;
                    dataTable.Rows.Add(row);
                }
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@OfficeIds",
                    TypeName = "ut_IntList",
                    Value = dataTable,
                    SqlDbType = SqlDbType.Structured
                };
                List<UserRightMaskTreeView> masks = new List<UserRightMaskTreeView>();
                IEnumerable<Guid> maskIds = unitOfWork.SQLQuery<Guid>("exec GetAspNetUserRightMaskByOfficeIds @OfficeIds", parameter);
                foreach (Guid maskId in maskIds)
                {
                    SqlParameter parMaskId = new SqlParameter
                    {
                        DbType = System.Data.DbType.Guid,
                        ParameterName = "@MaskId",
                        Value = maskId
                    };
                    IEnumerable<UserRightMaskDTO> officeUserRightMask =
                    unitOfWork.SQLQuery<UserRightMaskDTO>("exec GetAspNetUserRightMask @MaskId", parMaskId);
                    UserRightMaskTreeView mask = ToTreeView(officeUserRightMask);
                    masks.Add(mask);
                }
                return Ok(masks);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        [ResponseType(typeof(UserRightMaskTreeView))]
        public IHttpActionResult GetUserRightMask(Guid id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter maskId = new SqlParameter
                {
                    DbType = System.Data.DbType.Guid,
                    ParameterName = "@MaskId",
                    Value = id
                };
                List<UserRightMaskDTO> officeUserRightMask =
                    unitOfWork.SQLQuery<UserRightMaskDTO>("exec GetAspNetUserRightMask @MaskId", maskId).ToList();
                UserRightMaskTreeView mask = ToTreeView(officeUserRightMask);
                return Ok(mask);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetEmpty")]
        [ResponseType(typeof(UserRightMaskTreeView))]
        public IHttpActionResult GetEmptyMask()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<UserRightMaskDTO> officeUserRightMask =
                    unitOfWork.SQLQuery<UserRightMaskDTO>("exec GetAspNetUserRightMaskEmpty");
                UserRightMaskTreeView mask = ToTreeView(officeUserRightMask);
                return Ok(mask);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(UserRightMask))]
        public IHttpActionResult PutMask(Guid id, UserRightMaskTreeView mask)
        {
            if (mask == null || mask.Boxes == null || mask.Name == null || mask.Name == "")
            {
                return BadRequest();
            }
            if (mask.Id != id)
            {
                return BadRequest();
            }
            SqlParameter maskId = new SqlParameter()
            {
                DbType = System.Data.DbType.Guid,
                ParameterName = "@MaskId",
                Value = mask.Id
            };
            SqlParameter office = new SqlParameter()
            {
                DbType = System.Data.DbType.Int32,
                ParameterName = "@OfficeId",
                Value = mask.OfficeId
            };
            SqlParameter name = new SqlParameter()
            {
                DbType = System.Data.DbType.String,
                ParameterName = "@Name",
                Value = mask.Name
            };
            IEnumerable<int> actionIds = mask.Boxes.SelectMany(d => d.Controllers.SelectMany(c => c.Actions.Select(a => a.Id)));
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

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<UserRightMaskDTO> officeUserRightMask =
                    unitOfWork.SQLQuery<UserRightMaskDTO>("exec UpdateAspNetUserRightMask @MaskId, @Name, @OfficeId, @Actions", maskId, name, office, actions);
                UserRightMaskTreeView createdMask = ToTreeView(officeUserRightMask);
                return Ok(createdMask);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(UserRightMask))]
        public IHttpActionResult PostMask(UserRightMaskTreeView mask)
        {
            if (mask == null || mask.Boxes == null || mask.Name == null || mask.Name == "")
            {
                return BadRequest();
            }
            string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
            if (user == null || user.TableName != "Employee")
            {
                return BadRequest("User is not employee");
            }
            SqlParameter creator = new SqlParameter()
            {
                DbType = System.Data.DbType.Int32,
                ParameterName = "@CreatorId",
                Value = user.TableId
            };
            SqlParameter office = new SqlParameter()
            {
                DbType = System.Data.DbType.Int32,
                ParameterName = "@OfficeId",
                Value = mask.OfficeId
            };
            SqlParameter name = new SqlParameter()
            {
                DbType = System.Data.DbType.String,
                ParameterName = "@Name",
                Value = mask.Name
            };
            IEnumerable<int> actionIds = mask.Boxes.SelectMany(d => d.Controllers.SelectMany(c => c.Actions.Select(a => a.Id)));
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("ActionId");

            foreach (var id in actionIds)
            {
                DataRow row = dataTable.NewRow();
                row["ActionId"] = id;
                dataTable.Rows.Add(row);
            }
            SqlParameter actions = new SqlParameter
            {
                ParameterName = "@Actions",
                TypeName = "ut_MaskRight",
                Value = dataTable,
                SqlDbType = SqlDbType.Structured
            };
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<UserRightMaskDTO> officeUserRightMask =
                    unitOfWork.SQLQuery<UserRightMaskDTO>("exec CreateAspNetUserRightMask @Name, @OfficeId, @CreatorId, @Actions", name, office, creator, actions);
                UserRightMaskTreeView createdMask = ToTreeView(officeUserRightMask);
                return Ok(createdMask);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteMask(Guid id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                var mask = unitOfWork.UserRightMasksRepository.GetByID(id);
                mask.Deleted = true;
                unitOfWork.UserRightMasksRepository.Update(mask);
                unitOfWork.Save();
                return Ok(new { mask.Id, mask.Name, mask.OfficeId });
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
