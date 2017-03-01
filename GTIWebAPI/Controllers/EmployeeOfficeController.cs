using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Employee's positions in departments of offices 
    /// </summary>
    [RoutePrefix("api/EmployeeOffices")]
    public class EmployeeOfficeController : ApiController
    {

        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeOfficeDTO>))]
        public IHttpActionResult GetEmployeeOfficeAll()
        {
            IEnumerable<EmployeeOfficeDTO> dtos = new List<EmployeeOfficeDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeOffices.Where(e => e.Deleted != true)
                        .Include(d => d.Profession)
                        .Include(d => d.Office)
                        .Include(d => d.Department)
                        .ToList()
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
        /// Get positions of current employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeOfficeDTO>))]
        public IHttpActionResult GetEmployeeOfficeByEmployeeId(int employeeId)
        {

            IEnumerable<EmployeeOfficeDTO> dtos = new List<EmployeeOfficeDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeOffices.Where(e => e.Deleted != true && e.EmployeeId == employeeId)
                        .Include(d => d.Profession)
                        .Include(d => d.Office)
                        .Include(d => d.Department)
                        .ToList()
                        .Select(d => d.ToDTO()).ToList();

                    //offices = db.EmployeeOffices.Where(e => e.Deleted != true && e.EmployeeId == employeeId).ToList();

                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get position View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeOffice")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult GetEmployeeOffice(int id)
        {
            EmployeeOffice employeeOffice = new EmployeeOffice();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeOffice = db.EmployeeOffices.Find(id);
                    if (employeeOffice != null)
                    {
                        db.Entry(employeeOffice).Reference(d => d.Department).Load();
                        db.Entry(employeeOffice).Reference(d => d.Office).Load();
                        db.Entry(employeeOffice).Reference(d => d.Profession).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (employeeOffice == null)
            {
                return NotFound();
            }

            EmployeeOfficeDTO dto = employeeOffice.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee position
        /// </summary>
        /// <param name="id">id of position</param>
        /// <param name="employeeOffice">EmployeeOffice</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeOffice(int id, EmployeeOffice employeeOffice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeOffice.Id)
            {
                return BadRequest();
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(employeeOffice).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeOfficeExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    employeeOffice.Department = db.Departments.Find(employeeOffice.DepartmentId);
                    employeeOffice.Office = db.Offices.Find(employeeOffice.OfficeId);
                    employeeOffice.Profession = db.Professions.Find(employeeOffice.ProfessionId);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeOfficeDTO dto = employeeOffice.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee education document
        /// </summary>
        /// <param name="employeeOffice">EmployeeOffice object contains id = 0</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult PostEmployeeOffice(EmployeeOffice employeeOffice)
        {
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeOffice.Id = employeeOffice.NewId(db);
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.EmployeeOffices.Add(employeeOffice);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeOfficeExists(employeeOffice.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    employeeOffice.Department = db.Departments.Find(employeeOffice.DepartmentId);
                    employeeOffice.Office = db.Offices.Find(employeeOffice.OfficeId);
                    employeeOffice.Profession = db.Professions.Find(employeeOffice.ProfessionId);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeOfficeDTO dto = employeeOffice.ToDTO();
            return CreatedAtRoute("GetEmployeeOffice", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Dleete education document
        /// </summary>
        /// <param name="id">id of document</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult DeleteEmployeeOffice(int id)
        {
            EmployeeOffice employeeOffice = new EmployeeOffice();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeOffice = db.EmployeeOffices.Find(id);

                    if (employeeOffice == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employeeOffice).Reference(d => d.Department).Load();
                    db.Entry(employeeOffice).Reference(d => d.Office).Load();
                    db.Entry(employeeOffice).Reference(d => d.Profession).Load();

                    employeeOffice.Deleted = true;
                    db.Entry(employeeOffice).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeOfficeExists(id))
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

            EmployeeOfficeDTO dto = employeeOffice.ToDTO();

            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeOfficeExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeOffices.Count(e => e.Id == id) > 0;
            }
        }
    }
}
