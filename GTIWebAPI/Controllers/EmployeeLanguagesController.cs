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
        [ResponseType(typeof(IEnumerable<EmployeeLanguageDTO>))]
        public IHttpActionResult GetEmployeeLanguageAll()
        {
            IEnumerable<EmployeeLanguageDTO> dtos = new List<EmployeeLanguageDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeLanguages.Where(p => p.Deleted != true).Include(d => d.EmployeeLanguageType).Include(d => d.Language).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeLanguageDTO>))]
        public IHttpActionResult GetEmployeeLanguageByEmployee(int employeeId)
        {
            IEnumerable<EmployeeLanguageDTO> dtos = new List<EmployeeLanguageDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeLanguages.Where(p => p.Deleted != true && p.EmployeeId == employeeId).Include(d => d.EmployeeLanguageType).Include(d => d.Language).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
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
            if (employeeLanguage == null || !ModelState.IsValid || id != employeeLanguage.Id)
            {
                return BadRequest(ModelState);
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