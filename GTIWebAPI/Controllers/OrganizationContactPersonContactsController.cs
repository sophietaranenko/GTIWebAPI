using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository.Organization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Persons with whom our company has business  
    /// </summary>
    [RoutePrefix("api/OrganizationContactPersonContacts")]
    public class OrganizationContactPersonContactsController : ApiController
    {
        IOrganizationContactPersonContactsRepository repo;

        public OrganizationContactPersonContactsController()
        {
            repo = new OrganizationContactPersonContactsRepository();
        }

        public OrganizationContactPersonContactsController(IOrganizationContactPersonContactsRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationContactPersonId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonContactDTO>))]
        public IHttpActionResult GetOrganizationContactPersonContactByOrganizationContactPersonId(int organizationContactPersonId)
        {
            try
            {
                List<OrganizationContactPersonContactDTO> dtos = 
                    repo.GetByOrganizationContactPersonId(organizationContactPersonId)
                    .Select(p => p.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get one contact by contact id
        /// </summary>
        /// <param name="id">OrganizationContactPersonContact id</param>
        /// <returns>OrganizationContactPersonContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationContactPersonContact")]
        [ResponseType(typeof(OrganizationContactPersonContactDTO))]
        public IHttpActionResult GetOrganizationContactPersonContact(int id)
        {
            try
            {
                OrganizationContactPersonContactDTO dto = 
                    repo.Get(id)
                    .ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update employee contact
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationContactPersonContact">OrganizationContactPersonContact object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(OrganizationContactPersonContactDTO))]
        public IHttpActionResult PutOrganizationContactPersonContact(int id, OrganizationContactPersonContact organizationContactPersonContact)
        {
            if (organizationContactPersonContact == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationContactPersonContact.Id)
            {
                return BadRequest();
            }
            try
            {
                OrganizationContactPersonContactDTO dto = repo.Edit(organizationContactPersonContact).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Insert new employee contact
        /// </summary>
        /// <param name="organizationContactPersonContact">OrganizationContactPersonContact object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationContactPersonContactDTO))]
        public IHttpActionResult PostOrganizationContactPersonContact(OrganizationContactPersonContact organizationContactPersonContact)
        {
            if (organizationContactPersonContact == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizationContactPersonContactDTO dto = repo.Add(organizationContactPersonContact).ToDTO();
                return CreatedAtRoute("GetOrganizationContactPersonContact", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationContactPersonContactDTO))]
        public IHttpActionResult DeleteOrganizationContactPersonContact(int id)
        {
            try
            {
                OrganizationContactPersonContactDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
