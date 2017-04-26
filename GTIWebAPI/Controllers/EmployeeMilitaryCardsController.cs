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
    [RoutePrefix("api/EmployeeMilitaryCards")]
    public class EmployeeMilitaryCardsController : ApiController
    {

        private IDbContextFactory factory;

        public EmployeeMilitaryCardsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeMilitaryCardsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeMilitaryCardDTO>))]
        public IHttpActionResult GetEmployeeMilitaryCardAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeMilitaryCardDTO> list = unitOfWork.EmployeeMilitaryCardsRepository.Get(d => d.Deleted != true).Select(d => d.ToDTO());  
                return Ok(list);
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
        [ResponseType(typeof(IEnumerable<EmployeeMilitaryCardDTO>))]
        public IHttpActionResult GetEmployeeMilitaryCardByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeMilitaryCardDTO> list = unitOfWork.EmployeeMilitaryCardsRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId).Select(d => d.ToDTO());
                return Ok(list);
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
        [Route("Get", Name = "GetEmployeeMilitaryCard")]
        [ResponseType(typeof(EmployeeMilitaryCardDTO))]
        public IHttpActionResult GetEmployeeMilitaryCardView(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeMilitaryCardDTO militaryCard = unitOfWork.EmployeeMilitaryCardsRepository.GetByID(id).ToDTO();
                return Ok(militaryCard);
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
        public IHttpActionResult PutEmployeeMilitaryCard(int id, EmployeeMilitaryCard employeeMilitaryCard)
        {
            if (employeeMilitaryCard == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeMilitaryCard.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeMilitaryCardsRepository.Update(employeeMilitaryCard);
                unitOfWork.Save();
                EmployeeMilitaryCardDTO dto = employeeMilitaryCard.ToDTO();
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
        [ResponseType(typeof(EmployeeMilitaryCardDTO))]
        public IHttpActionResult PostEmployeeMilitaryCard(EmployeeMilitaryCard employeeMilitaryCard)
        {
            if (employeeMilitaryCard == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employeeMilitaryCard.Id = employeeMilitaryCard.NewId(unitOfWork);
                unitOfWork.EmployeeMilitaryCardsRepository.Insert(employeeMilitaryCard);
                unitOfWork.Save();
                EmployeeMilitaryCardDTO dto = employeeMilitaryCard.ToDTO();
                return CreatedAtRoute("GetEmployeeMilitaryCard", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(EmployeeMilitaryCard))]
        public IHttpActionResult DeleteEmployeeMilitaryCard(int id)
        {
            EmployeeMilitaryCard employeeMilitaryCard = new EmployeeMilitaryCard();
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeMilitaryCard card = unitOfWork.EmployeeMilitaryCardsRepository.GetByID(id);
                card.Deleted = true;
                unitOfWork.EmployeeMilitaryCardsRepository.Update(card);
                unitOfWork.Save();
                EmployeeMilitaryCardDTO dto = card.ToDTO();
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

    }
}
