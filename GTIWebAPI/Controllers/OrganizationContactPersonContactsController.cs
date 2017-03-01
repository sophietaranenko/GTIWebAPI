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
    /// <summary>
    /// Persons with whom our company has business  
    /// </summary>
    [RoutePrefix("api/OrganizationContactPersonContacts")]
    public class OrganizationContactPersonContactsController : ApiController
    {
        /// <summary>
        /// Get employee contacts by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationContactPersonContactDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationContactPersonId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonContactDTO>))]
        public IHttpActionResult GetOrganizationContactPersonContactByOrganizationContactPersonId(int organizationContactPersonId)
        {
            List<OrganizationContactPersonContact> organizationContactPersonContacts = new List<OrganizationContactPersonContact>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationContactPersonContacts = db.OrganizationContactPersonContacts
                .Where(p => p.Deleted != true && p.OrganizationContactPersonId == organizationContactPersonId)
                .Include(d => d.ContactType)
                .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<OrganizationContactPersonContactDTO> dtos = organizationContactPersonContacts.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
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
            OrganizationContactPersonContact organizationContactPersonContact = new OrganizationContactPersonContact();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationContactPersonContact = db.OrganizationContactPersonContacts.Find(id);
                    if (organizationContactPersonContact != null)
                    {
                        db.Entry(organizationContactPersonContact).Reference(d => d.ContactType).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (organizationContactPersonContact == null)
            {
                return NotFound();
            }

            OrganizationContactPersonContactDTO dto = organizationContactPersonContact.ToDTO();
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
            try
            {
                using (DbMain db = new DbMain(User))
                {
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
                    db.Entry(organizationContactPersonContact).Reference(d => d.ContactType).Load();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
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
            try
            {
                using (DbMain db = new DbMain(User))
                {

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
                    db.Entry(organizationContactPersonContact).Reference(d => d.ContactType).Load();
                }

            }
            catch (Exception e)
            {
                return BadRequest();
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
            OrganizationContactPersonContact organizationContactPersonContact = new OrganizationContactPersonContact();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationContactPersonContact = db.OrganizationContactPersonContacts.Find(id);
                    if (organizationContactPersonContact == null)
                    {
                        return NotFound();
                    }
                    db.Entry(organizationContactPersonContact).Reference(d => d.ContactType).Load();

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

                }

            }
            catch (Exception e)
            {
                return BadRequest();
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
            base.Dispose(disposing);
        }

        private bool OrganizationContactPersonContactExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.OrganizationContactPersonContacts.Count(e => e.Id == id) > 0;
            }
        }
    }
}
