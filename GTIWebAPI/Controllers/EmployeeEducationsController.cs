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
        private IDbContextFactory factory;

        public EmployeeEducationsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeEducationsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeEducationDTO>))]
        public IHttpActionResult GetEmployeeEducation()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeEducationDTO> dtos = unitOfWork.EmployeeEducationsRepository.Get(d => d.Deleted != true, includeProperties: "EducationStudyForm").Select(d => d.ToDTO());
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
        [ResponseType(typeof(IEnumerable<EmployeeEducationDTO>))]
        public IHttpActionResult GetEmployeeEducationByEmployeeId(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeEducationDTO> dtos = unitOfWork.EmployeeEducationsRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId, includeProperties: "EducationStudyForm").Select(d => d.ToDTO());
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeEducationDTO employeeEducation = unitOfWork.EmployeeEducationsRepository.Get(d => d.Id == id, includeProperties: "EducationStudyForm").FirstOrDefault().ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeEducationsRepository.Update(employeeEducation);
                unitOfWork.Save();
                EmployeeEducationDTO dto = unitOfWork.EmployeeEducationsRepository.Get(d => d.Id == id, includeProperties: "EducationStudyForm").FirstOrDefault().ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeEducation.Id = employeeEducation.NewId(unitOfWork);
                unitOfWork.EmployeeEducationsRepository.Insert(employeeEducation);
                unitOfWork.Save();
                EmployeeEducationDTO dto = unitOfWork.EmployeeEducationsRepository.Get(d => d.Id == employeeEducation.Id, includeProperties: "EducationStudyForm").FirstOrDefault().ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeEducation education = unitOfWork.EmployeeEducationsRepository.Get(d => d.Id == id, includeProperties: "EducationStudyForm").FirstOrDefault();
                education.Deleted = true;
                unitOfWork.EmployeeEducationsRepository.Update(education);
                unitOfWork.Save();
                EmployeeEducationDTO dto = education.ToDTO();
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