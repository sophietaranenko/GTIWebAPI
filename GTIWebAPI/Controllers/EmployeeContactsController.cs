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
        public IEnumerable<EmployeeContactDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
            m.CreateMap<EmployeeContact, EmployeeContactDTO>();
            m.CreateMap<ContactType, ContactTypeDTO>();
            });
            IEnumerable<EmployeeContactDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeContact>, IEnumerable<EmployeeContactDTO>>
                (db.EmployeeContact.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee contact by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeContactDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactsByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IEnumerable<EmployeeContactDTO> GetByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            IEnumerable<EmployeeContactDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeContact>, IEnumerable<EmployeeContactDTO>>
                (db.EmployeeContact.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one contact for view by contact id
        /// </summary>
        /// <param name="id">EmployeeContact id</param>
        /// <returns>EmployeeContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactView", Name = "GetContactView")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult GetContactView(int id)
        {
            EmployeeContact contact = db.EmployeeContact.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            EmployeeContactDTO dto = Mapper.Map<EmployeeContactDTO>(contact);
            return Ok(dto);
        }

        /// <summary>
        /// Get one contact for edit by contact id
        /// </summary>
        /// <param name="id">EmployeeContact id</param>
        /// <returns>EmployeeContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactEdit")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult GetContactEdit(int id)
        {
            EmployeeContact contact = db.EmployeeContact.Find(id);
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
        [Route("PutContact")]
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
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Insert new employee contact
        /// </summary>
        /// <param name="employeeContact">EmployeeContact object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostContact")]
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            EmployeeContactDTO dto = Mapper.Map<EmployeeContact, EmployeeContactDTO>(employeeContact);
            return CreatedAtRoute("GetContactView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteContact")]
        [ResponseType(typeof(EmployeeContact))]
        public IHttpActionResult DeleteEmployeeContact(int id)
        {
            EmployeeContact employeeContact = db.EmployeeContact.Find(id);
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
        /// Types of contact 
        /// </summary>
        /// <returns></returns>
        [Route("GetContactTypes")]
        [HttpGet]
        public IEnumerable<ContactTypeDTO> GetContactTypes()
        {
            List<ContactType> types = db.ContactType.Where(c => c.Deleted != true).ToList();
            Mapper.Initialize(m => m.CreateMap<ContactType, ContactTypeDTO>());
            IEnumerable<ContactTypeDTO> dtos = Mapper.Map<IEnumerable<ContactType>, IEnumerable<ContactTypeDTO>>(types);
            return dtos;
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
            return db.EmployeeContact.Count(e => e.Id == id) > 0;
        }
    }
}