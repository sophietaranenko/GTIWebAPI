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
    public class EmployeeDrivingLicensesController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeDrivingLicenses
        public IQueryable<EmployeeDrivingLicense> GetEmployeeDrivingLicense()
        {
            return db.EmployeeDrivingLicense;
        }

        // GET: api/EmployeeDrivingLicenses/5
        [ResponseType(typeof(EmployeeDrivingLicense))]
        public IHttpActionResult GetEmployeeDrivingLicense(int id)
        {
            EmployeeDrivingLicense employeeDrivingLicense = db.EmployeeDrivingLicense.Find(id);
            if (employeeDrivingLicense == null)
            {
                return NotFound();
            }

            return Ok(employeeDrivingLicense);
        }

        // PUT: api/EmployeeDrivingLicenses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeDrivingLicense(int id, EmployeeDrivingLicense employeeDrivingLicense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeDrivingLicense.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeDrivingLicense).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeDrivingLicenseExists(id))
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

        // POST: api/EmployeeDrivingLicenses
        [ResponseType(typeof(EmployeeDrivingLicense))]
        public IHttpActionResult PostEmployeeDrivingLicense(EmployeeDrivingLicense employeeDrivingLicense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeDrivingLicense.Add(employeeDrivingLicense);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeDrivingLicenseExists(employeeDrivingLicense.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeDrivingLicense.Id }, employeeDrivingLicense);
        }

        // DELETE: api/EmployeeDrivingLicenses/5
        [ResponseType(typeof(EmployeeDrivingLicense))]
        public IHttpActionResult DeleteEmployeeDrivingLicense(int id)
        {
            EmployeeDrivingLicense employeeDrivingLicense = db.EmployeeDrivingLicense.Find(id);
            if (employeeDrivingLicense == null)
            {
                return NotFound();
            }

            db.EmployeeDrivingLicense.Remove(employeeDrivingLicense);
            db.SaveChanges();

            return Ok(employeeDrivingLicense);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeDrivingLicenseExists(int id)
        {
            return db.EmployeeDrivingLicense.Count(e => e.Id == id) > 0;
        }
    }
}