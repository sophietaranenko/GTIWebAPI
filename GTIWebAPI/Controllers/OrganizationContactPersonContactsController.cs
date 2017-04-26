using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository;
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
        IDbContextFactory factory;

        public OrganizationContactPersonContactsController()
        {
            factory = new DbContextFactory();
        }

        public OrganizationContactPersonContactsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationContactPersonId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonContactDTO>))]
        public IHttpActionResult GetOrganizationContactPersonContactByOrganizationContactPersonId(int organizationContactPersonId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<OrganizationContactPersonContactDTO> dtos = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Deleted != true && d.OrganizationContactPersonId == organizationContactPersonId, includeProperties: "ContactType")
                    .Select(d => d.ToDTO());
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationContactPersonContactDTO dto = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Id == id, includeProperties: "ContactType")
                    .FirstOrDefault()
                    .ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationContactPersonContactsRepository.Update(organizationContactPersonContact);
                unitOfWork.Save();
                OrganizationContactPersonContactDTO dto = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Id == id, includeProperties: "ContactType")
                    .FirstOrDefault()
                    .ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                organizationContactPersonContact.Id = organizationContactPersonContact.NewId(unitOfWork);
                unitOfWork.OrganizationContactPersonContactsRepository.Insert(organizationContactPersonContact);
                unitOfWork.Save();
                OrganizationContactPersonContact c = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Id == organizationContactPersonContact.Id, includeProperties: "ContactType").FirstOrDefault();
                OrganizationContactPersonContactDTO dto = c.ToDTO();
                return CreatedAtRoute("GetOrganizationContactPersonContact", new { id = dto.Id }, dto);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationContactPersonContact contact = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Id == id, includeProperties: "ContactType")
                    .FirstOrDefault();
                contact.Deleted = true;
                unitOfWork.OrganizationContactPersonContactsRepository.Update(contact);
                OrganizationContactPersonContactDTO dto = contact.ToDTO();
                return Ok(dto);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
