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

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/EmployeeInternationalPassports")]
    public class EmployeeInternationalPassportsController : ApiController
    {
        private IRepository<EmployeeInternationalPassport> repo;

        public EmployeeInternationalPassportsController()
        {
            repo = new EmployeeInternationalPassportsRepository();
        }

        public EmployeeInternationalPassportsController(IRepository<EmployeeInternationalPassport> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<EmployeeInternationalPassportDTO>))]
        public IHttpActionResult GetEmployeeInternationalPassportAll()
        {
            try
            {
                List<EmployeeInternationalPassportDTO> dtos =
                    repo.GetAll()
                    .Select(e => e.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(List<EmployeeInternationalPassportDTO>))]
        public IHttpActionResult GetEmployeeInternationalPassportByEmployee(int employeeId)
        {
            try
            {
                List<EmployeeInternationalPassportDTO> dtos =
                    repo.GetByEmployeeId(employeeId)
                    .Select(e => e.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                EmployeeInternationalPassportDTO internationalPassport = repo.Get(id).ToDTO();
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
                EmployeeInternationalPassportDTO passport = repo.Edit(employeeInternationalPassport).ToDTO();
                return Ok(passport);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                EmployeeInternationalPassportDTO dto = repo.Add(employeeInternationalPassport).ToDTO();
                return CreatedAtRoute("GetEmployeeInternationalPassport", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                EmployeeInternationalPassportDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}