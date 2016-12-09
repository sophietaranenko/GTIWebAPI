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
    public class EmployeeInternationalPassportsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeInternationalPassports
        public IQueryable<EmployeeInternationalPassport> GetEmployeeInternationalPassport()
        {
            return db.EmployeeInternationalPassport;
        }

        // GET: api/EmployeeInternationalPassports/5
        [ResponseType(typeof(EmployeeInternationalPassport))]
        public IHttpActionResult GetEmployeeInternationalPassport(int id)
        {
            EmployeeInternationalPassport employeeInternationalPassport = db.EmployeeInternationalPassport.Find(id);
            if (employeeInternationalPassport == null)
            {
                return NotFound();
            }

            return Ok(employeeInternationalPassport);
        }

        // PUT: api/EmployeeInternationalPassports/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeInternationalPassport(int id, EmployeeInternationalPassport employeeInternationalPassport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeInternationalPassport.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeInternationalPassport).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeInternationalPassportExists(id))
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

        // POST: api/EmployeeInternationalPassports
        [ResponseType(typeof(EmployeeInternationalPassport))]
        public IHttpActionResult PostEmployeeInternationalPassport(EmployeeInternationalPassport employeeInternationalPassport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeInternationalPassport.Add(employeeInternationalPassport);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeInternationalPassportExists(employeeInternationalPassport.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeInternationalPassport.Id }, employeeInternationalPassport);
        }

        // DELETE: api/EmployeeInternationalPassports/5
        [ResponseType(typeof(EmployeeInternationalPassport))]
        public IHttpActionResult DeleteEmployeeInternationalPassport(int id)
        {
            EmployeeInternationalPassport employeeInternationalPassport = db.EmployeeInternationalPassport.Find(id);
            if (employeeInternationalPassport == null)
            {
                return NotFound();
            }

            db.EmployeeInternationalPassport.Remove(employeeInternationalPassport);
            db.SaveChanges();

            return Ok(employeeInternationalPassport);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeInternationalPassportExists(int id)
        {
            return db.EmployeeInternationalPassport.Count(e => e.Id == id) > 0;
        }
    }
}