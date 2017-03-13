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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee educations
    /// </summary>
    [RoutePrefix("api/EmployeeEducations")]
    public class EmployeeEducationsController : ApiController
    {

        /// <summary>
        /// Get all education documents
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeEducationDTO>))]
        public IHttpActionResult GetEmployeeEducation()
        {
            IEnumerable<EmployeeEducationDTO> dtos = new List<EmployeeEducationDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeEducations.Where(e => e.Deleted != true).Include(d => d.EducationStudyForm).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get education documents of current employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeEducationDTO>))]
        public IHttpActionResult GetEmployeeEducationByEmployeeId(int employeeId)
        {
            IEnumerable<EmployeeEducationDTO> dtos = null;

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeEducations.Where(e => e.Deleted != true && e.EmployeeId == employeeId).Include(d => d.EducationStudyForm).ToList()
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
        /// Get education document View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeEducation")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult GetEmployeeEducationView(int id)
        {
            EmployeeEducation employeeEducation = new EmployeeEducation();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeEducation = db.EmployeeEducations.Find(id);
                    if (employeeEducation != null)
                    {
                        db.Entry(employeeEducation).Reference(d => d.EducationStudyForm).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (employeeEducation == null)
            {
                return NotFound();
            }

            EmployeeEducationDTO dto = employeeEducation.ToDTO();

            return Ok(dto);
        }


        /// <summary>
        /// Update EmployeeEducation
        /// </summary>
        /// <param name="id">id of document</param>
        /// <param name="employeeEducation">EmployeeEducation document</param>
        /// <returns></returns>
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
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    db.Entry(employeeEducation).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeEducationExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (employeeEducation.StudyFormId != null)
                    {
                        employeeEducation.EducationStudyForm = db.EducationStudyForms.Find(employeeEducation.StudyFormId);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeEducationDTO dto = employeeEducation.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee education document
        /// </summary>
        /// <param name="employeeEducation"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult PostEmployeeEducation(EmployeeEducation employeeEducation)
        {
            EmployeeEducationDTO dto = null;
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeEducation.Id = employeeEducation.NewId(db);
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    db.EmployeeEducations.Add(employeeEducation);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeEducationExists(employeeEducation.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    int Id = employeeEducation.Id;
                    EmployeeEducation education = db.EmployeeEducations.Find(Id);
                    if (education.StudyFormId != null)
                    {
                        education.EducationStudyForm = db.EducationStudyForms.Find(education.StudyFormId);
                    }
                    dto = education.ToDTO();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetEmployeeEducation", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Dleete education document
        /// </summary>
        /// <param name="id">id of education document</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult DeleteEmployeeEducation(int id)
        {
            EmployeeEducation employeeEducation = null;
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeEducation = db.EmployeeEducations.Find(id);
                    if (employeeEducation == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employeeEducation).Reference(d => d.EducationStudyForm).Load();

                    employeeEducation.Deleted = true;
                    db.Entry(employeeEducation).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeEducationExists(id))
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

            EmployeeEducationDTO dto = employeeEducation.ToDTO();
            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeEducationExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.EmployeeEducations.Count(e => e.Id == id) > 0;
            }
        }
    }
}