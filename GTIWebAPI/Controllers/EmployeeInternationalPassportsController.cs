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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/EmployeeInternationalPassports")]
    public class EmployeeInternationalPassportsController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeInternationalPassportsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeInternationalPassportsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeInternationalPassportDTO>))]
        public IHttpActionResult GetEmployeeInternationalPassportAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeInternationalPassportDTO> dtos = unitOfWork.EmployeeInternationalPassportsRepository.Get(d => d.Deleted != true).Select(d => d.ToDTO());
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
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeInternationalPassportDTO>))]
        public IHttpActionResult GetEmployeeInternationalPassportByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeInternationalPassportDTO> dtos = unitOfWork.EmployeeInternationalPassportsRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId).Select(d => d.ToDTO());
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
        [Route("Get", Name = "GetEmployeeInternationalPassport")]
        [ResponseType(typeof(EmployeeInternationalPassportDTO))]
        public IHttpActionResult GetEmployeeInternationalPassport(int id)
        {
           try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeInternationalPassportDTO internationalPassport = unitOfWork.EmployeeInternationalPassportsRepository.GetByID(id).ToDTO();
                return Ok(internationalPassport);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeInternationalPassport(int id, EmployeeInternationalPassport employeeInternationalPassport)
        {
            if (employeeInternationalPassport == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeInternationalPassport.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeInternationalPassportsRepository.Update(employeeInternationalPassport);
                unitOfWork.Save();
                EmployeeInternationalPassportDTO passport = employeeInternationalPassport.ToDTO();
                return Ok(passport);
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
        [ResponseType(typeof(EmployeeInternationalPassportDTO))]
        public IHttpActionResult PostEmployeeInternationalPassport(EmployeeInternationalPassport employeeInternationalPassport)
        {
            if (employeeInternationalPassport == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeInternationalPassport.Id = employeeInternationalPassport.NewId(unitOfWork);
                unitOfWork.EmployeeInternationalPassportsRepository.Insert(employeeInternationalPassport);
                unitOfWork.Save();
                EmployeeInternationalPassportDTO dto = employeeInternationalPassport.ToDTO();
                return CreatedAtRoute("GetEmployeeInternationalPassport", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeeInternationalPassport))]
        public IHttpActionResult DeleteEmployeeInternationalPassport(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeInternationalPassport passport = unitOfWork.EmployeeInternationalPassportsRepository.GetByID(id);
                passport.Deleted = true;
                unitOfWork.EmployeeInternationalPassportsRepository.Update(passport);
                unitOfWork.Save();
                EmployeeInternationalPassportDTO dto = passport.ToDTO();
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