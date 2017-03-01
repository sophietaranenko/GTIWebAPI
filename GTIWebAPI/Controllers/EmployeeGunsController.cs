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
    /// <summary>
    /// Document confirming employee's permission to store and carry weapon 
    /// </summary>
    [RoutePrefix("api/EmployeeGuns")]
    public class EmployeeGunsController : ApiController
    {
        /// <summary>
        /// All guns
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeGunDTO>))]
        public IHttpActionResult GetEmployeeGunAll()
        {
            IEnumerable<EmployeeGunDTO> dtos = new List<EmployeeGunDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeGun.Where(p => p.Deleted != true).ToList()
                        .Select(g => g.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get employee gun by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeGunDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeGunDTO>))]
        public IHttpActionResult GetEmployeeGunByEmployee(int employeeId)
        {
            IEnumerable<EmployeeGunDTO> dtos = null;

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeGun.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList()
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
        /// Get one gun permission for view by gun id
        /// </summary>
        /// <param name="id">EmployeeGun id</param>
        /// <returns>EmployeeGunEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeGun")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult GetEmployeeGun(int id)
        {
            EmployeeGun gun = new EmployeeGun();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    gun = db.EmployeeGun.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (gun == null)
            {
                return NotFound();
            }

            EmployeeGunDTO dto = gun.ToDTO();

            return Ok(dto);
        }


        /// <summary>
        /// Update employee gun
        /// </summary>
        /// <param name="id">Gun id</param>
        /// <param name="employeeGun">EmployeeGun object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeGun(int id, EmployeeGun employeeGun)
        {
            if (employeeGun == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeGun.Id)
            {
                return BadRequest();
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(employeeGun).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeGunExists(id))
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

            EmployeeGunDTO dto = employeeGun.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee gun
        /// </summary>
        /// <param name="employeeGun">EmployeeGun object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult PostEmployeeGun(EmployeeGun employeeGun)
        {
            if (employeeGun == null)
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
                    employeeGun.Id = employeeGun.NewId(db);
                    db.EmployeeGun.Add(employeeGun);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeGunExists(employeeGun.Id))
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

            EmployeeGunDTO dto = employeeGun.ToDTO();
            return CreatedAtRoute("GetEmployeeGun", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete gun
        /// </summary>
        /// <param name="id">Gun Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeGun))]
        public IHttpActionResult DeleteEmployeeGun(int id)
        {
            EmployeeGun employeeGun = new EmployeeGun();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeGun = db.EmployeeGun.Find(id);
                    if (employeeGun == null)
                    {
                        return NotFound();
                    }
                    employeeGun.Deleted = true;
                    db.Entry(employeeGun).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeGunExists(id))
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

            EmployeeGunDTO dto = employeeGun.ToDTO();
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

        private bool EmployeeGunExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeGun.Count(e => e.Id == id) > 0;
            }
        }
    }
}