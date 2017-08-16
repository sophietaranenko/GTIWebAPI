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
    [RoutePrefix("api/InteractionMembers")]
    public class InteractionMembersController : ApiController
    {
        private IDbContextFactory factory;
        private IIdentityHelper helper;

        public InteractionMembersController()
        {
            factory = new DbContextFactory();
            helper = new IdentityHelper();
        }

        public InteractionMembersController(IDbContextFactory factory, IIdentityHelper helper)
        {
            this.factory = factory;
            this.helper = helper;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByInteractionId")]
        [ResponseType(typeof(IEnumerable<InteractionDTO>))]
        public IHttpActionResult GetInteractionMembersByInteractionId(int interactionId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<InteractionMemberDTO> members =
                    unitOfWork.InteractionMembersRepository.Get(d => d.InteractionId == interactionId, includeProperties: "Employee,Employee.EmployeePassports").Select(d => d.ToDTO());
                return Ok(members);
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
        [Route("Get",Name = "GetInteractionMemberById")]
        [ResponseType(typeof(InteractionDTO))]
        public IHttpActionResult GetInteractionMembersById(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionMemberDTO member =
                    unitOfWork.InteractionMembersRepository.Get(d => d.Id == id, includeProperties: "Employee,Employee.EmployeePassports").FirstOrDefault().ToDTO();
                return Ok(member);
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
        [ResponseType(typeof(InteractionDTO))]
        public IHttpActionResult GetInteractionMembersByInteractionId(InteractionMemberDTO member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                InteractionMember oMember = member.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);


                int count = unitOfWork.InteractionMembersRepository.Get(d => d.InteractionId == member.InteractionId && d.EmployeeId == member.EmployeeId).Count();

                if (count > 0)
                {
                    return BadRequest("This employee already takes part in this interaction");
                }

                unitOfWork.InteractionMembersRepository.Insert(oMember);
                unitOfWork.Save();



                InteractionMemberDTO dto =
                    unitOfWork.InteractionMembersRepository.Get(d => d.Id == oMember.Id, includeProperties: "Employee,Employee.EmployeePassports")
                    .FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetInteractionMemberById", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(InteractionDTO))]
        public IHttpActionResult DeleteInteractionMember(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                InteractionMemberDTO dto =
                   unitOfWork.InteractionMembersRepository.Get(d => d.Id == id, includeProperties: "Employee,Employee.EmployeePassports")
                   .FirstOrDefault().ToDTO();
                unitOfWork.InteractionMembersRepository.Delete(id);
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
