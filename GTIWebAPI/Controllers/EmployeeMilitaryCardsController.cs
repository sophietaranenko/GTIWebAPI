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
        /// <summary>
        /// All militaryCards
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeMilitaryCardDTO>))]
        public IHttpActionResult GetEmployeeMilitaryCardAll()
        {
            IEnumerable<EmployeeMilitaryCardDTO> dtos = new List<EmployeeMilitaryCardDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeMilitaryCards.Where(p => p.Deleted != true).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
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
        public IHttpActionResult GetEmployeeMilitaryCardByEmployee(int employeeId)
        {
            IEnumerable<EmployeeMilitaryCardDTO> dtos = new List<EmployeeMilitaryCardDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.EmployeeMilitaryCards.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
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
            EmployeeMilitaryCard militaryCard = new EmployeeMilitaryCard();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    militaryCard = db.EmployeeMilitaryCards.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (militaryCard == null)
            {
                return NotFound();
            }

            EmployeeMilitaryCardDTO dto = militaryCard.ToDTO();
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

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeMilitaryCardDTO dto = employeeMilitaryCard.ToDTO();
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

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }


            EmployeeMilitaryCardDTO dto = employeeMilitaryCard.ToDTO();
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
            EmployeeMilitaryCard employeeMilitaryCard = new EmployeeMilitaryCard();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    employeeMilitaryCard = db.EmployeeMilitaryCards.Find(id);
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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeMilitaryCardDTO dto = employeeMilitaryCard.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeMilitaryCardExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.EmployeeMilitaryCards.Count(e => e.Id == id) > 0;
            }
        }
    }
}
