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
    /// Manipulating Employee cars 
    /// </summary>
    [RoutePrefix("api/EmployeeCars")]
    public class EmployeeCarsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All the cars
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeCarDTO> GetEmplyeeCarAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeCar, EmployeeCarDTO>();
            });
            IEnumerable<EmployeeCarDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeCar>, IEnumerable<EmployeeCarDTO>>
                (db.EmployeeCars.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee car by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeCarDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeCarDTO>))]
        public IEnumerable<EmployeeCarDTO> GetEmployeeCarByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeCar, EmployeeCarDTO>();
            });
            IEnumerable<EmployeeCarDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeCar>, IEnumerable<EmployeeCarDTO>>
                (db.EmployeeCars.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one car for view by car id
        /// </summary>
        /// <param name="id">EmployeeCar id</param>
        /// <returns>EmployeeCarEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeCar")]
        [ResponseType(typeof(EmployeeCarDTO))]
        public IHttpActionResult GetEmployeeCar(int id)
        {
            EmployeeCar car = db.EmployeeCars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeCar, EmployeeCarDTO>();
            });
            EmployeeCarDTO dto = Mapper.Map<EmployeeCarDTO>(car);
            return Ok(dto);
        }

        /// <summary>
        /// Update employee car
        /// </summary>
        /// <param name="id">Car id</param>
        /// <param name="employeeCar">EmployeeCar object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeCar(int id, EmployeeCar employeeCar)
        {
            if (employeeCar == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeCar.Id)
            {
                return BadRequest();
            }
            db.Entry(employeeCar).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeCarExists(id))
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
                m.CreateMap<EmployeeCar, EmployeeCarDTO>();
            });
            EmployeeCarDTO dto = Mapper.Map<EmployeeCar, EmployeeCarDTO>(employeeCar);
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee car
        /// </summary>
        /// <param name="employeeCar">EmployeeCar object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeCarDTO))]
        public IHttpActionResult PostEmployeeCar(EmployeeCar employeeCar)
        {
            if (employeeCar == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employeeCar.Id = employeeCar.NewId(db);
            db.EmployeeCars.Add(employeeCar);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeCarExists(employeeCar.Id))
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
                m.CreateMap<EmployeeCar, EmployeeCarDTO>();
            });
            EmployeeCarDTO dto = Mapper.Map<EmployeeCar, EmployeeCarDTO>(employeeCar);
            return CreatedAtRoute("GetEmployeeCar", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete car
        /// </summary>
        /// <param name="id">Car Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeCar))]
        public IHttpActionResult DeleteEmployeeCar(int id)
        {
            EmployeeCar employeeCar = db.EmployeeCars.Find(id);
            if (employeeCar == null)
            {
                return NotFound();
            }
            employeeCar.Deleted = true;
            db.Entry(employeeCar).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeCarExists(id))
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
                m.CreateMap<EmployeeCar, EmployeeCarDTO>();
            });
            EmployeeCarDTO dto = Mapper.Map<EmployeeCar, EmployeeCarDTO>(employeeCar);
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

        private bool EmployeeCarExists(int id)
        {
            return db.EmployeeCars.Count(e => e.Id == id) > 0;
        }
    }
}