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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for user rights
    /// </summary>
    [RoutePrefix("api/UserRights")]
    public class UserRightsController : ApiController
    {
        private ApplicationUserManager _userManager;
        ApplicationDbContext db = new ApplicationDbContext();
        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetUserRightsView")]
        [ResponseType(typeof(List<UserRightDTO>))]
        public IHttpActionResult GetUserRightView(string UserId)
        {
            ApplicationUser u = UserManager.FindById(UserId);
            if (u == null)
            {
                return NotFound();
            }
            List<UserRightDTO> rights = u.UserRightsDto;
            if (rights == null)
            {
                return NotFound();
            }
            return Ok(rights);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetUserRightsEdit")]
        [ResponseType(typeof(List<UserRightDTO>))]
        public IHttpActionResult GetUserRightEdit(string UserId)
        {
            ApplicationUser u = UserManager.FindById(UserId);
            if (u == null)
            {
                return NotFound();
            }
            List<UserRightDTO> rights = u.UserRightsDto;
            if (rights == null)
            {
                return NotFound();
            }
            return Ok(rights);
        }


        [GTIFilter]
        [HttpPut]
        [Route("PutUserRights")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserRights(string UserId, List<UserRightEditDTO> rights)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (UserId == null)
            {
                return BadRequest();
            }

            List<UserRight> oldRights = UserManager.FindById(UserId).UserRights.ToList();
            db.UserRights.RemoveRange(oldRights);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            List<UserRight> newRights = new List<UserRight>();
            foreach (var item in rights)
            {
                UserRight right = new UserRight();

                int Id = right.NewId(db);
                right.Id = Id;
                right.OfficeId = item.OfficeId;
                right.ControllerId = item.ControllerId;
                right.ActionId = item.ActionId;
                right.AspNetUserId = UserId;

                newRights.Add(right);
            }

            db.UserRights.AddRange(newRights);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [GTIFilter]
        [HttpPost]
        [Route("PostUserRights")]
        [ResponseType(typeof(List<UserRightDTO>))]
        public IHttpActionResult PostUserRights(string UserId, List<UserRightEditDTO> rights)
        {
            if (rights == null)
            {
                return BadRequest(ModelState);
            }

            List<UserRight> newRights = new List<UserRight>();
            foreach (var item in rights)
            {
                UserRight right = new UserRight();

                int Id = right.NewId(db);
                right.Id = Id;
                right.OfficeId = item.OfficeId;
                right.ControllerId = item.ControllerId;
                right.ActionId = item.ActionId;
                right.AspNetUserId = UserId;

                newRights.Add(right);
            }
            db.UserRights.AddRange(newRights);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return StatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// Delete all rights by user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteUserRights")]
        [ResponseType(typeof(List<UserRightDTO>))]
        public IHttpActionResult DeleteUserRights(string UserId)
        {
            ApplicationUser u = UserManager.FindById(UserId);
            if (u == null)
            {
                return NotFound();
            }
            List<UserRightDTO> rightsDTO = u.UserRightsDto;
            if (rightsDTO == null)
            {
                return NotFound();
            }
            List<UserRight> rights = u.UserRights.ToList();
            db.UserRights.RemoveRange(rights);
            db.SaveChanges();
            return Ok(rightsDTO);
        }

        /// <summary>
        /// Get all controllers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetControllers")]
        public IEnumerable<ControllerDTO> GetControllers()
        {
            List<Controller> cList = db.Controllers.ToList();
            AutoMapper.Mapper.Initialize(m => m.CreateMap<Controller, ControllerDTO>());
            IEnumerable<ControllerDTO> dtoList = AutoMapper.Mapper.Map<IEnumerable<Controller>, IEnumerable<ControllerDTO>>(cList);
            return dtoList;
        }

        /// <summary>
        /// Get all actions of controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActions")]
        public IEnumerable<ActionDTO> GetActions(int controllerId)
        {
            List<Models.Security.Action> aList = db.Actions.Where(a => a.ControllerId == controllerId).ToList();
            AutoMapper.Mapper.Initialize(m => m.CreateMap<Models.Security.Action, ActionDTO>());
            IEnumerable<ActionDTO> dtoList = AutoMapper.Mapper.Map<IEnumerable<Models.Security.Action>, IEnumerable<ActionDTO>>(aList);
            return dtoList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
