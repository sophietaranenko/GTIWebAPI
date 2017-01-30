using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
    [RoutePrefix("api/OrganizationContactPersonContacts")]
    public class OrganizationContactPersonContactsController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get employee contacts by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationContactPersonContactDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationContactPersonId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonContactDTO>))]
        public IEnumerable<OrganizationContactPersonContactDTO> GetOrganizationContactPersonContactByOrganizationContactPersonId(int organizationContactPersonId)
        {
            List<OrganizationContactPersonContact> ptoperties = db.OrganizationContactPersonContacts
                .Where(p => p.Deleted != true && p.OrganizationContactPersonId == organizationContactPersonId).ToList();
            List<OrganizationContactPersonContactDTO> dtos = ptoperties.Select(p => p.ToDTO()).ToList();
            return dtos;
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
            OrganizationContactPersonContact contact = db.OrganizationContactPersonContacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            OrganizationContactPersonContactDTO dto = contact.ToDTO();
            return Ok(dto);
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationContactPersonContact(int id, OrganizationContactPersonContact organizationContactPersonContact)
        {
            if (organizationContactPersonContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationContactPersonContact.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationContactPersonContact).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationContactPersonContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually
            if (organizationContactPersonContact.ContactTypeId != null)
            {
                organizationContactPersonContact.ContactType = db.ContactTypes.Find(organizationContactPersonContact.ContactTypeId);
            }
            OrganizationContactPersonContactDTO dto = organizationContactPersonContact.ToDTO();
            return Ok(dto);
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
            organizationContactPersonContact.Id = organizationContactPersonContact.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.OrganizationContactPersonContacts.Add(organizationContactPersonContact);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationContactPersonContactExists(organizationContactPersonContact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually
            if (organizationContactPersonContact.ContactTypeId != null)
            {
                organizationContactPersonContact.ContactType = db.ContactTypes.Find(organizationContactPersonContact.ContactTypeId);
            }
            OrganizationContactPersonContactDTO dto = organizationContactPersonContact.ToDTO();
            return CreatedAtRoute("GetOrganizationContactPersonContact", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationContactPersonContact))]
        public IHttpActionResult DeleteOrganizationContactPersonContact(int id)
        {
            OrganizationContactPersonContact organizationContactPersonContact = db.OrganizationContactPersonContacts.Find(id);
            if (organizationContactPersonContact == null)
            {
                return NotFound();
            }
            organizationContactPersonContact.Deleted = true;
            db.Entry(organizationContactPersonContact).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationContactPersonContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            OrganizationContactPersonContactDTO dto = organizationContactPersonContact.ToDTO();
            return Ok(dto);
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

        private bool OrganizationContactPersonContactExists(int id)
        {
            return db.OrganizationContactPersonContacts.Count(e => e.Id == id) > 0;
        }
    }
}
