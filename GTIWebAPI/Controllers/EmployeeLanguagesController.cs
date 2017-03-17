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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Languages employee knows anf documents that confirm that fact 
    /// </summary>
    [RoutePrefix("api/EmployeeLanguages")]
    public class EmployeeLanguagesController : ApiController
    {
        private IRepository<EmployeeLanguage> repo;

        public EmployeeLanguagesController()
        {
            repo = new EmployeeLanguagesRepository();
        }

        public EmployeeLanguagesController(IRepository<EmployeeLanguage> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<EmployeeLanguageDTO>))]
        public IHttpActionResult GetEmployeeLanguageAll()
        {
            try
            {
                List<EmployeeLanguageDTO> dtos = repo.GetAll()
                    .Select(d => d.ToDTO())
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
        [ResponseType(typeof(List<EmployeeLanguageDTO>))]
        public IHttpActionResult GetEmployeeLanguageByEmployee(int employeeId)
        {
            try {
                List<EmployeeLanguageDTO> dtos = 
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
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
        [Route("Get", Name = "GetEmployeeLanguage")]
        [ResponseType(typeof(EmployeeLanguageDTO))]
        public IHttpActionResult GetEmployeeLanguage(int id)
        {
            try
            {
                EmployeeLanguageDTO dto = repo.Get(id).ToDTO();
                return Ok(dto); 
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
                EmployeeLanguageDTO dto = repo.Edit(employeeLanguage).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                EmployeeLanguageDTO dto = repo.Add(employeeLanguage).ToDTO();
                return CreatedAtRoute("GetEmployeeLanguage", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                EmployeeLanguageDTO dto = repo.Delete(id).ToDTO();
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