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
    /// Contact persons are pepople we contact to 
    /// </summary>
    [RoutePrefix("api/OrganizationContactPersons")]
    public class OrganizationContactPersonsController : ApiController
    {
        /// <summary>
        /// Get employee propertys by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationContactPersonDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonView>))]
        public IHttpActionResult GetOrganizationContactPersonByOrganizationId(int organizationId)
        {
            List<OrganizationContactPersonView> persons = new List<OrganizationContactPersonView>();
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    persons = db.OrganizationContactPersonViews
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
                    if (persons != null)
                    {
                        foreach (var person in persons)
                        {
                            person.OrganizationContactPersonContacts =
                                db.OrganizationContactPersonContacts
                                .Where(d => d.Deleted != true && d.OrganizationContactPersonId == person.Id)
                                .Include(d => d.ContactType)
                                .ToList();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<OrganizationContactPersonDTO> dtos = persons.Select(d => d.ToDTO()).ToList();
            return Ok(persons);
        }

        /// <summary>
        /// Get one property by property id
        /// </summary>
        /// <param name="id">OrganizationContactPerson id</param>
        /// <returns>OrganizationContactPersonEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationContactPerson")]
        [ResponseType(typeof(OrganizationContactPersonView))]
        public IHttpActionResult GetOrganizationContactPerson(int id)
        {
            OrganizationContactPersonView organizationContactPersonView = new OrganizationContactPersonView();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationContactPersonView = db.OrganizationContactPersonViews.Find(id);
                    if (organizationContactPersonView != null)
                    {
                        organizationContactPersonView.OrganizationContactPersonContacts =
                            db.OrganizationContactPersonContacts
                            .Where(c => c.OrganizationContactPersonId == organizationContactPersonView.Id)
                            .Include(d => d.ContactType)
                            .ToList();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            OrganizationContactPersonDTO dto = organizationContactPersonView.ToDTO();
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
            OrganizationContactPersonView organizationContactPersonView = new OrganizationContactPersonView();

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
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
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

                    organizationContactPersonView = db.OrganizationContactPersonViews.Find(organizationContactPerson.Id);
                    organizationContactPersonView.OrganizationContactPersonContacts = db.OrganizationContactPersonContacts
                       .Where(c => c.OrganizationContactPersonId == organizationContactPerson.Id)
                       .Include(d => d.ContactType)
                       .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
           

            OrganizationContactPersonDTO dto = organizationContactPersonView.ToDTO();

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
            OrganizationContactPersonDTO dto = new OrganizationContactPersonDTO();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationContactPerson.Id = organizationContactPerson.NewId(db);
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.OrganizationContactPersons.Add(organizationContactPerson);
                    


                    if (organizationContactPerson.OrganizationContactPersonContact != null)
                    {
                        if (organizationContactPerson.OrganizationContactPersonContact.Count > 0)
                        {
                            foreach (var contact in organizationContactPerson.OrganizationContactPersonContact)
                            {
                                contact.Id = contact.NewId(db);
                                contact.OrganizationContactPersonId = organizationContactPerson.Id;
                                db.OrganizationContactPersonContacts.Add(contact);
                            }
                        }
                    }

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

                    OrganizationContactPersonView organizationContactPersonView = db.OrganizationContactPersonViews.Find(organizationContactPerson.Id);
                    organizationContactPersonView.OrganizationContactPersonContacts = db.OrganizationContactPersonContacts
                       .Where(c => c.OrganizationContactPersonId == organizationContactPerson.Id)
                       .ToList();
                    dto = organizationContactPersonView.ToDTO();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

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
            OrganizationContactPerson organizationContactPerson = new OrganizationContactPerson();
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationContactPerson = db.OrganizationContactPersons.Find(id);
                    if (organizationContactPerson == null)
                    {
                        return NotFound();
                    }
                    db.Entry(organizationContactPerson).Collection(d => d.OrganizationContactPersonContact).Load();

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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
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
            base.Dispose(disposing);
        }

        private bool OrganizationContactPersonExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.OrganizationContactPersons.Count(e => e.Id == id) > 0;
            }    
        }
    }
}
