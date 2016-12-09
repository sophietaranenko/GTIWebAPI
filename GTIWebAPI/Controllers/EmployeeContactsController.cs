using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;

namespace GTIWebAPI.Controllers
{
    public class EmployeeContactsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeContacts
        public IQueryable<EmployeeContact> GetEmployeeContact()
        {
            return db.EmployeeContact;
        }

        // GET: api/EmployeeContacts/5
        [ResponseType(typeof(EmployeeContact))]
        public IHttpActionResult GetEmployeeContact(int id)
        {
            EmployeeContact employeeContact = db.EmployeeContact.Find(id);
            if (employeeContact == null)
            {
                return NotFound();
            }

            return Ok(employeeContact);
        }

        // PUT: api/EmployeeContacts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeContact(int id, EmployeeContact employeeContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeContact.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeContact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeContactExists(id))
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

        // POST: api/EmployeeContacts
        [ResponseType(typeof(EmployeeContact))]
        public IHttpActionResult PostEmployeeContact(EmployeeContact employeeContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeContact.Add(employeeContact);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeContactExists(employeeContact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeContact.Id }, employeeContact);
        }

        // DELETE: api/EmployeeContacts/5
        [ResponseType(typeof(EmployeeContact))]
        public IHttpActionResult DeleteEmployeeContact(int id)
        {
            EmployeeContact employeeContact = db.EmployeeContact.Find(id);
            if (employeeContact == null)
            {
                return NotFound();
            }

            db.EmployeeContact.Remove(employeeContact);
            db.SaveChanges();

            return Ok(employeeContact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeContactExists(int id)
        {
            return db.EmployeeContact.Count(e => e.Id == id) > 0;
        }
    }
}