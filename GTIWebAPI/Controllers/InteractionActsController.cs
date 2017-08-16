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

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/InteractionActs")]
    public class InteractionActsController : ApiController
    {
        private IDbContextFactory factory;

        public InteractionActsController()
        {
            factory = new DbContextFactory();
        }

        public InteractionActsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByInteractionId")]
        // [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetAllInteractionActByInteractionId(int interactionId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionActDTO> dtos = unitOfWork
                    .InteractionActsRepository
                    .Get(d => d.InteractionId == interactionId, includeProperties: "Act,InteractionActMembers,InteractionActMembers.Employee,InteractionActOrganizationMembers,InteractionActOrganizationMembers.OrganizationContactPerson")
                    .Select(d => d.ToDTO());
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
        [Route("Get", Name = "GetInteractionAct")]
        // [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetInteractionAct(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionActDTO dto = unitOfWork
                    .InteractionActsRepository
                    .Get(d => d.Id == id, includeProperties: "Act,InteractionActMembers,InteractionActMembers.Employee,InteractionActOrganizationMembers,InteractionActOrganizationMembers.OrganizationContactPerson")
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
        [HttpPost]
        [Route("Post")]
        // [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult PostInteractionAct(InteractionActDTO interactionActDTO)
        {
            if (interactionActDTO == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                InteractionAct interactionAct = interactionActDTO.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionActsRepository.Insert(interactionAct);
                unitOfWork.Save();
                InteractionActDTO dto = unitOfWork
                    .InteractionActsRepository
                    .Get(d => d.Id == interactionActDTO.Id, includeProperties: "Act,InteractionActMembers,InteractionActMembers.Employee,InteractionActOrganizationMembers,InteractionActOrganizationMembers.OrganizationContactPerson")
                    .FirstOrDefault()
                    .ToDTO();
                return CreatedAtRoute("GetInteractionAct", new { id = dto.Id }, dto);
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
        // [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult PutInteractionAct(int id, InteractionActDTO interactionActDTO)
        {
            if (interactionActDTO == null || id != interactionActDTO.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                InteractionAct interactionAct = interactionActDTO.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.InteractionActsRepository.Update(interactionAct);
                unitOfWork.Save();
                InteractionActDTO dto = unitOfWork
                    .InteractionActsRepository
                    .Get(d => d.Id == interactionActDTO.Id, includeProperties: "Act,InteractionActMembers,InteractionActMembers.Employee,InteractionActOrganizationMembers,InteractionActOrganizationMembers.OrganizationContactPerson")
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
        // [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult DeleteInteractionAct(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionActDTO dto = unitOfWork
                    .InteractionActsRepository
                    .Get(d => d.Id == id, includeProperties: "Act,InteractionActMembers,InteractionActMembers.Employee,InteractionActOrganizationMembers,InteractionActOrganizationMembers.OrganizationContactPerson")
                    .FirstOrDefault()
                    .ToDTO();
                unitOfWork.InteractionActsRepository.Delete(id);
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
