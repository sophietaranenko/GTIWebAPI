using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Contact persons are pepople we contact to 
    /// </summary>
    [RoutePrefix("api/OrganizationContactPersons")]
    public class OrganizationContactPersonsController : ApiController
    {
        private IDbContextFactory factory;

        public OrganizationContactPersonsController()
        {
            factory = new DbContextFactory();
        }

        public OrganizationContactPersonsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactPersonDTO>))]
        public IHttpActionResult GetOrganizationContactPersonByOrganizationId(int organizationId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<OrganizationContactPersonDTO> persons = unitOfWork.OrganizationContactPersonsViewRepository.Get(d => d.Deleted != true && d.OrganizationId == organizationId).Select(d => d.ToDTO());
                if (persons != null)
                {
                    foreach (var person in persons)
                    {
                        person.OrganizationContactPersonContact = unitOfWork.OrganizationContactPersonContactsRepository
                            .Get(d => d.Deleted != true && d.OrganizationContactPersonId == person.Id, includeProperties: "ContactType")
                            .Select(d => d.ToDTO());
                    }
                }
                return Ok(persons);
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
        [Route("Get", Name = "GetOrganizationContactPerson")]
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult GetOrganizationContactPerson(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationContactPersonDTO person = unitOfWork.OrganizationContactPersonsViewRepository
                    .Get(d => d.Id == id).FirstOrDefault().ToDTO();
                if (person == null)
                {
                    return NotFound();
                }
                person.OrganizationContactPersonContact = unitOfWork.OrganizationContactPersonContactsRepository
            .Get(d => d.Deleted != true && d.OrganizationContactPersonId == id, includeProperties: "ContactType")
            .Select(d => d.ToDTO());
                return Ok(person);
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
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult PutOrganizationContactPerson(int id, OrganizationContactPerson organizationContactPerson)
        {
            if (organizationContactPerson == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationContactPerson.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationContactPersonsRepository.Update(organizationContactPerson);
                unitOfWork.Save();
                OrganizationContactPersonDTO person = unitOfWork.OrganizationContactPersonsViewRepository
                    .Get(d => d.Id == id).FirstOrDefault().ToDTO();
                if (person == null)
                {
                    return NotFound();
                }
                person.OrganizationContactPersonContact = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Deleted != true && d.OrganizationContactPersonId == id, includeProperties: "ContactType")
                    .Select(d => d.ToDTO());
                return Ok(person);
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
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult PostOrganizationContactPerson(OrganizationContactPerson organizationContactPerson)
        {
            if (organizationContactPerson == null)
            {
                return BadRequest(ModelState);
            }
            try
            {

                UnitOfWork unitOfWork = new UnitOfWork(factory);
                organizationContactPerson.Id = organizationContactPerson.NewId(unitOfWork);
                foreach (var item in organizationContactPerson.OrganizationContactPersonContact)
                {
                    item.Id = item.NewId(unitOfWork);
                }
                unitOfWork.OrganizationContactPersonsRepository.Insert(organizationContactPerson);
                unitOfWork.Save();

                OrganizationContactPersonDTO person = unitOfWork.OrganizationContactPersonsViewRepository
                    .Get(d => d.Id == organizationContactPerson.Id).FirstOrDefault().ToDTO();
                if (person == null)
                {
                    return NotFound();
                }
                person.OrganizationContactPersonContact = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.Deleted != true && d.OrganizationContactPersonId == organizationContactPerson.Id, includeProperties: "ContactType")
                    .Select(d => d.ToDTO());
                return CreatedAtRoute("GetOrganizationContactPerson", new { id = person.Id }, person);
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
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult DeleteOrganizationContactPerson(int id)
        {
            OrganizationContactPerson organizationContactPerson = new OrganizationContactPerson();
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationContactPerson person = unitOfWork.OrganizationContactPersonsRepository
                    .Get(d => d.Id == id).FirstOrDefault();
                person.Deleted = true;
                unitOfWork.OrganizationContactPersonsRepository.Update(person);
                unitOfWork.Save();
                return Ok(person.ToDTO());
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
