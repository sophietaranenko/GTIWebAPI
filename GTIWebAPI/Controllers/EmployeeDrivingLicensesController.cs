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
    [RoutePrefix("api/EmployeeDrivingLicenses")]
    public class EmployeeDrivingLicensesController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All drivingLicenses
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeDrivingLicenseDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            IEnumerable<EmployeeDrivingLicenseDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeDrivingLicense>, IEnumerable<EmployeeDrivingLicenseDTO>>
                (db.EmployeeDrivingLicense.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee drivingLicense by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeDrivingLicenseDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetDrivingLicensesByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeDrivingLicenseDTO>))]
        public IEnumerable<EmployeeDrivingLicenseDTO> GetByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            IEnumerable<EmployeeDrivingLicenseDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeDrivingLicense>, IEnumerable<EmployeeDrivingLicenseDTO>>
                (db.EmployeeDrivingLicense.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one drivingLicense for view by drivingLicense id
        /// </summary>
        /// <param name="id">EmployeeDrivingLicense id</param>
        /// <returns>EmployeeDrivingLicenseEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetDrivingLicenseView", Name = "GetDrivingLicenseView")]
        [ResponseType(typeof(EmployeeDrivingLicenseDTO))]
        public IHttpActionResult GetDrivingLicenseView(int id)
        {
            EmployeeDrivingLicense drivingLicense = db.EmployeeDrivingLicense.Find(id);
            if (drivingLicense == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            EmployeeDrivingLicenseDTO dto = Mapper.Map<EmployeeDrivingLicenseDTO>(drivingLicense);
            return Ok(dto);
        }

        /// <summary>
        /// Get one drivingLicense for edit by drivingLicense id
        /// </summary>
        /// <param name="id">EmployeeDrivingLicense id</param>
        /// <returns>EmployeeDrivingLicenseEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetDrivingLicenseEdit")]
        [ResponseType(typeof(EmployeeDrivingLicenseDTO))]
        public IHttpActionResult GetDrivingLicenseEdit(int id)
        {
            EmployeeDrivingLicense drivingLicense = db.EmployeeDrivingLicense.Find(id);
            if (drivingLicense == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            EmployeeDrivingLicenseDTO dto = Mapper.Map<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>(drivingLicense);
            return Ok(dto);
        }

        /// <summary>
        /// Update employee drivingLicense
        /// </summary>
        /// <param name="id">DrivingLicense id</param>
        /// <param name="employeeDrivingLicense">EmployeeDrivingLicense object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutDrivingLicense")]
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            EmployeeDrivingLicenseDTO dto = Mapper.Map<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>(employeeDrivingLicense);
            return CreatedAtRoute("GetDrivingLicenseView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Insert new employee drivingLicense
        /// </summary>
        /// <param name="employeeDrivingLicense">EmployeeDrivingLicense object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostDrivingLicense")]
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

            employeeDrivingLicense.Id = employeeDrivingLicense.NewId(db);
            db.EmployeeDrivingLicense.Add(employeeDrivingLicense);

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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            EmployeeDrivingLicenseDTO dto = Mapper.Map<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>(employeeDrivingLicense);
            return CreatedAtRoute("GetDrivingLicenseView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete drivingLicense
        /// </summary>
        /// <param name="id">DrivingLicense Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteDrivingLicense")]
        [ResponseType(typeof(EmployeeDrivingLicense))]
        public IHttpActionResult DeleteEmployeeDrivingLicense(int id)
        {
            EmployeeDrivingLicense employeeDrivingLicense = db.EmployeeDrivingLicense.Find(id);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>();
            });
            EmployeeDrivingLicenseDTO dto = Mapper.Map<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>(employeeDrivingLicense);
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

        private bool EmployeeDrivingLicenseExists(int id)
        {
            return db.EmployeeDrivingLicense.Count(e => e.Id == id) > 0;
        }
    }
}