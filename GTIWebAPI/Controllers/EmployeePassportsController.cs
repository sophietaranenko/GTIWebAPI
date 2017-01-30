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
    //  [Authorize]
    [RoutePrefix("api/EmployeePassports")]
    public class EmployeePassportsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Get all employee passports
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeePassportDTO> GetEmployeePassportAll()
        {
            List<EmployeePassport> passports = db.EmployeePassports.Where(p => p.Deleted != true).ToList();
            List<EmployeePassportDTO> dtos = passports.Select(p => p.ToDTO()).ToList();
            return dtos;
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
        public IEnumerable<EmployeePassportDTO> GetEmployeePassportByEmployee(int employeeId)
        {
            List<EmployeePassport> passports = db.EmployeePassports
                .Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList();
            List<EmployeePassportDTO> dtos = passports.Select(p => p.ToDTO()).ToList();
            return dtos;
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
            EmployeePassport passport = db.EmployeePassports.Find(id);
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

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually

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
            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually

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
            EmployeePassport employeePassport = db.EmployeePassports.Find(id);
            if (employeePassport == null)
            {
                return NotFound();
            }
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
            EmployeePassportDTO dto = employeePassport.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeePassportExists(int id)
        {
            return db.EmployeePassports.Count(e => e.Id == id) > 0;
        }
    }
}