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
    [RoutePrefix("api/InteractionActOrganizationMembers")]
    public class InteractionActOrganizationMembersController : ApiController
    {
        private IDbContextFactory factory;

        public InteractionActOrganizationMembersController()
        {
            factory = new DbContextFactory();
        }

        public InteractionActOrganizationMembersController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByInteractionActId")]
        [ResponseType(typeof(IEnumerable<InteractionActOrganizationMemberDTO>))]
        public IHttpActionResult GetAllInteractionActOrganizationMemberByInteractionActId(int interactionActId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionActOrganizationMemberDTO> dtos = unitOfWork.InteractionActOrganizationMembersRepository.Get(d => d.InteractionActId == interactionActId, 
                    includeProperties: "OrganizationContactPerson").Select(d => d.ToDTO());
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
        [Route("Get", Name = "GetInteractionActOrganizationMember")]
        [ResponseType(typeof(InteractionActOrganizationMemberDTO))]
        public IHttpActionResult GetInteractionActOrganizationMember(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionActOrganizationMemberDTO dto = unitOfWork.InteractionActOrganizationMembersRepository.Get(d => d.Id == id, includeProperties: "OrganizationContactPerson").FirstOrDefault().ToDTO();
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
        [ResponseType(typeof(InteractionActOrganizationMemberDTO))]
        public IHttpActionResult PostInteractionActOrganizationMember(InteractionActOrganizationMemberDTO interactionActMemberDTO)
        {
            if (interactionActMemberDTO == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                InteractionActOrganizationMember interactionActMember = interactionActMemberDTO.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionActOrganizationMembersRepository.Insert(interactionActMember);
                unitOfWork.Save();
                InteractionActOrganizationMemberDTO dto = unitOfWork.InteractionActOrganizationMembersRepository.Get(d => d.Id == interactionActMemberDTO.Id, 
                    includeProperties: "OrganizationContactPerson").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetInteractionActOrganizationMember", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(IEnumerable<InteractionActOrganizationMemberDTO>))]
        public IHttpActionResult PutInteractionActOrganizationMember(int id, InteractionActOrganizationMemberDTO interactionActMemberDTO)
        {
            if (interactionActMemberDTO == null || id != interactionActMemberDTO.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                InteractionActOrganizationMember interactionActMember = interactionActMemberDTO.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionActOrganizationMembersRepository.Update(interactionActMember);
                unitOfWork.Save();
                InteractionActOrganizationMemberDTO dto = unitOfWork.InteractionActOrganizationMembersRepository.Get(d => d.Id == id, 
                    includeProperties: "OrganizationContactPerson").FirstOrDefault().ToDTO();
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
        [ResponseType(typeof(IEnumerable<InteractionActOrganizationMemberDTO>))]
        public IHttpActionResult DeleteInteractionActOrganizationMember(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionActOrganizationMemberDTO dto = unitOfWork.InteractionActOrganizationMembersRepository.Get(d => d.Id == id, 
                    includeProperties: "OrganizationContactPerson").FirstOrDefault().ToDTO();
                unitOfWork.InteractionActOrganizationMembersRepository.Delete(id);
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
