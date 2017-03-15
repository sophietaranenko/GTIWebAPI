using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Employee's positions in departments of offices 
    /// </summary>
    [RoutePrefix("api/EmployeeOffices")]
    public class EmployeeOfficesController : ApiController
    {
        private IRepository<EmployeeOffice> repo;

        public EmployeeOfficesController()
        {
            this.repo = new EmployeeOfficesRepository();
        }

        public EmployeeOfficesController(IRepository<EmployeeOffice> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeOfficeDTO>))]
        public IHttpActionResult GetEmployeeOfficeAll()
        {
            try
            {
                List<EmployeeOfficeDTO> list =
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
        [ResponseType(typeof(IEnumerable<EmployeeOfficeDTO>))]
        public IHttpActionResult GetEmployeeOfficeByEmployeeId(int employeeId)
        {
            try
            {
                List<EmployeeOfficeDTO> dtos = repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeOffice")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult GetEmployeeOffice(int id)
        {
            try
            {
                EmployeeOfficeDTO dto = repo.Get(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeOffice(int id, EmployeeOffice employeeOffice)
        {
            if (!ModelState.IsValid || id != employeeOffice.Id)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeOfficeDTO dto = repo.Edit(employeeOffice).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult PostEmployeeOffice(EmployeeOffice employeeOffice)
        {
            if (employeeOffice == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeOfficeDTO dto = repo.Add(employeeOffice).ToDTO();
                return CreatedAtRoute("GetEmployeeOffice", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult DeleteEmployeeOffice(int id)
        {
            try
            {
                EmployeeOfficeDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeOfficeExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.EmployeeOffices.Count(e => e.Id == id) > 0;
            }
        }
    }
}
