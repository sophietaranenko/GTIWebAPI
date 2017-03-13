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
using System;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Driving license is a document employee needs to drive a car 
    /// </summary>
    [RoutePrefix("api/EmployeeDrivingLicenses")]
    public class EmployeeDrivingLicensesController : ApiController
    {
        /// <summary>
        /// All driving licenses
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeDrivingLicenseDTO>))]
        public IHttpActionResult GetEmployeeDrivingLicenseAll()
        {
            IEnumerable<EmployeeDrivingLicenseDTO> dtos = new List<EmployeeDrivingLicenseDTO>();
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeDrivingLicenses.Where(p => p.Deleted != true).ToList()
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
        /// Get employee driving license by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeDrivingLicenseDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeDrivingLicenseDTO>))]
        public IHttpActionResult GetEmployeeDrivingLicenseByEmployee(int employeeId)
        {
            IEnumerable<EmployeeDrivingLicenseDTO> dtos = new List<EmployeeDrivingLicenseDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeDrivingLicenses.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList()
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
        /// Get one driving license for view by driving license id
        /// </summary>
        /// <param name="id">EmployeeDrivingLicense id</param>
        /// <returns>EmployeeDrivingLicenseEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetDrivingLicense")]
        [ResponseType(typeof(EmployeeDrivingLicenseDTO))]
        public IHttpActionResult GetDrivingLicense(int id)
        {
            EmployeeDrivingLicense employeeDrivingLicense = new EmployeeDrivingLicense();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeDrivingLicense = db.EmployeeDrivingLicenses.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            if (employeeDrivingLicense == null)
            {
                return NotFound();
            }
            EmployeeDrivingLicenseDTO dto = employeeDrivingLicense.ToDTO();
            return Ok(dto);
        }


        /// <summary>
        /// Update employee driving license
        /// </summary>
        /// <param name="id">Driving license id</param>
        /// <param name="employeeDrivingLicense">EmployeeDrivingLicense object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeDrivingLicense(int id, EmployeeDrivingLicense employeeDrivingLicense)
        {
            if (employeeDrivingLicense == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeDrivingLicense.Id)
            {
                return BadRequest();
            }
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    db.Entry(employeeDrivingLicense).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeDrivingLicenseExists(id))
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
                return BadRequest("Troubles with database connection");
            }

            EmployeeDrivingLicenseDTO dto = employeeDrivingLicense.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee driving license
        /// </summary>
        /// <param name="employeeDrivingLicense">EmployeeDrivingLicense object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeDrivingLicenseDTO))]
        public IHttpActionResult PostEmployeeDrivingLicense(EmployeeDrivingLicense employeeDrivingLicense)
        {
            if (employeeDrivingLicense == null)
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
                    employeeDrivingLicense.Id = employeeDrivingLicense.NewId(db);
                    db.EmployeeDrivingLicenses.Add(employeeDrivingLicense);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeDrivingLicenseExists(employeeDrivingLicense.Id))
                        {
                            return Conflict();
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
                return BadRequest("Troubles with database connection");
            }

            EmployeeDrivingLicenseDTO dto = employeeDrivingLicense.ToDTO();
            return CreatedAtRoute("GetDrivingLicense", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete driving license
        /// </summary>
        /// <param name="id">Driving license id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeDrivingLicense))]
        public IHttpActionResult DeleteEmployeeDrivingLicense(int id)
        {
            EmployeeDrivingLicense employeeDrivingLicense = new EmployeeDrivingLicense();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeDrivingLicense = db.EmployeeDrivingLicenses.Find(id);
                    if (employeeDrivingLicense == null)
                    {
                        return NotFound();
                    }
                    employeeDrivingLicense.Deleted = true;
                    db.Entry(employeeDrivingLicense).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeDrivingLicenseExists(id))
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
                return BadRequest("Troubles with database connection");
            }

            EmployeeDrivingLicenseDTO dto = employeeDrivingLicense.ToDTO();
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

        private bool EmployeeDrivingLicenseExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.EmployeeDrivingLicenses.Count(e => e.Id == id) > 0;
            }
        }
    }
}