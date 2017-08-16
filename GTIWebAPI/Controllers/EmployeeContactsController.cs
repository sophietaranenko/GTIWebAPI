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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Employee contacts (facebook, skype, home phone number etc.) 
    /// </summary>
    [RoutePrefix("api/EmployeeContacts")]
    public class EmployeeContactsController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeContactsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeContactsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IHttpActionResult GetEmployeeContactAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeContactDTO> dtos =
                    unitOfWork.EmployeeContactsRepository.Get(d => d.Deleted != true, includeProperties: "ContactType").Select(d => d.ToDTO());
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IHttpActionResult GetEmployeeContactByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeContactDTO> dtos = 
                    unitOfWork.EmployeeContactsRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == employeeId, includeProperties: "ContactType")
                    .Select(d => d.ToDTO()); 
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeContact")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult GetEmployeeContact(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeContactDTO employeeContact = unitOfWork.EmployeeContactsRepository.Get(d => d.Id == id, includeProperties: "ContactType").FirstOrDefault().ToDTO();
                return Ok(employeeContact);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeContact(int id, EmployeeContactDTO employeeContact)
        {
            if (employeeContact == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeContact.Id)
            {
                return BadRequest();
            }
            try
            {
                EmployeeContact contact = employeeContact.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeContactsRepository.Update(contact);
                unitOfWork.Save();
                EmployeeContactDTO dto = unitOfWork.EmployeeContactsRepository.Get(d => d.Id == id, includeProperties: "ContactType").FirstOrDefault().ToDTO();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult PostEmployeeContact(EmployeeContactDTO employeeContact)
        {
            if (employeeContact == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeContact contact = employeeContact.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                contact.Id = contact.NewId(unitOfWork);
                unitOfWork.EmployeeContactsRepository.Insert(contact);
                unitOfWork.Save();

                EmployeeContactDTO dto = unitOfWork.EmployeeContactsRepository.Get(d => d.Id == contact.Id, includeProperties: "ContactType").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetEmployeeContact", new { id = dto.Id }, dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeContactDTO))]
        public IHttpActionResult DeleteEmployeeContact(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeContact employeeContact = unitOfWork.EmployeeContactsRepository.Get(d => d.Id == id, includeProperties: "ContactType").FirstOrDefault();
                employeeContact.Deleted = true;
                unitOfWork.EmployeeContactsRepository.Update(employeeContact);
                unitOfWork.Save();
                EmployeeContactDTO dto = employeeContact.ToDTO();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}