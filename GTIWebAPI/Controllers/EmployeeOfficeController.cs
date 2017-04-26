using AutoMapper;
using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Employee's positions in departments of offices 
    /// </summary>
    [RoutePrefix("api/EmployeeOffices")]
    public class EmployeeOfficesController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeOfficesController()
        {
            this.factory = new DbContextFactory();
        }

        public EmployeeOfficesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeOfficeDTO>))]
        public IHttpActionResult GetEmployeeOfficeAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeOfficeDTO> list = unitOfWork.EmployeeOfficesRepository.Get(d => d.Deleted != true,includeProperties: "Office,Department,Profession").Select(d => d.ToDTO());
                return Ok(list);
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
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeOfficeDTO>))]
        public IHttpActionResult GetEmployeeOfficeByEmployeeId(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeOfficeDTO> dtos = unitOfWork.EmployeeOfficesRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId, includeProperties: "Office,Department,Profession").Select(d => d.ToDTO());
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeOffice")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult GetEmployeeOffice(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeOfficeDTO dto = unitOfWork.EmployeeOfficesRepository.Get(d => d.Id == id, includeProperties: "Office,Department,Profession").FirstOrDefault().ToDTO();
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

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeOffice(int id, EmployeeOffice employeeOffice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeOffice.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeOfficesRepository.Update(employeeOffice);
                unitOfWork.Save();
                EmployeeOfficeDTO dto = unitOfWork.EmployeeOfficesRepository.Get(d => d.Id == id, includeProperties: "Office,Department,Profession").FirstOrDefault().ToDTO();
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

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult PostEmployeeOffice(EmployeeOffice employeeOffice)
        {
            if (employeeOffice == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeOffice.Id = employeeOffice.NewId(unitOfWork);
                unitOfWork.EmployeeOfficesRepository.Insert(employeeOffice);
                unitOfWork.Save();
                EmployeeOfficeDTO dto = unitOfWork.EmployeeOfficesRepository.Get(d => d.Id == employeeOffice.Id, includeProperties: "Office,Department,Profession").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetEmployeeOffice", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult DeleteEmployeeOffice(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeOffice office = unitOfWork.EmployeeOfficesRepository.Get(d => d.Id == id, includeProperties: "Office,Department,Profession").FirstOrDefault();
                office.Deleted = true;
                unitOfWork.EmployeeOfficesRepository.Update(office);
                unitOfWork.Save();
                EmployeeOfficeDTO dto = office.ToDTO();
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
