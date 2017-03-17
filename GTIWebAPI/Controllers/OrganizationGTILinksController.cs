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
using GTIWebAPI.Models.Repository.Organization;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/OrganizationGTILinks")]
    public class OrganizationGTILinksController : ApiController
    {
        IOrganizationRepository<OrganizationGTILink> repo;

        public OrganizationGTILinksController()
        {
            repo = new OrganizationGTILinksRepository();
        }

        public OrganizationGTILinksController(IOrganizationRepository<OrganizationGTILink> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationGTILinkDTO>))]
        public IHttpActionResult GetOrganizationGTILinkByOrganizationId(int organizationId)
        {
            try
            {
                List<OrganizationGTILinkDTO> dtos = repo.GetByOrganizationId(organizationId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                OrganizationGTILinkDTO dto = repo.Get(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult PostOrganizationGTILink(OrganizationGTILink organizationGTILink)
        {
            try
            {
                string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                if (user != null && user.TableName == "Employee")
                {
                    int EmployeeId = user.TableId;

                    if (organizationGTILink == null)
                    {
                        return BadRequest(ModelState);
                    }
                    organizationGTILink.EmployeeId = EmployeeId;
                    try
                    {
                        OrganizationGTILinkDTO dto = repo.Add(organizationGTILink).ToDTO();
                        return CreatedAtRoute("GetOrganizationGTILink", new { id = dto.Id }, dto);
                    }
                    catch (Exception e)
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return BadRequest();
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
            string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
            if (user != null && user.TableName == "Employee")
            {
                List<OrganizationGTILinkDTO> createdLinks = new List<OrganizationGTILinkDTO>();

                int EmployeeId = user.TableId;
                int OrganzationId = links.OrganizationId;
                try
                {
                    using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                    {
                        foreach (var item in links.OrganizationGTIIds)
                        {
                            int OrganizationGTIId = item;
                            OrganizationGTILink link = new OrganizationGTILink();
                            link.OrganizationId = OrganzationId;
                            link.GTIId = OrganizationGTIId;
                            link.EmployeeId = EmployeeId;
                            OrganizationGTILinkDTO dto = repo.Add(link).ToDTO();
                            createdLinks.Add(dto);
                        }
                    }
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
                OrganizationGTILinkDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }



    }

}
