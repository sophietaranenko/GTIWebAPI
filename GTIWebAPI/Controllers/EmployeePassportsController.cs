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
using AutoMapper;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Filters;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee passports
    /// </summary>
    [RoutePrefix("api/EmployeePassports")]
    public class EmployeePassportsController : ApiController
    {
        /// <summary>
        /// Get all employee passports
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeePassportDTO>))]
        public IHttpActionResult GetEmployeePassportAll()
        {
            List<EmployeePassport> passports = new List<EmployeePassport>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    passports = db.EmployeePassports.Where(p => p.Deleted != true)
                        .Include(d => d.Address)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<EmployeePassportDTO> dtos = passports.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Get employee passports by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeePassportDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeePassportDTO>))]
        public IHttpActionResult GetEmployeePassportByEmployee(int employeeId)
        {
            List<EmployeePassport> passports = new List<EmployeePassport>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    passports = db.EmployeePassports
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .Include(d => d.Address)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<EmployeePassportDTO> dtos = passports.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Get one passport by passport id
        /// </summary>
        /// <param name="id">EmployeePassport id</param>
        /// <returns>EmployeePassportEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeePassport")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult GetEmployeePassport(int id)
        {
            EmployeePassport passport = new EmployeePassport();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    passport = db.EmployeePassports.Find(id);
                    if (passport != null)
                    {
                        db.Entry(passport).Reference(d => d.Address).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (passport == null)
            {
                return NotFound();
            }
            EmployeePassportDTO dto = passport.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee passport
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="employeePassport">EmployeePassport object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeePassport(int id, EmployeePassport employeePassport)
        {
            if (employeePassport == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeePassport.Id)
            {
                return BadRequest();
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(employeePassport.Address).State = EntityState.Modified;

                    db.Entry(employeePassport).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeePassportExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (employeePassport.Address != null)
                    {
                        if (employeePassport.Address.PlaceId != null)
                        {
                            employeePassport.Address.AddressPlace = db.Places.Find(employeePassport.Address.PlaceId);
                        }
                        if (employeePassport.Address.LocalityId != null)
                        {
                            employeePassport.Address.AddressLocality = db.Localities.Find(employeePassport.Address.LocalityId);
                        }
                        if (employeePassport.Address.VillageId != null)
                        {
                            employeePassport.Address.AddressVillage = db.Villages.Find(employeePassport.Address.VillageId);
                        }
                        if (employeePassport.Address.RegionId != null)
                        {
                            employeePassport.Address.AddressRegion = db.Regions.Find(employeePassport.Address.RegionId);
                        }
                        if (employeePassport.Address.CountryId != null)
                        {
                            employeePassport.Address.Country = db.Countries.Find(employeePassport.Address.CountryId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeePassportDTO dto = employeePassport.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee passport
        /// </summary>
        /// <param name="employeePassport">EmployeePassport object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult PostEmployeePassport(EmployeePassport employeePassport)
        {
            if (employeePassport == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeePassport.Id = employeePassport.NewId(db);
                    employeePassport.Address.Id = employeePassport.Address.NewId(db);
                    employeePassport.AddressId = employeePassport.Address.Id;
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.Addresses.Add(employeePassport.Address);
                    db.EmployeePassports.Add(employeePassport);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeePassportExists(employeePassport.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (employeePassport.Address != null)
                    {
                        if (employeePassport.Address.PlaceId != null)
                        {
                            employeePassport.Address.AddressPlace = db.Places.Find(employeePassport.Address.PlaceId);
                        }
                        if (employeePassport.Address.LocalityId != null)
                        {
                            employeePassport.Address.AddressLocality = db.Localities.Find(employeePassport.Address.LocalityId);
                        }
                        if (employeePassport.Address.VillageId != null)
                        {
                            employeePassport.Address.AddressVillage = db.Villages.Find(employeePassport.Address.VillageId);
                        }
                        if (employeePassport.Address.RegionId != null)
                        {
                            employeePassport.Address.AddressRegion = db.Regions.Find(employeePassport.Address.RegionId);
                        }
                        if (employeePassport.Address.CountryId != null)
                        {
                            employeePassport.Address.Country = db.Countries.Find(employeePassport.Address.CountryId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeePassportDTO dto = employeePassport.ToDTO();
            return CreatedAtRoute("GetEmployeePassport", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete passport
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeePassport))]
        public IHttpActionResult DeleteEmployeePassport(int id)
        {
            EmployeePassport employeePassport = new EmployeePassport();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeePassport = db.EmployeePassports.Find(id);
                    if (employeePassport == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employeePassport).Reference(d => d.Address).Load();

                    employeePassport.Deleted = true;
                    db.Entry(employeePassport).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }

                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeePassportExists(id))
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

            EmployeePassportDTO dto = employeePassport.ToDTO();
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

        private bool EmployeePassportExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeePassports.Count(e => e.Id == id) > 0;
            }
        }
    }
}