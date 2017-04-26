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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Driving license is a document employee needs to drive a car 
    /// </summary>
    [RoutePrefix("api/EmployeeDrivingLicenses")]
    public class EmployeeDrivingLicensesController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeDrivingLicensesController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeDrivingLicensesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeDrivingLicenseDTO>))]
        public IHttpActionResult GetEmployeeDrivingLicenseAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeDrivingLicenseDTO> licenses = unitOfWork.EmployeeDrivingLicensesRepository.Get(d => d.Deleted != true).Select(d => d.ToDTO());
                return Ok(licenses);
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
        [ResponseType(typeof(IEnumerable<EmployeeDrivingLicenseDTO>))]
        public IHttpActionResult GetEmployeeDrivingLicenseByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeDrivingLicenseDTO> licenses = unitOfWork.EmployeeDrivingLicensesRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId).Select(d => d.ToDTO());
                return Ok(licenses);
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
        [Route("Get", Name = "GetDrivingLicense")]
        [ResponseType(typeof(EmployeeDrivingLicenseDTO))]
        public IHttpActionResult GetDrivingLicense(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeDrivingLicenseDTO employeeDrivingLicense = unitOfWork.EmployeeDrivingLicensesRepository.GetByID(id).ToDTO();
                return Ok(employeeDrivingLicense);
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
        public IHttpActionResult PutEmployeeDrivingLicense(int id, EmployeeDrivingLicense employeeDrivingLicense)
        {
            if (employeeDrivingLicense == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeDrivingLicense.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeDrivingLicensesRepository.Update(employeeDrivingLicense);
                unitOfWork.Save();
                EmployeeDrivingLicenseDTO dto = unitOfWork.EmployeeDrivingLicensesRepository.GetByID(id).ToDTO();
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
        [ResponseType(typeof(EmployeeDrivingLicenseDTO))]
        public IHttpActionResult PostEmployeeDrivingLicense(EmployeeDrivingLicense employeeDrivingLicense)
        {
            if (employeeDrivingLicense == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeDrivingLicense.Id = employeeDrivingLicense.NewId(unitOfWork);
                unitOfWork.EmployeeDrivingLicensesRepository.Insert(employeeDrivingLicense);
                unitOfWork.Save();
                EmployeeDrivingLicenseDTO dto = employeeDrivingLicense.ToDTO();
                return CreatedAtRoute("GetDrivingLicense", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeeDrivingLicense))]
        public IHttpActionResult DeleteEmployeeDrivingLicense(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeDrivingLicense license = unitOfWork.EmployeeDrivingLicensesRepository.GetByID(id);
                license.Deleted = true;
                unitOfWork.EmployeeDrivingLicensesRepository.Update(license);
                unitOfWork.Save();
                EmployeeDrivingLicenseDTO dto = license.ToDTO();
                return Ok(dto);
            }
            catch (NullReferenceException nle)
            {
                return NotFound();
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