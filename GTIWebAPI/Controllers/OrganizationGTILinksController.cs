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

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/OrganizationGTILinks")]
    public class OrganizationGTILinksController : ApiController
    {

        /// <summary>
        /// Get employee links by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationGTILinkDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationGTILinkDTO>))]
        public IHttpActionResult GetOrganizationGTILinkByOrganizationId(int organizationId)
        {
            List<OrganizationGTILink> links = new List<OrganizationGTILink>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    links = db.OrganizationGTILinks
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                    .ToList();

                    foreach (var link in links)
                    {
                        if (link.GTIId != null)
                        {
                            link.OrganizationGTI = db.GTIOrganizations.Find(link.GTIId);
                            if (link.OrganizationGTI != null)
                            {
                                link.OrganizationGTI.Office = db.Offices.Find(link.OrganizationGTI.OfficeId);
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                return BadRequest();
            }


            List<OrganizationGTILinkDTO> dtos = links.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Get one link by link id
        /// </summary>
        /// <param name="id">OrganizationGTILink id</param>
        /// <returns>OrganizationGTILinkEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationGTILink")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult GetOrganizationGTILink(int id)
        {
            OrganizationGTILink link = new OrganizationGTILink();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    link = db.OrganizationGTILinks.Find(id);

                    if (link == null)
                    {
                        return NotFound();
                    }

                    if (link.GTIId != null)
                    {
                        link.OrganizationGTI = db.GTIOrganizations.Find(link.GTIId);
                        if (link.OrganizationGTI != null)
                        {
                            link.OrganizationGTI.Office = db.Offices.Find(link.OrganizationGTI.OfficeId);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            OrganizationGTILinkDTO dto = link.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new organizationGTI link (with EmployeeId as creator) 
        /// </summary>
        /// <param name="organizationGTILink">OrganizationGTILink object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult PostOrganizationGTILink(OrganizationGTILink organizationGTILink)
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

                try
                {
                    using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                    {
                        organizationGTILink.EmployeeId = EmployeeId;
                        organizationGTILink.Id = organizationGTILink.NewId(db);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }


                        db.OrganizationGTILinks.Add(organizationGTILink);

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            if (OrganizationGTILinkExists(organizationGTILink.Id))
                            {
                                return Conflict();
                            }
                            else
                            {
                                throw;
                            }
                        }

                        if (organizationGTILink.GTIId != null)
                        {
                            organizationGTILink.OrganizationGTI = db.GTIOrganizations.Find(organizationGTILink.GTIId);
                            if (organizationGTILink.OrganizationGTI != null)
                            {
                                organizationGTILink.OrganizationGTI.Office = db.Offices.Find(organizationGTILink.OrganizationGTI.OfficeId);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return BadRequest();
                }

                OrganizationGTILinkDTO dto = organizationGTILink.ToDTO();
                return CreatedAtRoute("GetOrganizationGTILink", new { id = dto.Id }, dto);
            }
            return BadRequest();
        }

        /// <summary>
        /// Insert new organizationGTI link (with EmployeeId as creator) 
        /// </summary>
        /// <param name="organizationGTILink">OrganizationGTILink object</param>
        /// <returns></returns>
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
                            link.Id = link.NewId(db);
                            db.OrganizationGTILinks.Add(link);
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateException)
                        {
                            throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    return BadRequest();
                }

                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Delete link
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationGTILink))]
        public IHttpActionResult DeleteOrganizationGTILink(int id)
        {
            OrganizationGTILink organizationGTILink = new OrganizationGTILink();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationGTILink = db.OrganizationGTILinks.Find(id);
                    if (organizationGTILink == null)
                    {
                        return NotFound();
                    }
                    organizationGTILink.Deleted = true;
                    db.Entry(organizationGTILink).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OrganizationGTILinkExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (organizationGTILink.GTIId != null)
                    {
                        organizationGTILink.OrganizationGTI = db.GTIOrganizations.Find(organizationGTILink.GTIId);
                        if (organizationGTILink.OrganizationGTI != null)
                        {
                            organizationGTILink.OrganizationGTI.Office = db.Offices.Find(organizationGTILink.OrganizationGTI.OfficeId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            OrganizationGTILinkDTO dto = organizationGTILink.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool OrganizationGTILinkExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.OrganizationGTILinks.Count(e => e.Id == id) > 0;
            }
        }



    }

}
