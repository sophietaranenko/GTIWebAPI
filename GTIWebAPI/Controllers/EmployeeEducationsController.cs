using System;
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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/EmployeeEducations")]
    public class EmployeeEducationsController : ApiController
    {
        private IRepository<EmployeeEducation> repo;

        public EmployeeEducationsController()
        {
            repo = new EmployeeEducationsRepository();
        }

        public EmployeeEducationsController(IRepository<EmployeeEducation> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<EmployeeEducationDTO>))]
        public IHttpActionResult GetEmployeeEducation()
        {
            try
            {
                List<EmployeeEducationDTO> dtos =
                     repo.GetAll()
                     .Select(d => d.ToDTO())
                     .ToList();
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
        [ResponseType(typeof(List<EmployeeEducationDTO>))]
        public IHttpActionResult GetEmployeeEducationByEmployeeId(int employeeId)
        {
            try
            {
                List<EmployeeEducationDTO> dtos =
                repo.GetByEmployeeId(employeeId)
                .Select(d => d.ToDTO())
                .ToList();
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
        [Route("Get", Name = "GetEmployeeEducation")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult GetEmployeeEducationView(int id)
        {
            try
            {
                EmployeeEducationDTO employeeEducation = repo.Get(id).ToDTO();
                return Ok(employeeEducation);
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
        public IHttpActionResult PutEmployeeEducation(int id, EmployeeEducation employeeEducation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeEducation.Id)
            {
                return BadRequest();
            }
            try
            {
                EmployeeEducationDTO dto = repo.Edit(employeeEducation).ToDTO();
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
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult PostEmployeeEducation(EmployeeEducation employeeEducation)
        {
            try
            {
                EmployeeEducationDTO dto = repo.Add(employeeEducation).ToDTO();
                return CreatedAtRoute("GetEmployeeEducation", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult DeleteEmployeeEducation(int id)
        {
            EmployeeEducation employeeEducation = null;
            try
            {
                EmployeeEducationDTO dto = repo.Delete(id).ToDTO();
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