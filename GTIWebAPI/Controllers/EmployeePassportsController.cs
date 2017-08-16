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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee passports
    /// </summary>
    [RoutePrefix("api/EmployeePassports")]
    public class EmployeePassportsController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeePassportsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeePassportsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeePassportDTO>))]
        public IHttpActionResult GetEmployeePassportAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeePassportDTO> passports = unitOfWork.EmployeePassportsRepository
                    .Get(d => d.Deleted != true, includeProperties: "Address, Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage").Select(d => d.ToDTO());
                return Ok(passports);
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
        [ResponseType(typeof(IEnumerable<EmployeePassportDTO>))]
        public IHttpActionResult GetEmployeePassportByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeePassportDTO> passports = unitOfWork.EmployeePassportsRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == employeeId, 
                    includeProperties: "Address, Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage").Select(d => d.ToDTO());
                return Ok(passports);
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
        [Route("Get", Name = "GetEmployeePassport")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult GetEmployeePassport(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeePassport p = unitOfWork.EmployeePassportsRepository
                   .Get(d => d.Id == id,
                   includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
                   .FirstOrDefault();
                EmployeePassportDTO employeePassport = p.ToDTO();

                return Ok(employeePassport);
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
        public IHttpActionResult PutEmployeePassport(int id, EmployeePassportDTO employeePassport)
        {
            if (employeePassport == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeePassport.Id)
            {
                return BadRequest();
            }
            try
            {
                EmployeePassport passport = employeePassport.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeePassportsRepository.Update(passport);
                unitOfWork.Save();
                EmployeePassportDTO dto = unitOfWork.EmployeePassportsRepository
                   .Get(d => d.Id == id,
                   includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage").FirstOrDefault().ToDTO();
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

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeePassportDTO))]
        public IHttpActionResult PostEmployeePassport(EmployeePassportDTO employeePassport)
        {
            if (employeePassport == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeePassport passport = employeePassport.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                passport.Address.Id = passport.Address.NewId(unitOfWork);
                passport.AddressId = passport.Address.Id;
                passport.Id = passport.NewId(unitOfWork);
                unitOfWork.EmployeePassportsRepository.Insert(passport);
                unitOfWork.Save();
                EmployeePassportDTO dto = unitOfWork.EmployeePassportsRepository
                   .Get(d => d.Id == passport.Id,
                   includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetEmployeePassport", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeePassport))]
        public IHttpActionResult DeleteEmployeePassport(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeePassport passport = unitOfWork.EmployeePassportsRepository
                   .Get(d => d.EmployeeId == id,
                   includeProperties: "Address, Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage").FirstOrDefault();
                passport.Deleted = true;
                unitOfWork.EmployeePassportsRepository.Update(passport);
                unitOfWork.Save();
                EmployeePassportDTO dto = passport.ToDTO();
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