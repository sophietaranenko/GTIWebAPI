using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

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
        [ResponseType(typeof(List<EmployeeViewDTO>))]
        public IHttpActionResult GetEmployeeAll(string officeIds)
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            List<EmployeeView> employeeList = new List<EmployeeView>();
            try
            {
                List<EmployeeViewDTO> dtos = 
                    repo.GetAll(OfficeIds)
                    .Select(d => d.ToDTO())
                    .ToList();
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
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employee.Id)
            {
                return BadRequest();
            }
            try
            {
                EmployeeEditDTO dto = repo.Edit(employee).ToDTOEdit();
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
        [ResponseType(typeof(EmployeeEditDTO))]
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
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            try
            {
                EmployeeEditDTO dto = repo.Delete(id).ToDTOEdit();
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