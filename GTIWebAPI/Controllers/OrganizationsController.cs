using GTIWebAPI.Filters;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GTIWebAPI.Models.Dictionary;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for organizations
    /// </summary>
    [RoutePrefix("api/Organizations")]
    public class OrganizationsController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        [GTIFilter]
        [HttpGet]
        [Route("SearchOrganization")]
        public IEnumerable<OrganizationSearchDTO> SearchOrganization(int countryId, string registrationNumber)
        {
            IEnumerable<OrganizationSearchDTO> organizationList = db.SearchOrganization(countryId, registrationNumber);
            foreach (var item in organizationList)
            {
                item.OrganizationGTILinks = db.GetOrganizationGTIByOrganization(item.Id);
            }
            return organizationList;
        }

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<OrganizationView> GetOrganizationByOfficeIds(string officeIds)
        {
            IEnumerable<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            IEnumerable<OrganizationView> organizationList = db.GetOrganizationsByOffices(OfficeIds);
            return organizationList;
        }


        /// <summary>
        /// Get one organization by organization Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetView", Name = "GetOrganizationView")]
        [ResponseType(typeof(OrganizationDTO))]
        public IHttpActionResult GetOrganizationView(int id)
        {
            Organization organization = db.Organizations.Find(id);
            OrganizationDTO dto = organization.MapToDTO();


            List<OrganizationAddress> addresses = db.OrganizationAddresses.Where(a => a.Deleted != true && a.OrganizationId == id).ToList();
            dto.OrganizationAddresses = addresses.Select(a => a.ToDTO());


            List<OrganizationContactPerson> contactPersons = db.OrganizationContactPersons.Where(p => p.Deleted != true && p.OrganizationId == id).ToList();
            dto.OrganizationContactPersons = contactPersons.Select(c => c.ToDTO());


            List<OrganizationGTILink> links = db.OrganizationGTILinks.Where(d => d.Deleted != true && d.OrganizationId == id).ToList();
            //поскольку в таблице OrganizationGTILink не может быть внешних ключей на старые базы 
            //можем доставать объекты только по такому сценарию
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
            dto.OrganizationGTILinks = links.Select(c => c.ToDTO());

            List<OrganizationProperty> properties = db.OrganizationProperties.Where(o => o.Deleted != true && o.OrganizationId == id).ToList();
            dto.OrganizationProperties = properties.Select(s => s.ToDTO());

            List<OrganizationTaxAddress> taxAddresses = db.OrganizationTaxAddresses.Where(o => o.Deleted != true && o.OrganizationId == id).ToList();
            dto.OrganizationTaxAddresses = taxAddresses.Select(a => a.ToDTO());

            List<OrganizationLanguageName> names = db.OrganizationLanguageNames.Where(o => o.Deleted != true && o.OrganizationId == id).ToList();
            dto.OrganizationLanguageNames = names.Select(a => a.ToDTO());

            List<OrganizationLanguageShortName> shortNames = db.OrganizationLanguageShortNames.Where(o => o.Deleted != true && o.OrganizationId == id).ToList();
            dto.OrganizationLanguageShortNames = shortNames.Select(d => d.ToDTO());

            return Ok(dto);
        }

        /// <summary>
        /// Get one organization by organization Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEdit", Name = "GetOrganizationEdit")]
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult GetOrganizationEdit(int id)
        {
            Organization organization = db.Organizations.Find(id);
            OrganizationEditDTO dto = organization.MapToEdit();
            return Ok(dto);
        }


        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutOrganization(int id, Organization organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != organization.Id)
            {
                return BadRequest();
            }
            
            db.Entry(organization).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!OrganizationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            OrganizationEditDTO dto = db.Organizations.Find(organization.Id).MapToEdit();
            return Ok(dto);
        }


        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult PostOrganization(Organization organization)
        {
            string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

            if (user != null && user.TableName == "Employee")
            {
                organization.EmployeeId = user.TableId;
                organization.Id = organization.NewId(db);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Organizations.Add(organization);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationExists(organization.Id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                OrganizationEditDTO dto = db.Organizations.Find(organization.Id).MapToEdit();
                return CreatedAtRoute("GetOrganizationEdit", new { id = dto.Id }, dto);
            }

            return BadRequest();            
        }

        /// <summary>
        /// Delete organization
        /// </summary>
        /// <param name="id">organization id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteOrganization(int id)
        {
            Organization organization = db.Organizations.Find(id);

            if (organization != null)
            {
                organization.Deleted = true;
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
            }

            OrganizationEditDTO dto = organization.MapToEdit();
            return Ok(dto);
        }



        [GTIFilter]
        [HttpGet]
        [Route("GetOrganizationList")]
        [ResponseType(typeof(OrganizationList))]
        public IHttpActionResult GetOrganizationTypes()
        {
            OrganizationList list = OrganizationList.CreateOrganizationList(db);
            return Ok(list);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrganizationExists(int id)
        {
            return db.Organizations.Count(e => e.Id == id) > 0;
        }
    }
}
