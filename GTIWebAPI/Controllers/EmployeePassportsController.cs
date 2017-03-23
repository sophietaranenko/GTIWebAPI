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
using AutoMapper;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee passports
    /// </summary>
    [RoutePrefix("api/EmployeePassports")]
    public class EmployeePassportsController : ApiController
    {
        private IRepository<EmployeePassport> repo;

        public EmployeePassportsController()
        {
            repo = new EmployeePassportsRepository();
        }

        public EmployeePassportsController(IRepository<EmployeePassport> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<EmployeePassportDTO>))]
        public IHttpActionResult GetEmployeePassportAll()
        {
            try
            {
                List<EmployeePassportDTO> passports = 
                    repo.GetAll()
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(passports);
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
        [ResponseType(typeof(List<EmployeePassportDTO>))]
        public IHttpActionResult GetEmployeePassportByEmployee(int employeeId)
        {
            try
            {
                List<EmployeePassportDTO> passports = 
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(passports);
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
        [Route("Get", Name = "GetEmployeePassport")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult GetEmployeePassport(int id)
        {
            try
            {
                EmployeePassportDTO employeePassport =
                    repo.Get(id)
                    .ToDTO();
                return Ok(employeePassport);
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
        public IHttpActionResult PutEmployeePassport(int id, EmployeePassport employeePassport)
        {
            if (employeePassport == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeePassport.Id)
            {
                return BadRequest();
            }
            try
            {
                EmployeePassportDTO passport =
                    repo.Edit(employeePassport)
                    .ToDTO();
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
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult PostEmployeePassport(EmployeePassport employeePassport)
        {
            if (employeePassport == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                EmployeePassportDTO dto =
                    repo.Add(employeePassport)
                    .ToDTO();
                return CreatedAtRoute("GetEmployeePassport", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeePassport))]
        public IHttpActionResult DeleteEmployeePassport(int id)
        {
            try
            {
                EmployeePassportDTO employeePassport =
                     repo.Delete(id)
                     .ToDTO();
                return Ok(employeePassport);
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