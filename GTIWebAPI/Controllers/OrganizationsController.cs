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
        [GTIFilter]
        [HttpGet]
        [Route("SearchOrganization")]
        [ResponseType(typeof(IEnumerable<OrganizationSearchDTO>))]
        public IHttpActionResult SearchOrganization(int countryId, string registrationNumber)
        {
            IEnumerable<OrganizationSearchDTO> organizationList = new List<OrganizationSearchDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationList = db.SearchOrganization(countryId, registrationNumber);
                    foreach (var item in organizationList)
                    {
                        item.OrganizationGTILinks = db.GetOrganizationGTIByOrganization(item.Id);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(organizationList);
        }

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<OrganizationView>))]
        public IHttpActionResult GetOrganizationByOfficeIds(string officeIds)
        {
            IEnumerable<int> OfficeIds = QueryParser.Parse(officeIds, ',');

            IEnumerable<OrganizationView> organizationList = new List<OrganizationView>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationList = db.GetOrganizationsByOffices(OfficeIds);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(organizationList);
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
            OrganizationDTO dto = new OrganizationDTO();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    Organization organization = db.Organizations.Find(id);
                    dto = organization.MapToDTO();


                    List<OrganizationAddress> addresses =
                        db.OrganizationAddresses
                        .Where(a => a.Deleted != true && a.OrganizationId == id)
                        .Include(d => d.Address)
                        .Include(d => d.Address.AddressLocality)
                        .Include(d => d.Address.AddressPlace)
                        .Include(d => d.Address.AddressRegion)
                        .Include(d => d.Address.AddressVillage)
                        .Include(d => d.Address.Country)
                        .Include(d => d.OrganizationAddressType)
                        .ToList();
                    dto.OrganizationAddresses = addresses.Select(a => a.ToDTO());


                    List<OrganizationContactPersonView> contactPersons =
                        db.OrganizationContactPersonViews
                        .Where(p => p.Deleted != true && p.OrganizationId == id)
                        .ToList();
                    if (contactPersons != null)
                    {
                        foreach (var person in contactPersons)
                        {
                            person.OrganizationContactPersonContacts = db.OrganizationContactPersonContacts
                                .Where(d => d.OrganizationContactPersonId == person.Id)
                                .Include(d => d.ContactType)
                                .ToList();
                        }
                    }
                    dto.OrganizationContactPersons = contactPersons.Select(c => c.ToDTO());


                    List<OrganizationGTILink> links = db.OrganizationGTILinks
                        .Where(d => d.Deleted != true && d.OrganizationId == id)
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
                    dto.OrganizationGTILinks = links.Select(c => c.ToDTO());

                    List<OrganizationProperty> properties =
                        db.OrganizationProperties
                        .Where(o => o.Deleted != true && o.OrganizationId == id)
                        .Include(d => d.OrganizationPropertyType)
                        .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                        .ToList();

                    IEnumerable<int> popertyTypeIds = properties.Select(d => d.OrganizationPropertyTypeId).Distinct();
                    List<OrganizationPropertyTreeView> propertiesDTO = new List<OrganizationPropertyTreeView>();
                    foreach (int value in popertyTypeIds)
                    {
                        List<OrganizationProperty> propertiesByType = properties.Where(d => d.OrganizationPropertyTypeId == value).ToList();
                        propertiesDTO.Add(new OrganizationPropertyTreeView
                        {
                            OrganizationPropertyTypeId = value,
                            PropertiesById = propertiesByType.Select(d => d.ToDTO())
                        });
                    }
                    dto.OrganizationProperties = propertiesDTO;

                    List<OrganizationTaxAddress> taxAddresses =
                        db.OrganizationTaxAddresses
                        .Where(o => o.Deleted != true && o.OrganizationId == id)
                        .Include(d => d.Address)
                        .Include(d => d.Address.AddressRegion)
                        .Include(d => d.Address.AddressLocality)
                        .Include(d => d.Address.AddressPlace)
                        .Include(d => d.Address.AddressVillage)
                        .Include(d => d.Address.Country)
                        .ToList();
                    dto.OrganizationTaxAddresses = taxAddresses.Select(a => a.ToDTO());

                    List<OrganizationLanguageName> names =
                        db.OrganizationLanguageNames
                        .Where(o => o.Deleted != true && o.OrganizationId == id)
                        .Include(d => d.Language)
                        .ToList();
                    dto.OrganizationLanguageNames = names.Select(a => a.ToDTO());
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

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
            Organization organization = new Organization();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organization = db.Organizations.Find(id);
                    if (organization != null)
                    {
                        db.Entry(organization).Reference(d => d.Country).Load();
                        db.Entry(organization).Reference(d => d.OrganizationLegalForm).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

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

            try
            {
                using (DbMain db = new DbMain(User))
                {
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

                    db.Entry(organization).Reference(d => d.Country).Load();
                    db.Entry(organization).Reference(d => d.OrganizationLegalForm).Load();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            OrganizationEditDTO dto = organization.MapToEdit();
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

                try
                {
                    using (DbMain db = new DbMain(User))
                    {
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
                        db.Entry(organization).Reference(d => d.Country).Load();
                        db.Entry(organization).Reference(d => d.OrganizationLegalForm).Load();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest();
                }


                OrganizationEditDTO dto = organization.MapToEdit();
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
            Organization organization = new Organization();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organization = db.Organizations.Find(id);

                    if (organization != null)
                    {
                        db.Entry(organization).Reference(d => d.Country).Load();
                        db.Entry(organization).Reference(d => d.OrganizationLegalForm).Load();

                        organization.Deleted = true;
                        db.Entry(organization).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
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
            OrganizationList list = new OrganizationList();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    list = OrganizationList.CreateOrganizationList(db);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(list);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool OrganizationExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.Organizations.Count(e => e.Id == id) > 0;
            }
        }
    }
}
