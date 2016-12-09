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
    public class EmployeeCarsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeCars
        public IQueryable<EmployeeCar> GetEmployeeCar()
        {
            return db.EmployeeCar;
        }

        // GET: api/EmployeeCars/5
        [ResponseType(typeof(EmployeeCar))]
        public IHttpActionResult GetEmployeeCar(int id)
        {
            EmployeeCar employeeCar = db.EmployeeCar.Find(id);
            if (employeeCar == null)
            {
                return NotFound();
            }

            return Ok(employeeCar);
        }

        // PUT: api/EmployeeCars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeCar(int id, EmployeeCar employeeCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeCar.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeCar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeCarExists(id))
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

        // POST: api/EmployeeCars
        [ResponseType(typeof(EmployeeCar))]
        public IHttpActionResult PostEmployeeCar(EmployeeCar employeeCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeCar.Add(employeeCar);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeCarExists(employeeCar.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeCar.Id }, employeeCar);
        }

        // DELETE: api/EmployeeCars/5
        [ResponseType(typeof(EmployeeCar))]
        public IHttpActionResult DeleteEmployeeCar(int id)
        {
            EmployeeCar employeeCar = db.EmployeeCar.Find(id);
            if (employeeCar == null)
            {
                return NotFound();
            }

            db.EmployeeCar.Remove(employeeCar);
            db.SaveChanges();

            return Ok(employeeCar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeCarExists(int id)
        {
            return db.EmployeeCar.Count(e => e.Id == id) > 0;
        }
    }
}