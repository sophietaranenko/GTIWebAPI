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

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/EmployeeInternationalPassports")]
    public class EmployeeInternationalPassportsController : ApiController
    {
        /// <summary>
        /// All internationalPassports
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeInternationalPassportDTO>))]
        public IHttpActionResult GetEmployeeInternationalPassportAll()
        {
            IEnumerable<EmployeeInternationalPassportDTO> dtos = new List<EmployeeInternationalPassportDTO>();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeInternationalPassports.Where(p => p.Deleted != true).ToList()
                            .Select(e => e.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get employee internationalPassport by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeInternationalPassportDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeInternationalPassportDTO>))]
        public IHttpActionResult GetEmployeeInternationalPassportByEmployee(int employeeId)
        {
            IEnumerable<EmployeeInternationalPassportDTO> dtos = new List<EmployeeInternationalPassportDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos =
                     db.EmployeeInternationalPassports.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList()
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
        /// Get one internationalPassport for edit by internationalPassport id
        /// </summary>
        /// <param name="id">EmployeeInternationalPassport id</param>
        /// <returns>EmployeeInternationalPassportEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeInternationalPassport")]
        [ResponseType(typeof(EmployeeInternationalPassportDTO))]
        public IHttpActionResult GetEmployeeInternationalPassport(int id)
        {
            EmployeeInternationalPassport internationalPassport = new EmployeeInternationalPassport();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    internationalPassport = db.EmployeeInternationalPassports.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (internationalPassport == null)
            {
                return NotFound();
            }

            EmployeeInternationalPassportDTO dto = internationalPassport.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee internationalPassport
        /// </summary>
        /// <param name="id">InternationalPassport id</param>
        /// <param name="employeeInternationalPassport">EmployeeInternationalPassport object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeInternationalPassport(int id, EmployeeInternationalPassport employeeInternationalPassport)
        {
            if (employeeInternationalPassport == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeInternationalPassport.Id)
            {
                return BadRequest();
            }
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(employeeInternationalPassport).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeInternationalPassportExists(id))
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

            EmployeeInternationalPassportDTO dto = employeeInternationalPassport.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee internationalPassport
        /// </summary>
        /// <param name="employeeInternationalPassport">EmployeeInternationalPassport object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeInternationalPassportDTO))]
        public IHttpActionResult PostEmployeeInternationalPassport(EmployeeInternationalPassport employeeInternationalPassport)
        {
            if (employeeInternationalPassport == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeInternationalPassport.Id = employeeInternationalPassport.NewId(db);
                    db.EmployeeInternationalPassports.Add(employeeInternationalPassport);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeInternationalPassportExists(employeeInternationalPassport.Id))
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
                return BadRequest();
            }

            EmployeeInternationalPassportDTO dto = employeeInternationalPassport.ToDTO();
            return CreatedAtRoute("GetEmployeeInternationalPassport", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete internationalPassport
        /// </summary>
        /// <param name="id">InternationalPassport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeInternationalPassport))]
        public IHttpActionResult DeleteEmployeeInternationalPassport(int id)
        {
            EmployeeInternationalPassport employeeInternationalPassport = null;

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeInternationalPassport = db.EmployeeInternationalPassports.Find(id);
                    if (employeeInternationalPassport == null)
                    {
                        return NotFound();
                    }
                    employeeInternationalPassport.Deleted = true;
                    db.Entry(employeeInternationalPassport).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeInternationalPassportExists(id))
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

            EmployeeInternationalPassportDTO dto = employeeInternationalPassport.ToDTO();
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

        private bool EmployeeInternationalPassportExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeInternationalPassports.Count(e => e.Id == id) > 0;
            }
        }
    }
}