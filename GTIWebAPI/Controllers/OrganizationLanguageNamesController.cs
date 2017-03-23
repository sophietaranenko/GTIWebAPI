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
    [RoutePrefix("api/OrganizationLanguageNames")]
    public class OrganizationLanguageNamesController : ApiController
    {
        private IOrganizationRepository<OrganizationLanguageName> repo;

        public OrganizationLanguageNamesController()
        {
            repo = new OrganizationLanguageNamesRepository();
        }

        public OrganizationLanguageNamesController(IOrganizationRepository<OrganizationLanguageName> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationLanguageNameDTO>))]
        public IHttpActionResult GetOrganizationLanguageNameByOrganizationId(int organizationId)
        {
            try
            {
                List<OrganizationLanguageNameDTO> dtos = 
                    repo.GetByOrganizationId(organizationId)
                    .Select(p => p.ToDTO())
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
        [Route("Get", Name = "GetOrganizationLanguageName")]
        [ResponseType(typeof(OrganizationLanguageNameDTO))]
        public IHttpActionResult GetOrganizationLanguageName(int id)
        {
            try
            {
                OrganizationLanguageNameDTO dto = repo.Get(id).ToDTO();
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
        [ResponseType(typeof(OrganizationLanguageNameDTO))]
        public IHttpActionResult PutOrganizationLanguageName(int id, OrganizationLanguageName organizationLanguageName)
        {
            if (organizationLanguageName == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationLanguageName.Id)
            {
                return BadRequest();
            }
            try
            {
                OrganizationLanguageNameDTO dto = repo.Edit(organizationLanguageName).ToDTO();
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
        [ResponseType(typeof(OrganizationLanguageNameDTO))]
        public IHttpActionResult PostOrganizationLanguageName(OrganizationLanguageName organizationLanguageName)
        {
            if (organizationLanguageName == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizationLanguageNameDTO dto = repo.Add(organizationLanguageName).ToDTO();
                return CreatedAtRoute("GetOrganizationLanguageName", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(OrganizationLanguageNameDTO))]
        public IHttpActionResult DeleteOrganizationLanguageName(int id)
        {
            try
            {
                OrganizationLanguageNameDTO dto = repo.Delete(id).ToDTO();
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
