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


using GTIWebAPI.Models.Account;
using GTIWebAPI.Exceptions;
using GTIWebAPI.Models.Repository;
using System.Web.Http.Controllers;
using System.Security.Principal;
using GTIWebAPI.NovelleDirectory;

namespace GTIWebAPI.Controllers
{

    public interface IIdentityHelper
    {
        string GetUserId(HttpActionContext context);

        ApplicationUser FindUserById(string userId);

        string GetUserTableName(HttpActionContext context);

        string GetUserId(IPrincipal p);

        string GetUserTableName(IPrincipal p);

        int GetUserTableId(IPrincipal p);

        string GetUserEmail(IPrincipal p);
    }

    public class IdentityHelper : IIdentityHelper
    {
        public ApplicationUser FindUserById(string userId)
        {
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
            return user;
        }

        public string GetUserEmail(IPrincipal p)
        {
            return FindUserById(GetUserId(p)).Email;
        }

        public string GetUserId(HttpActionContext context)
        {
            return context.RequestContext.Principal.Identity.GetUserId();
        }

        public string GetUserId(IPrincipal p)
        {
            return p.Identity.GetUserId();
        }

        public int GetUserTableId(IPrincipal p)
        {
            return 0;
        //    return FindUserById(GetUserId(p)).TableId;
        }

        public string GetUserTableName(HttpActionContext context)
        {
            return "";
          //  return FindUserById(GetUserId(context)).TableName;
        }

        public string GetUserTableName(IPrincipal p)
        {
            return "";
           // return FindUserById(GetUserId(p)).TableName;
        }

    }





    [RoutePrefix("api/OrganizationGTILinks")]
    public class OrganizationGTILinksController : ApiController
    {
        //Добавить проверки 

        IDbContextFactory factory;
        IIdentityHelper identityHelper;

        public OrganizationGTILinksController()
        {
            factory = new DbContextFactory();
            identityHelper = new IdentityHelper();
        }

        public OrganizationGTILinksController(IDbContextFactory factory)
        {
            this.factory = factory;
            identityHelper = new IdentityHelper();
        }

        public OrganizationGTILinksController(IDbContextFactory factory, IIdentityHelper identityHelper)
        {
            this.factory = factory;
            this.identityHelper = identityHelper;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationGTILinkDTO>))]
        public IHttpActionResult GetOrganizationGTILinkByOrganizationId(int organizationId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<OrganizationGTILinkDTO> links = unitOfWork.OrganizationGTILinksRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == organizationId)
                    .Select(d => d.ToDTO()).ToList();

                foreach (var link in links)
                {
                    if (link.GTIId != null)
                    {
                        link.OrganizationGTI =
                            unitOfWork.OrganizationGTIsRepository.Get(d => d.Id == link.GTIId)
                            .FirstOrDefault().ToDTO();
                        if (link.OrganizationGTI != null)
                        {
                            link.OrganizationGTI.Office =
                                unitOfWork.OfficesRepository.Get(d => d.Id == link.OrganizationGTI.OfficeId)
                                .FirstOrDefault().ToDTO();
                        }
                    }
                }
                IEnumerable<OrganizationGTIDTO> dtos = links.Select(d => d.OrganizationGTI).Distinct(); 
                //do something with links
                return Ok(links);
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
        [Route("Get", Name = "GetOrganizationGTILink")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult GetOrganizationGTILink(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationGTILinkDTO link = unitOfWork.OrganizationGTILinksRepository
                    .Get(d => d.Id == id).FirstOrDefault().ToDTO();

                if (link.GTIId != null)
                {
                    link.OrganizationGTI = unitOfWork.OrganizationGTIsRepository
                    .Get(d => d.Id == link.GTIId).FirstOrDefault().ToDTO();
                    if (link.OrganizationGTI != null)
                    {
                        link.OrganizationGTI.Office = unitOfWork.OfficesRepository
                        .Get(d => d.Id == link.OrganizationGTI.OfficeId).FirstOrDefault().ToDTO();
                    }
                }
                return Ok(link);
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
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult PostOrganizationGTILink(OrganizationGTILinkDTO organizationGTILink)
        {
            try
            {
                

                if (organizationGTILink == null)
                {
                    return BadRequest(ModelState);
                }

                if (identityHelper.GetUserTableName(User) == "Employee")
                {
                    OrganizationGTILink link = organizationGTILink.FromDTO();

                    int EmployeeId = identityHelper.GetUserTableId(User);
                    link.EmployeeId = EmployeeId;



                    UnitOfWork unitOfWork = new UnitOfWork(factory);
                    OrganizationGTILink existingLink = unitOfWork.OrganizationGTILinksRepository
                        .Get(d => d.Deleted != true && d.GTIId == link.GTIId)
                        .FirstOrDefault();
                    if (existingLink != null)
                    {
                        return BadRequest("Link to this GTI Organization already exist");
                    }




                    link.Id = link.NewId(unitOfWork);
                    unitOfWork.OrganizationGTILinksRepository.Insert(link);
                    unitOfWork.Save();



                    OrganizationGTILinkDTO dto = unitOfWork.OrganizationGTILinksRepository
                    .Get(d => d.Id ==link.Id).FirstOrDefault().ToDTO();

                    if (dto.GTIId != null)
                    {
                        dto.OrganizationGTI = unitOfWork.OrganizationGTIsRepository
                        .Get(d => d.Id == dto.GTIId).FirstOrDefault().ToDTO();
                        if (dto.OrganizationGTI != null)
                        {
                            dto.OrganizationGTI.Office = unitOfWork.OfficesRepository
                            .Get(d => d.Id == dto.OrganizationGTI.OfficeId).FirstOrDefault().ToDTO();
                        }
                    }
                    return CreatedAtRoute("GetOrganizationGTILink", new { id = dto.Id }, dto);
                }
                else
                {
                    return BadRequest();
                }
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
        [Route("SeveralLinksPost")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult PostOrganizationGTICreateLink(OrganizationGTICreateLinkDTO links)
        {
            if (links == null)
            {
                return BadRequest(ModelState);
            }
            if (links.OrganizationGTIIds == null)
            {
                return BadRequest();
            }
            // string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
            //  ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
          //  ApplicationUser user = identityHelper.FindUserById(User.Identity.GetUserId());
            if (identityHelper.GetUserTableName(User) == "Employee")
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                foreach(var item in links.OrganizationGTIIds)
                { 
                    OrganizationGTILink existingLink = unitOfWork.OrganizationGTILinksRepository
                        .Get(d => d.Deleted != true && d.GTIId == item)
                        .FirstOrDefault();
                    if (existingLink != null)
                     {
                        return BadRequest(String.Format("Link to {0} already exist", unitOfWork.OrganizationGTIsRepository.GetByID(item).NativeName));
                     }
                }
                List<OrganizationGTILinkDTO> createdLinks = new List<OrganizationGTILinkDTO>();
                int EmployeeId = identityHelper.GetUserTableId(User);
                int OrganzationId = links.OrganizationId;
                try
                {
                    foreach (var item in links.OrganizationGTIIds)
                    {
                        //Добавление
                        int OrganizationGTIId = item;
                        OrganizationGTILink link = new OrganizationGTILink();
                        link.Id = link.NewId(unitOfWork);
                        link.OrganizationId = OrganzationId;
                        link.GTIId = OrganizationGTIId;
                        link.EmployeeId = EmployeeId;
                        unitOfWork.OrganizationGTILinksRepository.Insert(link);
                        unitOfWork.Save();
                        //Достать со всеми вложенными
                        OrganizationGTILinkDTO newLink = unitOfWork.OrganizationGTILinksRepository
                        .Get(d => d.Id == link.Id).FirstOrDefault().ToDTO();
                        if (link.GTIId != null)
                        {
                            newLink.OrganizationGTI = unitOfWork.OrganizationGTIsRepository
                            .Get(d => d.Id == newLink.GTIId).FirstOrDefault().ToDTO();

                            if (newLink.OrganizationGTI != null)
                            {
                                newLink.OrganizationGTI.Office = unitOfWork.OfficesRepository
                                .Get(d => d.Id == newLink.OrganizationGTI.OfficeId).FirstOrDefault().ToDTO();
                            }
                        }
                        createdLinks.Add(newLink);
                    }
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

                return Ok(createdLinks);
            }
            return BadRequest();
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationGTILink))]
        public IHttpActionResult DeleteOrganizationGTILink(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationGTILink link = unitOfWork.OrganizationGTILinksRepository
                    .Get(d => d.Id == id).FirstOrDefault();
                link.Deleted = true;
                unitOfWork.OrganizationGTILinksRepository.Update(link);
                unitOfWork.Save();
                return Ok(link.ToDTO());
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }



    }

}
