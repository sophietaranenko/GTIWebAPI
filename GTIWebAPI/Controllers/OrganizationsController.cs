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
        [Route("GetByFilter")]
        public IEnumerable<OrganizationView> GetOrganizationsByFilter(string filter)
        {
            IEnumerable<OrganizationView> organizationList = db.OrganizationsFilter(filter);
            return organizationList;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        public IEnumerable<OrganizationView> GetOrganizationsByEmployeeId(int employeeId)
        {
            IEnumerable<OrganizationView> organizationList = db.OrganizationsFilter("")
                .Where(c => c.EmployeeId == employeeId).ToList();
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


            List<OrganizationAddress> addresses = db.OrganizationAddresses.Where(a => a.Deleted != true).ToList();
            dto.OrganizationAddresses = addresses.Select(a => a.ToDTO());


            List<OrganizationContactPerson> contactPersons = db.OrganizationContactPersons.Where(p => p.Deleted != true).ToList();
            dto.OrganizationContactPersons = contactPersons.Select(c => c.ToDTO());


            List<OrganizationGTILink> links = db.OrganizationGTILinks.Where(d => d.Deleted != true).ToList();
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



            dto.OrganizationProperties = new List<OrganizationPropertyDTO>();

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
        public IHttpActionResult GetOrganization(int id)
        {
            Mapper.Initialize(m => m.CreateMap<Address, AddressDTO>());
            Organization organization = db.Organizations.Find(id);
            OrganizationEditDTO dto = organization.MapToEdit();
            return Ok(dto);
        }


        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutOrganization(int id, OrganizationEditDTO organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organization.Id)
            {
                return BadRequest();
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationEditDTO, Organization>();
                m.CreateMap<OrganizationLegalFormDTO, OrganizationLegalForm>();
            });

            Organization newOrganization = Mapper.Map<Organization>(organization);
            db.Entry(newOrganization).State = EntityState.Modified;

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
            return StatusCode(HttpStatusCode.NoContent);
        }


        [GTIFilter]
        [HttpPost]
        [Route("PostOrganization")]
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult PostOrganization(OrganizationEditDTO postDto)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationEditDTO, Organization>();
                m.CreateMap<OrganizationLegalFormDTO, OrganizationLegalForm>();
                m.CreateMap<AddressDTO, Address>();
            });

            Organization organization = Mapper.Map<Organization>(postDto);

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
            OrganizationEditDTO dto = organization.MapToEdit();
            return CreatedAtRoute("GetOrganizationEdit", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete organization
        /// </summary>
        /// <param name="id">organization id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteOrganization")]
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


        //instead of OrganizationType - OwnershipForm && method GetLists for all help lists in Organizations namespace
        //[GTIFilter]
        //[HttpGet]
        //[Route("GetOrganizationTypes")]
        //public IEnumerable<OrganizationTypeDTO> GetOrganizationTypes()
        //{
        //    List<OrganizationType> types = db.OrganizationTypes.OrderBy(o => o.Name).ToList();
        //    Mapper.Initialize(m => m.CreateMap<OrganizationType, OrganizationTypeDTO>());
        //    IEnumerable<OrganizationTypeDTO> dtos = Mapper.Map<IEnumerable<OrganizationType>, IEnumerable<OrganizationTypeDTO>>(types);
        //    return dtos;
        //}

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
