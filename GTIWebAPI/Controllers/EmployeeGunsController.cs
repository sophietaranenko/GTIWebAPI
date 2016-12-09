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
    public class EmployeeGunsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeGuns
        public IQueryable<EmployeeGun> GetEmployeeGun()
        {
            return db.EmployeeGun;
        }

        // GET: api/EmployeeGuns/5
        [ResponseType(typeof(EmployeeGun))]
        public IHttpActionResult GetEmployeeGun(int id)
        {
            EmployeeGun employeeGun = db.EmployeeGun.Find(id);
            if (employeeGun == null)
            {
                return NotFound();
            }

            return Ok(employeeGun);
        }

        // PUT: api/EmployeeGuns/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeGun(int id, EmployeeGun employeeGun)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeGun.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeGun).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeGunExists(id))
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

        // POST: api/EmployeeGuns
        [ResponseType(typeof(EmployeeGun))]
        public IHttpActionResult PostEmployeeGun(EmployeeGun employeeGun)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeGun.Add(employeeGun);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeGunExists(employeeGun.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeGun.Id }, employeeGun);
        }

        // DELETE: api/EmployeeGuns/5
        [ResponseType(typeof(EmployeeGun))]
        public IHttpActionResult DeleteEmployeeGun(int id)
        {
            EmployeeGun employeeGun = db.EmployeeGun.Find(id);
            if (employeeGun == null)
            {
                return NotFound();
            }

            db.EmployeeGun.Remove(employeeGun);
            db.SaveChanges();

            return Ok(employeeGun);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeGunExists(int id)
        {
            return db.EmployeeGun.Count(e => e.Id == id) > 0;
        }
    }
}