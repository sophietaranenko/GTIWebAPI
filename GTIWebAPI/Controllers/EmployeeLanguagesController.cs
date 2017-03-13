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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Languages employee knows anf documents that confirm that fact 
    /// </summary>
    [RoutePrefix("api/EmployeeLanguages")]
    public class EmployeeLanguagesController : ApiController
    {
        //private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All languages
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get employee language by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeLanguageDTO</returns>
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

        /// <summary>
        /// Get one language for view by language id
        /// </summary>
        /// <param name="id">EmployeeLanguage id</param>
        /// <returns>EmployeeLanguageEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeLanguage")]
        [ResponseType(typeof(EmployeeLanguageDTO))]
        public IHttpActionResult GetEmployeeLanguage(int id)
        {
            EmployeeLanguage employeeLanguage = new EmployeeLanguage();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeLanguage = db.EmployeeLanguages.Find(id);
                    if (employeeLanguage != null)
                    {
                        db.Entry(employeeLanguage).Reference(d => d.EmployeeLanguageType).Load();
                        db.Entry(employeeLanguage).Reference(d => d.Language).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (employeeLanguage == null)
            {
                return NotFound();
            }

            EmployeeLanguageDTO dto = employeeLanguage.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee language
        /// </summary>
        /// <param name="id">Language id</param>
        /// <param name="employeeLanguage">EmployeeLanguage object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeLanguage(int id, EmployeeLanguage employeeLanguage)
        {
            if (employeeLanguage == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeLanguage.Id)
            {
                return BadRequest();
            }

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    db.Entry(employeeLanguage).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeLanguageExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (employeeLanguage.LanguageId != null)
                    {
                        employeeLanguage.Language = db.Languages.Find(employeeLanguage.LanguageId);
                    }
                    if (employeeLanguage.EmployeeLanguageTypeId != null)
                    {
                        employeeLanguage.EmployeeLanguageType = db.EmployeeLanguageTypes.Find(employeeLanguage.EmployeeLanguageTypeId);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeLanguageDTO dto = employeeLanguage.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee language
        /// </summary>
        /// <param name="employeeLanguage">EmployeeLanguage object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeLanguageDTO))]
        public IHttpActionResult PostEmployeeLanguage(EmployeeLanguage employeeLanguage)
        {
            if (employeeLanguage == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeLanguage.Id = employeeLanguage.NewId(db);
                    db.EmployeeLanguages.Add(employeeLanguage);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeLanguageExists(employeeLanguage.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (employeeLanguage.LanguageId != null)
                    {
                        employeeLanguage.Language = db.Languages.Find(employeeLanguage.LanguageId);
                    }
                    if (employeeLanguage.EmployeeLanguageTypeId != null)
                    {
                        employeeLanguage.EmployeeLanguageType = db.EmployeeLanguageTypes.Find(employeeLanguage.EmployeeLanguageTypeId);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeLanguageDTO dto = employeeLanguage.ToDTO();
            return CreatedAtRoute("GetEmployeeLanguage", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete language
        /// </summary>
        /// <param name="id">Language Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeLanguage))]
        public IHttpActionResult DeleteEmployeeLanguage(int id)
        {
            EmployeeLanguage employeeLanguage = new EmployeeLanguage();
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeLanguage = db.EmployeeLanguages.Find(id);
                    if (employeeLanguage == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employeeLanguage).Reference(d => d.EmployeeLanguageType).Load();
                    db.Entry(employeeLanguage).Reference(d => d.Language).Load();

                    employeeLanguage.Deleted = true;
                    db.Entry(employeeLanguage).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeLanguageExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeLanguageDTO dto = employeeLanguage.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeLanguageExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.EmployeeLanguages.Count(e => e.Id == id) > 0;
            }
        }
    }
}