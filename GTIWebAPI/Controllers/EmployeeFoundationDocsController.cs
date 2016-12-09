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
    public class EmployeeFoundationDocsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeFoundationDocs
        public IQueryable<EmployeeFoundationDoc> GetEmployeeFoundationDoc()
        {
            return db.EmployeeFoundationDoc;
        }

        // GET: api/EmployeeFoundationDocs/5
        [ResponseType(typeof(EmployeeFoundationDoc))]
        public IHttpActionResult GetEmployeeFoundationDoc(int id)
        {
            EmployeeFoundationDoc employeeFoundationDoc = db.EmployeeFoundationDoc.Find(id);
            if (employeeFoundationDoc == null)
            {
                return NotFound();
            }

            return Ok(employeeFoundationDoc);
        }

        // PUT: api/EmployeeFoundationDocs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeFoundationDoc(int id, EmployeeFoundationDoc employeeFoundationDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeFoundationDoc.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeFoundationDoc).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeFoundationDocExists(id))
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

        // POST: api/EmployeeFoundationDocs
        [ResponseType(typeof(EmployeeFoundationDoc))]
        public IHttpActionResult PostEmployeeFoundationDoc(EmployeeFoundationDoc employeeFoundationDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeFoundationDoc.Add(employeeFoundationDoc);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeFoundationDocExists(employeeFoundationDoc.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeFoundationDoc.Id }, employeeFoundationDoc);
        }

        // DELETE: api/EmployeeFoundationDocs/5
        [ResponseType(typeof(EmployeeFoundationDoc))]
        public IHttpActionResult DeleteEmployeeFoundationDoc(int id)
        {
            EmployeeFoundationDoc employeeFoundationDoc = db.EmployeeFoundationDoc.Find(id);
            if (employeeFoundationDoc == null)
            {
                return NotFound();
            }

            db.EmployeeFoundationDoc.Remove(employeeFoundationDoc);
            db.SaveChanges();

            return Ok(employeeFoundationDoc);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeFoundationDocExists(int id)
        {
            return db.EmployeeFoundationDoc.Count(e => e.Id == id) > 0;
        }
    }
}