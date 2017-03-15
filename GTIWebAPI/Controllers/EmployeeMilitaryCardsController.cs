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

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/EmployeeMilitaryCards")]
    public class EmployeeMilitaryCardsController : ApiController
    {

        private IRepository<EmployeeMilitaryCard> repo;

        public EmployeeMilitaryCardsController()
        {
            repo = new EmployeeMilitaryCardsRepository();
        }

        public EmployeeMilitaryCardsController(IRepository<EmployeeMilitaryCard> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeMilitaryCardDTO>))]
        public IHttpActionResult GetEmployeeMilitaryCardAll()
        {
            try
            {
                List<EmployeeMilitaryCardDTO> list =
                    repo.GetAll()
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                List<EmployeeMilitaryCardDTO> list =
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest();
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
                EmployeeMilitaryCardDTO militaryCard = repo.Get(id).ToDTO();
                return Ok(militaryCard);
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
            if (employeeMilitaryCard == null || !ModelState.IsValid || id != employeeMilitaryCard.Id)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeMilitaryCardDTO dto = repo.Edit(employeeMilitaryCard).ToDTO();
                return Ok(dto);
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
                EmployeeMilitaryCardDTO dto = repo.Add(employeeMilitaryCard).ToDTO();
                return CreatedAtRoute("GetEmployeeMilitaryCard", new { id = dto.Id }, dto);
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
                EmployeeMilitaryCardDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
