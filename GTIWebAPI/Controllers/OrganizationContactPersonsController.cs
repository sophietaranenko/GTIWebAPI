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
    [RoutePrefix("api/OrganizationContactPersons")]
    public class OrganizationContactPersonsController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get employee propertys by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationContactPersonDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonDTO>))]
        public IEnumerable<OrganizationContactPersonDTO> GetOrganizationContactPersonByOrganizationId(int organizationId)
        {
            List<OrganizationContactPerson> persons = db.OrganizationContactPersons
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
            List<OrganizationContactPersonDTO> dtos = persons.Select(p => p.ToDTO()).ToList();
            return dtos;
        }

        /// <summary>
        /// Get one property by property id
        /// </summary>
        /// <param name="id">OrganizationContactPerson id</param>
        /// <returns>OrganizationContactPersonEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationContactPerson")]
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult GetOrganizationContactPerson(int id)
        {
            OrganizationContactPerson person = db.OrganizationContactPersons.Find(id);
            if (person == null)
            {
                return NotFound();
            }
            OrganizationContactPersonDTO dto = person.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee property
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationContactPerson">OrganizationContactPerson object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationContactPerson(int id, OrganizationContactPerson organizationContactPerson)
        {
            if (organizationContactPerson == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationContactPerson.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationContactPerson).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationContactPersonExists(id))
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
            organizationContactPerson.OrganizationContactPersonContact = db.OrganizationContactPersonContacts
               .Where(c => c.OrganizationContactPersonId == organizationContactPerson.Id)
               .ToList();
            OrganizationContactPersonDTO dto = organizationContactPerson.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee property
        /// </summary>
        /// <param name="organizationContactPerson">OrganizationContactPerson object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult PostOrganizationContactPerson(OrganizationContactPerson organizationContactPerson)
        {
            if (organizationContactPerson == null)
            {
                return BadRequest(ModelState);
            }
            organizationContactPerson.Id = organizationContactPerson.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.OrganizationContactPersons.Add(organizationContactPerson);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationContactPersonExists(organizationContactPerson.Id))
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
            organizationContactPerson.OrganizationContactPersonContact = db.OrganizationContactPersonContacts
                .Where(c => c.OrganizationContactPersonId == organizationContactPerson.Id)
                .ToList();
            OrganizationContactPersonDTO dto = organizationContactPerson.ToDTO();
            return CreatedAtRoute("GetOrganizationContactPerson", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete property
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationContactPerson))]
        public IHttpActionResult DeleteOrganizationContactPerson(int id)
        {
            OrganizationContactPerson organizationContactPerson = db.OrganizationContactPersons.Find(id);
            if (organizationContactPerson == null)
            {
                return NotFound();
            }
            organizationContactPerson.Deleted = true;
            db.Entry(organizationContactPerson).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationContactPersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            OrganizationContactPersonDTO dto = organizationContactPerson.ToDTO();
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

        private bool OrganizationContactPersonExists(int id)
        {
            return db.OrganizationContactPersons.Count(e => e.Id == id) > 0;
        }
    }
}
