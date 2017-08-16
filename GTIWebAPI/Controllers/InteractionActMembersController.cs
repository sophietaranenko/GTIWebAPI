using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/InteractionActMembers")]
    public class InteractionActMembersController : ApiController
    {
        private IDbContextFactory factory;

        public InteractionActMembersController()
        {
            factory = new DbContextFactory();
        }

        public InteractionActMembersController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByInteractionActId")]
        [ResponseType(typeof(IEnumerable<InteractionActMemberDTO>))]
        public IHttpActionResult GetAllInteractionActMemberByInteractionActId(int interactionActId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionActMemberDTO> dtos = unitOfWork.InteractionActMembersRepository.Get(d => d.InteractionActId == interactionActId,
                    includeProperties: "Employee,Employee.EmployeePassports").Select(d => d.ToDTO());
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

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetInteractionActMember")]
        [ResponseType(typeof(InteractionActMemberDTO))]
        public IHttpActionResult GetInteractionActMember(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionActMemberDTO dto = unitOfWork.InteractionActMembersRepository.Get(d => d.Id == id, includeProperties: "Employee,Employee.EmployeePassports").FirstOrDefault().ToDTO();
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
        [ResponseType(typeof(InteractionActMemberDTO))]
        public IHttpActionResult PostInteractionActMember(InteractionActMemberDTO interactionActMember)
        {
            if (interactionActMember == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                InteractionActMember member = interactionActMember.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionActMembersRepository.Insert(member);
                unitOfWork.Save();
                InteractionActMemberDTO dto = unitOfWork.InteractionActMembersRepository.Get(d => d.Id == interactionActMember.Id, includeProperties: "Employee,Employee.EmployeePassports").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetInteractionActMember", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(IEnumerable<InteractionActMemberDTO>))]
        public IHttpActionResult PutInteractionActMember(int id, InteractionActMemberDTO interactionActMember)
        {
            if (interactionActMember == null || id != interactionActMember.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                InteractionActMember member = interactionActMember.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionActMembersRepository.Update(member);
                unitOfWork.Save();
                InteractionActMemberDTO dto = unitOfWork.InteractionActMembersRepository.Get(d => d.Id == id, includeProperties: "Employee,Employee.EmployeePassports").FirstOrDefault().ToDTO();
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
        [ResponseType(typeof(IEnumerable<InteractionActMemberDTO>))]
        public IHttpActionResult DeleteInteractionActMember(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionActMemberDTO dto = unitOfWork.InteractionActMembersRepository.Get(d => d.Id == id, includeProperties: "Employee,Employee.EmployeePassports").FirstOrDefault().ToDTO();
                unitOfWork.InteractionActMembersRepository.Delete(id);
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
