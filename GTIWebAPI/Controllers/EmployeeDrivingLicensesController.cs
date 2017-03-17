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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Driving license is a document employee needs to drive a car 
    /// </summary>
    [RoutePrefix("api/EmployeeDrivingLicenses")]
    public class EmployeeDrivingLicensesController : ApiController
    {
        private IRepository<EmployeeDrivingLicense> repo;

        public EmployeeDrivingLicensesController()
        {
            repo = new EmployeeDrivingLicensesRepository();
        }

        public EmployeeDrivingLicensesController(IRepository<EmployeeDrivingLicense> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<EmployeeDrivingLicenseDTO>))]
        public IHttpActionResult GetEmployeeDrivingLicenseAll()
        {
            try
            {
                List<EmployeeDrivingLicenseDTO> licenses =
                    repo.GetAll()
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(licenses);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(List<EmployeeDrivingLicenseDTO>))]
        public IHttpActionResult GetEmployeeDrivingLicenseByEmployee(int employeeId)
        {
            try
            {
                List<EmployeeDrivingLicenseDTO> licenses =
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(licenses);
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
                EmployeeDrivingLicenseDTO employeeDrivingLicense = repo.Get(id).ToDTO();
                return Ok(employeeDrivingLicense);
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
                EmployeeDrivingLicenseDTO dto = repo.Edit(employeeDrivingLicense).ToDTO();
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
                EmployeeDrivingLicenseDTO dto = repo.Add(employeeDrivingLicense).ToDTO();
                return CreatedAtRoute("GetDrivingLicense", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
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
                EmployeeDrivingLicenseDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}