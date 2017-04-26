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
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Languages employee knows anf documents that confirm that fact 
    /// </summary>
    [RoutePrefix("api/EmployeeLanguages")]
    public class EmployeeLanguagesController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeLanguagesController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeLanguagesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeLanguageDTO>))]
        public IHttpActionResult GetEmployeeLanguageAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeLanguageDTO> dtos = unitOfWork.EmployeeLanguagesRepository.Get(d => d.Deleted != true, includeProperties: "Language,EmployeeLanguageType").Select(d => d.ToDTO());
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
        [ResponseType(typeof(IEnumerable<EmployeeLanguageDTO>))]
        public IHttpActionResult GetEmployeeLanguageByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeLanguageDTO> dtos = unitOfWork.EmployeeLanguagesRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId, includeProperties: "Language,EmployeeLanguageType").Select(d => d.ToDTO());
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
        [Route("Get", Name = "GetEmployeeLanguage")]
        [ResponseType(typeof(EmployeeLanguageDTO))]
        public IHttpActionResult GetEmployeeLanguage(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeLanguageDTO dto = unitOfWork.EmployeeLanguagesRepository.Get(d => d.Id == id, includeProperties: "Language,EmployeeLanguageType").FirstOrDefault().ToDTO();
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
        public IHttpActionResult PutEmployeeLanguage(int id, EmployeeLanguage employeeLanguage)
        {
            if (employeeLanguage == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeLanguage.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeLanguagesRepository.Update(employeeLanguage);
                unitOfWork.Save();
                EmployeeLanguageDTO dto = unitOfWork.EmployeeLanguagesRepository.Get(d => d.Id == id, includeProperties: "Language,EmployeeLanguageType").FirstOrDefault().ToDTO();
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
        [ResponseType(typeof(EmployeeLanguageDTO))]
        public IHttpActionResult PostEmployeeLanguage(EmployeeLanguage employeeLanguage)
        {
            if (employeeLanguage == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeLanguage.Id = employeeLanguage.NewId(unitOfWork);
                unitOfWork.EmployeeLanguagesRepository.Insert(employeeLanguage);
                unitOfWork.Save();
                EmployeeLanguageDTO dto = unitOfWork.EmployeeLanguagesRepository.Get(d => d.Id == employeeLanguage.Id, includeProperties: "Language,EmployeeLanguageType").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetEmployeeLanguage", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeeLanguage))]
        public IHttpActionResult DeleteEmployeeLanguage(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeLanguage language = unitOfWork.EmployeeLanguagesRepository.Get(d => d.Id == id, includeProperties: "Language,EmployeeLanguageType").FirstOrDefault();
                language.Deleted = true;
                unitOfWork.EmployeeLanguagesRepository.Update(language);
                unitOfWork.Save();
                EmployeeLanguageDTO dto = language.ToDTO();
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