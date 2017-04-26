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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Document confirming employee's permission to store and carry weapon 
    /// </summary>
    [RoutePrefix("api/EmployeeGuns")]
    public class EmployeeGunsController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeeGunsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeGunsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeGunDTO>))]
        public IHttpActionResult GetEmployeeGunAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeGunDTO> guns = unitOfWork.EmployeeGunsRepository.Get(d => d.Deleted != true).Select(d => d.ToDTO());
                return Ok(guns);
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
        [ResponseType(typeof(IEnumerable<EmployeeGunDTO>))]
        public IHttpActionResult GetEmployeeGunByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitofWork = new UnitOfWork(factory);
                IEnumerable<EmployeeGunDTO> guns = unitofWork.EmployeeGunsRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId).Select(d => d.ToDTO());
                return Ok(guns);
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
        [Route("Get", Name = "GetEmployeeGun")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult GetEmployeeGun(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeGunDTO gun = unitOfWork.EmployeeGunsRepository.GetByID(id).ToDTO();
                return Ok(gun);
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
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeGun(int id, EmployeeGun employeeGun)
        {
            if (employeeGun == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeGun.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeGunsRepository.Update(employeeGun);
                unitOfWork.Save();
                EmployeeGunDTO gun = employeeGun.ToDTO();
                return Ok(gun);
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
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult PostEmployeeGun(EmployeeGun employeeGun)
        {
            if (employeeGun == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeGun.Id = employeeGun.NewId(unitOfWork);
                unitOfWork.EmployeeGunsRepository.Insert(employeeGun);
                unitOfWork.Save();
                EmployeeGunDTO gun = employeeGun.ToDTO();
                return CreatedAtRoute("GetEmployeeGun", new { id = gun.Id }, gun);
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
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult DeleteEmployeeGun(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeGun gun = unitOfWork.EmployeeGunsRepository.GetByID(id);
                gun.Deleted = true;
                unitOfWork.EmployeeGunsRepository.Update(gun);
                unitOfWork.Save();
                EmployeeGunDTO dto = gun.ToDTO();
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