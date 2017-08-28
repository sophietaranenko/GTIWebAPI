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
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// EmployeeCar is a car technical passport which employee owns 
    /// </summary>
    [RoutePrefix("api/EmployeeCars")]
    public class EmployeeCarsController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeCarsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeCarsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeCarDTO>))]
        public IHttpActionResult GetEmplyeeCarAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeCarDTO> dtos =
                    unitOfWork.EmployeeCarsRepository.Get(d => d.Deleted != true).Select(d => d.ToDTO());
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeCar> cs = unitOfWork.EmployeeCarsRepository.
                    Get(d => d.Deleted != true && d.EmployeeId == employeeId);
                IEnumerable<EmployeeCarDTO> dtos = cs.Select(d => d.ToDTO());
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeCarDTO dto = unitOfWork.EmployeeCarsRepository.GetByID(id).ToDTO();
                if (dto == null)
                {
                    return NotFound();
                }
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
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
        public IHttpActionResult PutEmployeeCar(int id, EmployeeCarDTO employeeCar)
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
                EmployeeCar car = employeeCar.FromDTO();

                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeCarsRepository.Update(car);
                unitOfWork.Save();
                

                //cos there are no included object-properies we need to load, then just ToDTO call  
                EmployeeCarDTO dto = car.ToDTO();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
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
        public IHttpActionResult PostEmployeeCar(EmployeeCarDTO employeeCar)
        {
            if (employeeCar == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeCar car = employeeCar.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeCar.Id = car.NewId(unitOfWork);
                unitOfWork.EmployeeCarsRepository.Insert(car);
                unitOfWork.Save();
                //cos there are no included object-properies we need to load, then just ToDTO call  
                EmployeeCarDTO dto = car.ToDTO();
                return CreatedAtRoute("GetEmployeeCar", new { id = dto.Id }, dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                EmployeeCar employeeCar = unitOfWork.EmployeeCarsRepository.GetByID(id);

                employeeCar.Deleted = true;
                unitOfWork.EmployeeCarsRepository.Update(employeeCar);
                unitOfWork.Save();

                EmployeeCarDTO dto = employeeCar.ToDTO();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}