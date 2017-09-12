using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Sales;
using GTIWebAPI.Notifications;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/Interactions")]
    public class InteractionsController : ApiController
    {

        private IDbContextFactory factory;
        private IIdentityHelper helper;
        
        public InteractionsController()
        {
            factory = new DbContextFactory();
            helper = new IdentityHelper();
        }

        public InteractionsController(IDbContextFactory factory, IIdentityHelper helper)
        {
            this.factory = factory;
            this.helper = helper;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetAllInteractions()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionDTO> dtos = unitOfWork.InteractionsRepository.Get(d => d.InteractionSucceedId == null && d.InteractionBrokenId == null, 
                     includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                    InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                    InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                    InteractionStatusMovements,InteractionStatusMovements.Status"
                ).Select(d => d.ToDTO());
                return Ok(dtos);
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

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetByOfficeIds")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetAllInteractionsByOfficeId(string officeIds)
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
                IEnumerable<InteractionDTO> orgs = unitOfWork.SQLQuery<InteractionDTO>("exec InteractionByOfficeIds @OfficeIds", parameter);
                return Ok(orgs);
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
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetAllInteractionsByEmployeeId(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                IEnumerable<InteractionDTO> orgs = unitOfWork.SQLQuery<InteractionDTO>("exec InteractionByEmployeeId @EmployeeId", parameter);
                return Ok(orgs);
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
        [Route("Get", Name = "GetInteraction")]
        [ResponseType(typeof(InteractionDTO))]
        public IHttpActionResult GetInteraction(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionDTO dto = unitOfWork.InteractionsRepository.Get(d => d.Id == id,
                    includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                    InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                    InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                    InteractionStatusMovements,InteractionStatusMovements.Status,InteractionBroken,InteractionBroken.InteractionBrokenReason,InteractionSucceed,InteractionSucceed.Office"
                  ).FirstOrDefault().ToDTO();
                return Ok(dto);
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

        /// <summary>
        /// Creates an Interaction
        /// With Current User as Member 
        /// And With status #1 
        /// </summary>
        /// <param name="interactionDTO"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        public IHttpActionResult PostInteraction(InteractionDTO interactionDTO)
        {
            if (interactionDTO == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Interaction interaction = interactionDTO.FromDTO();


                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionsRepository.Insert(interaction);
                unitOfWork.Save();


                Interaction toReturn = unitOfWork.InteractionsRepository.Get(d => d.Id == interaction.Id,
                        includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                    InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                    InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                    InteractionStatusMovements,InteractionStatusMovements.Status"
                      ).FirstOrDefault();

                string userId = User.Identity.GetUserId();

                EmployeeShortForm author = unitOfWork.EmployeesRepository
                    .Get(d => d.AspNetUserId == userId, includeProperties: "EmployeePassports")
                    .FirstOrDefault()
                    .ToShortForm();

                Notification notification = toReturn.ToAddingNotify(author);
                unitOfWork.NotificationsRepository.Insert(notification);
                unitOfWork.Save();
                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                foreach (var item in notification.NotificationRecipients)
                {
                    string employeeUserId = item.Employee.AspNetUserId;
                    //context.Clients.User(employeeUserId).displayMessage(notification.NotificationText, notification.LinkName, notification.LinkId, notification.NotificationDate);
                    context.Clients.All.displayMessage(notification.NotificationText, notification.LinkName, notification.LinkId, notification.NotificationDate);
                }

                InteractionDTO dto = toReturn.ToDTO();

                return CreatedAtRoute("GetInteraction", new { id = dto.Id }, dto);
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
        public IHttpActionResult PutInteraction(int id, InteractionDTO interactionDto)
        {
            if (interactionDto == null || interactionDto.Id != id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Interaction interaction = interactionDto.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionsRepository.Update(interaction);
                unitOfWork.Save();



                Interaction toReturn = unitOfWork.InteractionsRepository.Get(d => d.Id == interaction.Id,
                        includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                        InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                        InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                        InteractionStatusMovements,InteractionStatusMovements.Status"
                      ).FirstOrDefault();

                string userId = User.Identity.GetUserId();
                EmployeeShortForm author = unitOfWork.EmployeesRepository
                    .Get(d => d.AspNetUserId == userId, includeProperties: "EmployeePassports")
                    .FirstOrDefault()
                    .ToShortForm();
                Notification notification = toReturn.ToEditingNotify(author);
                unitOfWork.NotificationsRepository.Insert(notification);
                unitOfWork.Save();

                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                foreach (var item in notification.NotificationRecipients)
                {
                    string employeeUserId = item.Employee.AspNetUserId;
                   // context.Clients.All.displayMessage(notification.NotificationText, notification.LinkName, notification.LinkId, notification.NotificationDate);
                    context.Clients.User(employeeUserId).displayMessage(notification.NotificationText, notification.LinkName, notification.LinkId, notification.NotificationDate);
                }

                InteractionDTO dto = toReturn.ToDTO();

                return Ok(dto);
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
        public IHttpActionResult DeleteInteraction(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                Interaction toReturn = unitOfWork.InteractionsRepository.Get(d => d.Id == id,
                        includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                        InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                        InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                        InteractionStatusMovements,InteractionStatusMovements.Status"
                      ).FirstOrDefault();

                unitOfWork.InteractionsRepository.Delete(id);
                unitOfWork.Save();


                string userId = User.Identity.GetUserId();
                EmployeeShortForm author = unitOfWork.EmployeesRepository
                    .Get(d => d.AspNetUserId == userId, includeProperties: "EmployeePassports")
                    .FirstOrDefault()
                    .ToShortForm();
                Notification notification = toReturn.ToAddingNotify(author);
                unitOfWork.NotificationsRepository.Insert(notification);
                unitOfWork.Save();


                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                foreach (var item in notification.NotificationRecipients)
                {
                    string employeeUserId = item.Employee.AspNetUserId;
                    context.Clients.User(employeeUserId).displayMessage(notification.NotificationText, notification.LinkName, notification.LinkId, notification.NotificationDate);
                }

                InteractionDTO dto = toReturn.ToDTO();

                return Ok(dto);
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
        [Route("MarkAsBroken")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult MarkInteractionAsBroken(int interactionId, InteractionBrokenDTO brokenDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionBroken broken = brokenDTO.FromDTO();
                unitOfWork.InteractionsBrokenRepository.Insert(broken);
                unitOfWork.Save();
                Interaction interaction = unitOfWork.InteractionsRepository.Get(d => d.Id == interactionId).FirstOrDefault();
                interaction.InteractionBrokenId = broken.Id;
                unitOfWork.InteractionsRepository.Update(interaction);
                unitOfWork.Save();

                Interaction toReturn = unitOfWork.InteractionsRepository.Get(d => d.Id == interaction.Id,
                        includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                        InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                        InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                        InteractionStatusMovements,InteractionStatusMovements.Status,InteractionBroken,InteractionBroken.InteractionBrokenReason"
                      ).FirstOrDefault();
                return Ok(toReturn.ToDTO());
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
        [Route("MarkAsSucceed")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult MarkInteractionAsBroken(int interactionId, InteractionSucceedDTO succeedDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionSucceed succeed = succeedDTO.FromDTO();
                unitOfWork.InteractionsSucceedRepository.Insert(succeed);
                unitOfWork.Save();

                Interaction interaction = unitOfWork.InteractionsRepository.Get(d => d.Id == interactionId).FirstOrDefault();
                interaction.InteractionSucceedId = succeed.Id;
                unitOfWork.InteractionsRepository.Update(interaction);
                unitOfWork.Save();

                Interaction toReturn = unitOfWork.InteractionsRepository.Get(d => d.Id == interaction.Id,
                        includeProperties: @"InteractionMembers,InteractionMembers.Employee,InteractionMembers.Employee.EmployeePassports,
                        InteractionActs,InteractionActs.Act,InteractionActs.InteractionActMembers,InteractionActs.InteractionActMembers.Employee,InteractionActs.InteractionActMembers.Employee.EmployeePassports,
                        InteractionActs.InteractionActOrganizationMembers,InteractionActs.InteractionActOrganizationMembers.OrganizationContactPerson,
                        InteractionStatusMovements,InteractionStatusMovements.Status,InteractionSucceed,InteractionSucceed.Office"
                      ).FirstOrDefault();
                return Ok(toReturn.ToDTO());
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

        [HttpGet]
        [Route("GetBrokenReason")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetInteractionBrokenReasons()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionBrokenReasonDTO> dtos = unitOfWork.InteractionBrokenReasonsRepository.Get().Select(d => d.ToDTO());
                return Ok(dtos);
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

        [HttpGet]
        [Route("GetBrokenReason")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetInteractionStatuses()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionStatusDTO> dtos = unitOfWork.InteractionStatusesRepository.Get().Select(d => d.ToDTO());
                return Ok(dtos);
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
