using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        private IDbContextFactory factory;
        private IIdentityHelper helper;

        public TasksController()
        {
            factory = new DbContextFactory();
            helper = new IdentityHelper();
        }

        public TasksController(IDbContextFactory factory, IIdentityHelper helper)
        {
            this.factory = factory;
            this.helper = helper;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAllCurrent")]
        public IHttpActionResult GetAllTasks(int employeeId, string currentDate)
        {
            try
            {
                DateTime date = DateTime.Parse(currentDate);
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<Task> createdTasks = unitOfWork.TasksRepository
                    .Get(d => d.CreatorId == employeeId && d.DateBegin <= date && (d.DateEnd ?? new DateTime(2300, 1, 1)) >= date,
                    includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports");
                IEnumerable<Task> employeeTasks = unitOfWork.TasksRepository
                    .Get(d => d.DoerId == employeeId && d.DateBegin <= date && (d.DateEnd ?? new DateTime(2300, 1, 1)) >= date,
                    includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports");
                return Ok(
                        new
                        {
                            CreatedTasks = createdTasks.Select(d => d.ToDTO()),
                            EmployeeTasks = employeeTasks.Select(d => d.ToDTO())
                        }
                    );
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
        [Route("GetCreator")]
        [ResponseType(typeof(IEnumerable<TaskDTO>))]
        public IHttpActionResult GetCreatorTasks(int employeeId, string currentDate)
        {
            try
            {
                DateTime date = DateTime.Parse(currentDate);
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<Task> createdTasks = unitOfWork.TasksRepository
                    .Get(d => d.CreatorId == employeeId && d.DateBegin <= date && (d.DateEnd ?? new DateTime(2300, 1, 1)) >= date,
                    includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports");
                return Ok(createdTasks.Select(d => d.ToDTO()));
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
        [Route("GetDoer")]
        [ResponseType(typeof(IEnumerable<TaskDTO>))]
        public IHttpActionResult GetDoerTasks(int employeeId, string currentDate)
        {
            try
            {
                DateTime date = DateTime.Parse(currentDate);
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<Task> createdTasks = unitOfWork.TasksRepository
                    .Get(d => d.DoerId == employeeId && d.DateBegin <= date && (d.DateEnd ?? new DateTime(2300, 1, 1)) >= date,
                    includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports");
                return Ok(createdTasks.Select(d => d.ToDTO()));
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
        [Route("Get", Name = "GetTaskById")]
        [ResponseType(typeof(TaskDTO))]
        public IHttpActionResult GetTask(Guid id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                Task task = unitOfWork.TasksRepository
                    .Get(d => d.Id == id,
                    includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports")
                    .FirstOrDefault();
                return Ok(task.ToDTO());
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
        [ResponseType(typeof(TaskDTO))]
        public IHttpActionResult PostTask(TaskDTO taskDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Task task = taskDTO.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                task.Id = Guid.NewGuid();
                unitOfWork.TasksRepository.Insert(task);
                unitOfWork.Save();
                TaskDTO dto = unitOfWork.TasksRepository
                    .Get(d => d.Id == task.Id, includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports")
                    .FirstOrDefault()
                    .ToDTO();
                return CreatedAtRoute("GetTaskById", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(TaskDTO))]
        public IHttpActionResult PutTask(Guid id, TaskDTO taskDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != taskDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                Task task = taskDTO.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.TasksRepository.Update(task);
                unitOfWork.Save();
                TaskDTO dto = unitOfWork.TasksRepository
                    .Get(d => d.Id == task.Id, includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports")
                    .FirstOrDefault()
                    .ToDTO();
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
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(TaskDTO))]
        public IHttpActionResult DeleteTask(Guid id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                TaskDTO dto = unitOfWork.TasksRepository
                     .Get(d => d.Id == id, includeProperties: "Creator,Creator.EmployeePassports,Doer,Doer.EmployeePassports")
                     .FirstOrDefault()
                     .ToDTO();
                unitOfWork.TasksRepository.Delete(id);
                unitOfWork.Save();
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
