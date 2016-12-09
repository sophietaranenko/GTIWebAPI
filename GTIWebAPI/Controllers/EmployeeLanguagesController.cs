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
    public class EmployeeLanguagesController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeLanguages
        public IQueryable<EmployeeLanguage> GetEmployeeLanguage()
        {
            return db.EmployeeLanguage;
        }

        // GET: api/EmployeeLanguages/5
        [ResponseType(typeof(EmployeeLanguage))]
        public IHttpActionResult GetEmployeeLanguage(int id)
        {
            EmployeeLanguage employeeLanguage = db.EmployeeLanguage.Find(id);
            if (employeeLanguage == null)
            {
                return NotFound();
            }

            return Ok(employeeLanguage);
        }

        // PUT: api/EmployeeLanguages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeLanguage(int id, EmployeeLanguage employeeLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeLanguage.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeLanguage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeLanguageExists(id))
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

        // POST: api/EmployeeLanguages
        [ResponseType(typeof(EmployeeLanguage))]
        public IHttpActionResult PostEmployeeLanguage(EmployeeLanguage employeeLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeLanguage.Add(employeeLanguage);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeLanguageExists(employeeLanguage.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeLanguage.Id }, employeeLanguage);
        }

        // DELETE: api/EmployeeLanguages/5
        [ResponseType(typeof(EmployeeLanguage))]
        public IHttpActionResult DeleteEmployeeLanguage(int id)
        {
            EmployeeLanguage employeeLanguage = db.EmployeeLanguage.Find(id);
            if (employeeLanguage == null)
            {
                return NotFound();
            }

            db.EmployeeLanguage.Remove(employeeLanguage);
            db.SaveChanges();

            return Ok(employeeLanguage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeLanguageExists(int id)
        {
            return db.EmployeeLanguage.Count(e => e.Id == id) > 0;
        }
    }
}