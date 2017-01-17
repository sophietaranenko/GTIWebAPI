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
        public IEnumerable<EmployeePassportDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            IEnumerable<EmployeePassportDTO> dtos = Mapper
                .Map<IEnumerable<EmployeePassport>, IEnumerable<EmployeePassportDTO>>
                (db.EmployeePassport.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee passports by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeePassportDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetPassportsByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeePassportDTO>))]
        public IEnumerable<EmployeePassportDTO> GetByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            IEnumerable<EmployeePassportDTO> dtos = Mapper
                .Map<IEnumerable<EmployeePassport>, IEnumerable<EmployeePassportDTO>>
                (db.EmployeePassport.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one passport for view by passport id
        /// </summary>
        /// <param name="id">EmployeePassport id</param>
        /// <returns>EmployeePassportEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetPassportView", Name = "GetPassportView")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult GetPassportView(int id)
        {
            EmployeePassport passport = db.EmployeePassport.Find(id);
            if (passport == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeePassportDTO dto = Mapper.Map<EmployeePassportDTO>(passport);
            return Ok(dto);
        }

        /// <summary>
        /// Get one passport for edit by passport id
        /// </summary>
        /// <param name="id">EmployeePassport id</param>
        /// <returns>EmployeePassportEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetPassportEdit")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult GetPassportEdit(int id)
        {
            EmployeePassport passport = db.EmployeePassport.Find(id);
            if (passport == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeePassportDTO dto = Mapper.Map<EmployeePassport, EmployeePassportDTO>(passport);
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
        [Route("PutPassport")]
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
            employeePassport = db.EmployeePassport.Find(employeePassport.Id);
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeePassportDTO dto = Mapper.Map<EmployeePassport, EmployeePassportDTO>(employeePassport);
            return CreatedAtRoute("GetPassportView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Insert new employee passport
        /// </summary>
        /// <param name="employeePassport">EmployeePassport object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostPassport")]
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
            db.Address.Add(employeePassport.Address);
            db.EmployeePassport.Add(employeePassport);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeePassportDTO dto = Mapper.Map<EmployeePassport, EmployeePassportDTO>(employeePassport);
            return CreatedAtRoute("GetPassportView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete passport
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeletePassport")]
        [ResponseType(typeof(EmployeePassport))]
        public IHttpActionResult DeleteEmployeePassport(int id)
        {
            EmployeePassport employeePassport = db.EmployeePassport.Find(id);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeePassportDTO dto = Mapper.Map<EmployeePassport, EmployeePassportDTO>(employeePassport);
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
            return db.EmployeePassport.Count(e => e.Id == id) > 0;
        }
    }
}