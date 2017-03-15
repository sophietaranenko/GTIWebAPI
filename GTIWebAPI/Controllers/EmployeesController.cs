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
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Service;
using AutoMapper;
using GTIWebAPI.Filters;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Repository;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller manipulating employees
    /// </summary>
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        private IEmployeesRepository repo;

        public EmployeesController()
        {
            repo = new EmployeesRepository();
        }

        public EmployeesController(IEmployeesRepository repo)
        {
            this.repo = repo;
        }

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeViewDTO>))]
        public IHttpActionResult GetEmployeeAll(string officeIds)
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            IEnumerable<EmployeeView> employeeList = new List<EmployeeView>();
            try
            {
                List<EmployeeViewDTO> dtos = 
                    repo.GetAll(OfficeIds)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get employee full information (with driving licenses, passports, etc.) 
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetView")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult GetEmployeeView(int id)
        {
            try
            {
                EmployeeDTO employee = repo.GetView(id).ToDTOView();
                return Ok(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get employee for edit (contains only employees data) 
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeeEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEdit", Name = "GetEmployeeEdit")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult GetEmployeeEdit(int id)
        { 
            try
            {
                EmployeeEditDTO employee = repo.GetEdit(id).ToDTOEdit();
                return Ok(employee);
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
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid || id != employee.Id)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeEditDTO dto = repo.Edit(employee).ToDTOEdit();
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
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (employee == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeEditDTO dto = repo.Add(employee).ToDTOEdit();
                return CreatedAtRoute("GetEmployeeEdit", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            try
            {
                EmployeeEditDTO dto = repo.Delete(id).ToDTOEdit();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        //убрать дикую чушь (в репозитории) 
        [HttpGet]
        [Route("GetLists")]
        [ResponseType(typeof(EmployeeList))]
        public IHttpActionResult GetEmployeeLists()
        {
            try
            {
                EmployeeList list = repo.GetList();
                return Ok(list);
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