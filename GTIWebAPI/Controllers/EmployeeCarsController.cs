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
using GTIWebAPI.Models.Repository;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// EmployeeCar is a car technical passport which employee owns 
    /// </summary>
    [RoutePrefix("api/EmployeeCars")]
    public class EmployeeCarsController : ApiController
    {
        private IEmployeeCarsRepository repo;

        public EmployeeCarsController()
        {
            repo = new EmployeeCarsRepository();
        }

        public EmployeeCarsController(IEmployeeCarsRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeCarDTO>))]
        public IHttpActionResult GetEmplyeeCarAll()
        {
            try
            {
                IEnumerable<EmployeeCarDTO> dtos = repo.GetAll().Select(p => p.ToDTO()).ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }  
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeCarDTO>))]
        public IHttpActionResult GetEmployeeCarByEmployee(int employeeId)
        {
            try
            {
                IEnumerable<EmployeeCarDTO> dtos = repo.GetByEmployeeId(employeeId).Select(d => d.ToDTO()).ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            } 
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
            try
            {
                EmployeeCar employeeCar = repo.Get(id);
                if (employeeCar == null)
                {
                    return NotFound();
                }
                EmployeeCarDTO dto = employeeCar.ToDTO();
                return Ok(dto);
            }
            catch (DataException e)
            {
                //log
                return BadRequest(e.Message);
            }         
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
            if (employeeCar == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeCar.Id)
            {
                return BadRequest();
            }
            try
            {
                employeeCar = repo.Edit(employeeCar);
                EmployeeCarDTO dto = employeeCar.ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                //log
                return BadRequest(e.Message);
            }
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
            if (employeeCar == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                employeeCar = repo.Add(employeeCar);
                EmployeeCarDTO dto = employeeCar.ToDTO();
                return CreatedAtRoute("GetEmployeeCar", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                //log
                return BadRequest(e.Message);
            }
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
            try
            {
                EmployeeCar employeeCar = repo.Delete(id);
                EmployeeCarDTO dto = employeeCar.ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                //log
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}