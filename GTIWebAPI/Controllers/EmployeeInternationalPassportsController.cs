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
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All internationalPassports
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeInternationalPassportDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>();
            });
            IEnumerable<EmployeeInternationalPassportDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeInternationalPassport>, IEnumerable<EmployeeInternationalPassportDTO>>
                (db.EmployeeInternationalPassport.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee internationalPassport by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeInternationalPassportDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetInternationalPassportsByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeInternationalPassportDTO>))]
        public IEnumerable<EmployeeInternationalPassportDTO> GetByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>();
            });
            IEnumerable<EmployeeInternationalPassportDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeInternationalPassport>, IEnumerable<EmployeeInternationalPassportDTO>>
                (db.EmployeeInternationalPassport.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one internationalPassport for view by internationalPassport id
        /// </summary>
        /// <param name="id">EmployeeInternationalPassport id</param>
        /// <returns>EmployeeInternationalPassportEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetInternationalPassportView", Name = "GetInternationalPassportView")]
        [ResponseType(typeof(EmployeeInternationalPassportDTO))]
        public IHttpActionResult GetInternationalPassportView(int id)
        {
            EmployeeInternationalPassport internationalPassport = db.EmployeeInternationalPassport.Find(id);
            if (internationalPassport == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>();
            });
            EmployeeInternationalPassportDTO dto = Mapper.Map<EmployeeInternationalPassportDTO>(internationalPassport);
            return Ok(dto);
        }

        /// <summary>
        /// Get one internationalPassport for edit by internationalPassport id
        /// </summary>
        /// <param name="id">EmployeeInternationalPassport id</param>
        /// <returns>EmployeeInternationalPassportEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetInternationalPassportEdit")]
        [ResponseType(typeof(EmployeeInternationalPassportDTO))]
        public IHttpActionResult GetInternationalPassportEdit(int id)
        {
            EmployeeInternationalPassport internationalPassport = db.EmployeeInternationalPassport.Find(id);
            if (internationalPassport == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>();
            });
            EmployeeInternationalPassportDTO dto = Mapper.Map<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>(internationalPassport);
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
        [Route("PutInternationalPassport")]
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
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Insert new employee internationalPassport
        /// </summary>
        /// <param name="employeeInternationalPassport">EmployeeInternationalPassport object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostInternationalPassport")]
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

            employeeInternationalPassport.Id = employeeInternationalPassport.NewId(db);
            db.EmployeeInternationalPassport.Add(employeeInternationalPassport);

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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>();
            });
            EmployeeInternationalPassportDTO dto = Mapper.Map<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>(employeeInternationalPassport);
            return CreatedAtRoute("GetInternationalPassportView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete internationalPassport
        /// </summary>
        /// <param name="id">InternationalPassport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteInternationalPassport")]
        [ResponseType(typeof(EmployeeInternationalPassport))]
        public IHttpActionResult DeleteEmployeeInternationalPassport(int id)
        {
            EmployeeInternationalPassport employeeInternationalPassport = db.EmployeeInternationalPassport.Find(id);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>();
            });
            EmployeeInternationalPassportDTO dto = Mapper.Map<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>(employeeInternationalPassport);
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

        private bool EmployeeInternationalPassportExists(int id)
        {
            return db.EmployeeInternationalPassport.Count(e => e.Id == id) > 0;
        }
    }
}