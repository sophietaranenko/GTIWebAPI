using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Filters;
using System;
using System.Net;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee contacts (facebook, skype, home phone number etc. and their values) 
    /// </summary>
    [RoutePrefix("api/EmployeeContacts")]
    public class EmployeeContactsController : ApiController
    {
        /// <summary>
        /// To get all not deleted contacts 
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IHttpActionResult GetEmployeeContactAll()
        {
            IEnumerable<EmployeeContactDTO> dtos = new List<EmployeeContactDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeContacts.Where(p => p.Deleted != true).Include(b => b.ContactType).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get currenct employee contacts by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IHttpActionResult GetEmployeeContactByEmployee(int employeeId)
        {
            IEnumerable<EmployeeContactDTO> dtos = new List<EmployeeContactDTO>();

            try
            {
               using (DbMain db = new DbMain(User))
               {
                    dtos = db.EmployeeContacts.Where(p => p.Deleted != true && p.EmployeeId == employeeId).Include(c => c.ContactType).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(dtos);
        }


        /// <summary>
        /// Get one contact for edit by contact id
        /// </summary>
        /// <param name="id">EmployeeContact id</param>
        /// <returns>EmployeeContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeContact")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult GetEmployeeContact(int id)
        {
            EmployeeContact employeeContact = new EmployeeContact();
            
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeContact = db.EmployeeContacts.Find(id);
                    if (employeeContact != null)
                    {
                        db.Entry(employeeContact).Reference(d => d.ContactType).Load();
                    }      
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            if (employeeContact == null)
            {
                return NotFound();
            }

            EmployeeContactDTO dto = employeeContact.ToDTO();

            return Ok(dto);
        }

        /// <summary>
        /// Update employee contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="employeeContact">EmployeeContact object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeContact(int id, EmployeeContact employeeContact)
        {
            if (employeeContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeContact.Id)
            {
                return BadRequest();
            }
            try
            {
                using (DbMain db = new DbMain(User))
                {
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
                    employeeContact = db.EmployeeContacts.Find(employeeContact.Id);
                    employeeContact.ContactType = db.ContactTypes.Find(employeeContact.ContactTypeId);
                 }

            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }
            EmployeeContactDTO dto = employeeContact.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Add employee contact 
        /// </summary>
        /// <param name="employeeContact">EmployeeContact object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult PostEmployeeContact(EmployeeContact employeeContact)
        {
            if (employeeContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeContact.Id = employeeContact.NewId(db);
                    db.EmployeeContacts.Add(employeeContact);
                    db.SaveChanges();
                    employeeContact.ContactType = db.ContactTypes.Find(employeeContact.ContactTypeId);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            EmployeeContactDTO dto = employeeContact.ToDTO();
            return CreatedAtRoute("GetEmployeeContact", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeContact))]
        public IHttpActionResult DeleteEmployeeContact(int id)
        {
            EmployeeContact employeeContact = new EmployeeContact();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeContact = db.EmployeeContacts.Find(id);
                    if (employeeContact == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employeeContact).Reference(d => d.ContactType).Load();

                    employeeContact.Deleted = true;
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
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            EmployeeContactDTO dto = employeeContact.ToDTO();
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

        private bool EmployeeContactExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeContacts.Count(e => e.Id == id) > 0;
            }
        }
    }
}