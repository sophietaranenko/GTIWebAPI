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
using GTIWebAPI.Filters;
using AutoMapper;
using GTIWebAPI.Models.Personnel;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for contacts
    /// </summary>
    [RoutePrefix("api/EmployeeContacts")]
    public class EmployeeContactsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All contacts
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeContactDTO> GetEmployeeContactAll()
        {
            Mapper.Initialize(m =>
            {
            m.CreateMap<EmployeeContact, EmployeeContactDTO>();
            m.CreateMap<ContactType, ContactTypeDTO>();
            });
            IEnumerable<EmployeeContactDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeContact>, IEnumerable<EmployeeContactDTO>>
                (db.EmployeeContacts.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee contact by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeContactDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IEnumerable<EmployeeContactDTO> GetEmployeeContactByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            IEnumerable<EmployeeContactDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeContact>, IEnumerable<EmployeeContactDTO>>
                (db.EmployeeContacts.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
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
            EmployeeContact contact = db.EmployeeContacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            EmployeeContactDTO dto = Mapper.Map<EmployeeContact, EmployeeContactDTO>(contact);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            EmployeeContactDTO dto = Mapper.Map<EmployeeContactDTO>(employeeContact);
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee contact
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

            employeeContact.Id = employeeContact.NewId(db);
            db.EmployeeContacts.Add(employeeContact);

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
            employeeContact.ContactType = db.ContactTypes.Find(employeeContact.ContactTypeId);
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            EmployeeContactDTO dto = Mapper.Map<EmployeeContactDTO>(employeeContact);
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
            EmployeeContact employeeContact = db.EmployeeContacts.Find(id);
            if (employeeContact == null)
            {
                return NotFound();
            }
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            EmployeeContactDTO dto = Mapper.Map<EmployeeContact, EmployeeContactDTO>(employeeContact);
            return Ok(dto);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
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
            return db.EmployeeContacts.Count(e => e.Id == id) > 0;
        }
    }
}