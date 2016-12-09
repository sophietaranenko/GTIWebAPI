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
    public class EmployeeMilitaryCardsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        // GET: api/EmployeeMilitaryCards
        public IQueryable<EmployeeMilitaryCard> GetEmployeeMilitaryCard()
        {
            return db.EmployeeMilitaryCard;
        }

        // GET: api/EmployeeMilitaryCards/5
        [ResponseType(typeof(EmployeeMilitaryCard))]
        public IHttpActionResult GetEmployeeMilitaryCard(int id)
        {
            EmployeeMilitaryCard employeeMilitaryCard = db.EmployeeMilitaryCard.Find(id);
            if (employeeMilitaryCard == null)
            {
                return NotFound();
            }

            return Ok(employeeMilitaryCard);
        }

        // PUT: api/EmployeeMilitaryCards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeMilitaryCard(int id, EmployeeMilitaryCard employeeMilitaryCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeMilitaryCard.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeMilitaryCard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeMilitaryCardExists(id))
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

        // POST: api/EmployeeMilitaryCards
        [ResponseType(typeof(EmployeeMilitaryCard))]
        public IHttpActionResult PostEmployeeMilitaryCard(EmployeeMilitaryCard employeeMilitaryCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeMilitaryCard.Add(employeeMilitaryCard);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeMilitaryCardExists(employeeMilitaryCard.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employeeMilitaryCard.Id }, employeeMilitaryCard);
        }

        // DELETE: api/EmployeeMilitaryCards/5
        [ResponseType(typeof(EmployeeMilitaryCard))]
        public IHttpActionResult DeleteEmployeeMilitaryCard(int id)
        {
            EmployeeMilitaryCard employeeMilitaryCard = db.EmployeeMilitaryCard.Find(id);
            if (employeeMilitaryCard == null)
            {
                return NotFound();
            }

            db.EmployeeMilitaryCard.Remove(employeeMilitaryCard);
            db.SaveChanges();

            return Ok(employeeMilitaryCard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeMilitaryCardExists(int id)
        {
            return db.EmployeeMilitaryCard.Count(e => e.Id == id) > 0;
        }
    }
}