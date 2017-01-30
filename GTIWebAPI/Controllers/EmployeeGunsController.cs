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
    [RoutePrefix("api/EmployeeGuns")]
    public class EmployeeGunsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All guns
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeGunDTO> GetEmployeeGunAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeGun, EmployeeGunDTO>();
            });
            IEnumerable<EmployeeGunDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeGun>, IEnumerable<EmployeeGunDTO>>
                (db.EmployeeGun.Where(p => p.Deleted != true).ToList());
            return dtos;
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
        public IEnumerable<EmployeeGunDTO> GetEmployeeGunByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeGun, EmployeeGunDTO>();
            });
            IEnumerable<EmployeeGunDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeGun>, IEnumerable<EmployeeGunDTO>>
                (db.EmployeeGun.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one gun for view by gun id
        /// </summary>
        /// <param name="id">EmployeeGun id</param>
        /// <returns>EmployeeGunEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeGun")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult GetEmployeeGun(int id)
        {
            EmployeeGun gun = db.EmployeeGun.Find(id);
            if (gun == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeGun, EmployeeGunDTO>();
            });
            EmployeeGunDTO dto = Mapper.Map<EmployeeGunDTO>(gun);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeGun, EmployeeGunDTO>();
            });
            EmployeeGunDTO dto = Mapper.Map<EmployeeGun, EmployeeGunDTO>(employeeGun);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeGun, EmployeeGunDTO>();
            });
            EmployeeGunDTO dto = Mapper.Map<EmployeeGun, EmployeeGunDTO>(employeeGun);
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
            EmployeeGun employeeGun = db.EmployeeGun.Find(id);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeGun, EmployeeGunDTO>();
            });
            EmployeeGunDTO dto = Mapper.Map<EmployeeGun, EmployeeGunDTO>(employeeGun);
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

        private bool EmployeeGunExists(int id)
        {
            return db.EmployeeGun.Count(e => e.Id == id) > 0;
        }
    }
}