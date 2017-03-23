using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository.Organization;
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
    /// Contact persons are pepople we contact to 
    /// </summary>
    [RoutePrefix("api/OrganizationContactPersons")]
    public class OrganizationContactPersonsController : ApiController
    {
        private IOrganizationContactPersonsRepository repo;

        public OrganizationContactPersonsController()
        {
            repo = new OrganizationContactPersonsRepository();
        }

        public OrganizationContactPersonsController(IOrganizationContactPersonsRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(List<OrganizationContactPersonDTO>))]
        public IHttpActionResult GetOrganizationContactPersonByOrganizationId(int organizationId)
        {
            try
            {
                List<OrganizationContactPersonDTO> dtos = repo.GetByOrganizationId(organizationId)
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

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationContactPerson")]
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult GetOrganizationContactPerson(int id)
        {
            try
            {
                OrganizationContactPersonDTO dto = repo.Get(id).ToDTO();
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
                OrganizationContactPersonDTO dto = repo.Edit(organizationContactPerson).ToDTO();
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
        [ResponseType(typeof(OrganizationContactPersonDTO))]
        public IHttpActionResult PostOrganizationContactPerson(OrganizationContactPerson organizationContactPerson)
        {
            if (organizationContactPerson == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizationContactPersonDTO dto = repo.Add(organizationContactPerson).ToDTO();
                return CreatedAtRoute("GetOrganizationContactPerson", new { id = dto.Id }, dto);
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
                OrganizationContactPersonDTO dto = repo.Delete(id).ToDTO();
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
