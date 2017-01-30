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
    [RoutePrefix("api/EmployeeMilitaryCards")]
    public class EmployeeMilitaryCardsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All militaryCards
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeMilitaryCardDTO> GetEmployeeMilitaryCardAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>();
            });
            IEnumerable<EmployeeMilitaryCardDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeMilitaryCard>, IEnumerable<EmployeeMilitaryCardDTO>>
                (db.EmployeeMilitaryCards.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee militaryCard by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeMilitaryCardDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeMilitaryCardDTO>))]
        public IEnumerable<EmployeeMilitaryCardDTO> GetEmployeeMilitaryCardByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>();
            });
            IEnumerable<EmployeeMilitaryCardDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeMilitaryCard>, IEnumerable<EmployeeMilitaryCardDTO>>
                (db.EmployeeMilitaryCards.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one militaryCard for view by militaryCard id
        /// </summary>
        /// <param name="id">EmployeeMilitaryCard id</param>
        /// <returns>EmployeeMilitaryCardEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeMilitaryCard")]
        [ResponseType(typeof(EmployeeMilitaryCardDTO))]
        public IHttpActionResult GetEmployeeMilitaryCardView(int id)
        {
            EmployeeMilitaryCard militaryCard = db.EmployeeMilitaryCards.Find(id);
            if (militaryCard == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>();
            });
            EmployeeMilitaryCardDTO dto = Mapper.Map<EmployeeMilitaryCardDTO>(militaryCard);
            return Ok(dto);
        }


        /// <summary>
        /// Update employee militaryCard
        /// </summary>
        /// <param name="id">MilitaryCard id</param>
        /// <param name="employeeMilitaryCard">EmployeeMilitaryCard object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeMilitaryCard(int id, EmployeeMilitaryCard employeeMilitaryCard)
        {
            if (employeeMilitaryCard == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeMilitaryCard.Id)
            {
                return BadRequest();
            }
            db.Entry(employeeMilitaryCard).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeMilitaryCardExists(id))
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
                m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>();
            });
            EmployeeMilitaryCardDTO dto = Mapper.Map<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>(employeeMilitaryCard);
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee militaryCard
        /// </summary>
        /// <param name="employeeMilitaryCard">EmployeeMilitaryCard object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeMilitaryCardDTO))]
        public IHttpActionResult PostEmployeeMilitaryCard(EmployeeMilitaryCard employeeMilitaryCard)
        {
            if (employeeMilitaryCard == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employeeMilitaryCard.Id = employeeMilitaryCard.NewId(db);
            db.EmployeeMilitaryCards.Add(employeeMilitaryCard);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeMilitaryCardExists(employeeMilitaryCard.Id))
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
                m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>();
            });
            EmployeeMilitaryCardDTO dto = Mapper.Map<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>(employeeMilitaryCard);
            return CreatedAtRoute("GetEmployeeMilitaryCard", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete militaryCard
        /// </summary>
        /// <param name="id">MilitaryCard Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeMilitaryCard))]
        public IHttpActionResult DeleteEmployeeMilitaryCard(int id)
        {
            EmployeeMilitaryCard employeeMilitaryCard = db.EmployeeMilitaryCards.Find(id);
            if (employeeMilitaryCard == null)
            {
                return NotFound();
            }
            employeeMilitaryCard.Deleted = true;
            db.Entry(employeeMilitaryCard).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeMilitaryCardExists(id))
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
                m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>();
            });
            EmployeeMilitaryCardDTO dto = Mapper.Map<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>(employeeMilitaryCard);
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

        private bool EmployeeMilitaryCardExists(int id)
        {
            return db.EmployeeMilitaryCards.Count(e => e.Id == id) > 0;
        }
    }
}
