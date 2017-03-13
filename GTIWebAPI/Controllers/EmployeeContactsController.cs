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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee contacts (facebook, skype, home phone number etc. and their values) 
    /// </summary>
    [RoutePrefix("api/EmployeeContacts")]
    public class EmployeeContactsController : ApiController
    {
        private IEmployeeContactsRepository repo;

        public EmployeeContactsController()
        {
            repo = new EmployeeContactsRepository();
        }

        public EmployeeContactsController(IEmployeeContactsRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeContactDTO>))]
        public IHttpActionResult GetEmployeeContactAll()
        {
            try
            {
                IEnumerable<EmployeeContactDTO> dtos =
                    repo.GetAll()
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
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
                IEnumerable<EmployeeContactDTO> dtos =
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
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
                EmployeeContactDTO employeeContact = repo.Get(id).ToDTO();
                return Ok(employeeContact);
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
        public IHttpActionResult PutEmployeeContact(int id, EmployeeContact employeeContact)
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
                EmployeeContactDTO dto = repo.Edit(employeeContact).ToDTO();
                return Ok(dto);
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
        public IHttpActionResult PostEmployeeContact(EmployeeContact employeeContact)
        {
            if (employeeContact == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeContactDTO dto = repo.Add(employeeContact).ToDTO();
                return CreatedAtRoute("GetEmployeeContact", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeContact))]
        public IHttpActionResult DeleteEmployeeContact(int id)
        {
            try
            {
                EmployeeContactDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
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