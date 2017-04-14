using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using Microsoft.AspNet.Identity;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Account;

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

       // [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<UserRightMask>))]
        public IHttpActionResult GetUserRightMask()
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            IEnumerable<UserRightMask> officeUserRightMask = unitOfWork.UserRightMasksRepository.Get(includeProperties: "Office,Employee");
            return Ok(officeUserRightMask);
        }

        [HttpGet]
        [Route("GetEmpty")]
        [ResponseType(typeof(IEnumerable<RightControllerDTO>))]
        public IHttpActionResult GetEmptyMask()
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            IEnumerable<RightControllerAction> actions = unitOfWork.ActionsRepository.Get(includeProperties: "Controller");
            IEnumerable<RightControllerDTO> controllers = actions.Select(d => d.Controller).Distinct().Select(d => d.ToDTO());

            UserRightMask mask = new UserRightMask();
            mask.Rights = new List<UserRightMaskRight>();
            foreach(var item in actions)
            {
                mask.Rights.Add(new UserRightMaskRight()
                {
                    Action = item,
                    ActionId = item.Id,
                    Id = 0,
                    MaskId = 0,
                    Value = false
                });
            }
            UserRightMaskDTO dto = mask.ToDTO();
            return Ok(dto);
        }

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(UserRightMask))]
        public IHttpActionResult PostMask(UserRightMask mask)
        {
            if (mask == null || mask.Rights == null || mask.Name == null || mask.Name == "")
            {
                return BadRequest();
            }

            string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
            if (user == null || user.TableName != "Employee")
            {
                return BadRequest();
            }
            mask.CreatorId = user.TableId;
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            try
            {
                unitOfWork.UserRightMasksRepository.Insert(mask);
                unitOfWork.Save();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            if (mask.Id == 0)
            {
                return BadRequest();
            }

            foreach (var item in mask.Rights)
            {
                item.MaskId = mask.Id;
                unitOfWork.UserRightMaskRightsRepository.Insert(item);
            }
            unitOfWork.Save();
            unitOfWork.UserRightMasksRepository.GetByID(mask.Id);

            //UserRightMaskDTO 

            return Ok(mask);
        }

       
    }

   
}
