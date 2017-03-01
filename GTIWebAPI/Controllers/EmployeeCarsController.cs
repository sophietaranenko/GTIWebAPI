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
    /// EmployeeCar is a car technical passport which employee owns 
    /// </summary>
    [RoutePrefix("api/EmployeeCars")]
    public class EmployeeCarsController : ApiController
    {
        /// <summary>
        /// All the cars
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeCarDTO>))]
        public IHttpActionResult GetEmplyeeCarAll()
        {
            IEnumerable<EmployeeCarDTO> dtos = new List<EmployeeCarDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeCars.Where(p => p.Deleted != true).ToList()
                        .Select(p => p.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get employee car by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeCarDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeCarDTO>))]
        public IHttpActionResult GetEmployeeCarByEmployee(int employeeId)
        {
            IEnumerable<EmployeeCarDTO> dtos = new List<EmployeeCarDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeCars.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList()
                        .Select(c => c.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            } 

            return Ok(dtos);
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
            EmployeeCar employeeCar = new EmployeeCar();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeCar = db.EmployeeCars.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }
            if (employeeCar == null)
            {
                return NotFound();
            }

            EmployeeCarDTO dto = employeeCar.ToDTO();
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

            try
            {
                using (DbMain db = new DbMain(User))
                {
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
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            EmployeeCarDTO dto = employeeCar.ToDTO(); 
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

            try
            {
                using (DbMain db = new DbMain(User))
                {
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
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            EmployeeCarDTO dto = employeeCar.ToDTO();
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
            EmployeeCar employeeCar = new EmployeeCar();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeCar = db.EmployeeCars.Find(id);
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
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            EmployeeCarDTO dto = employeeCar.ToDTO();
            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeCarExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeCars.Count(e => e.Id == id) > 0;
            }
        }
    }
}