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
    [RoutePrefix("api/OrganizationLanguageNames")]
    public class OrganizationLanguageNamesController : ApiController
    {
        private IDbContextFactory factory;
        public OrganizationLanguageNamesController()
        {
            factory = new DbContextFactory();
        }

        public OrganizationLanguageNamesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationLanguageNameDTO>))]
        public IHttpActionResult GetOrganizationLanguageNameByOrganizationId(int organizationId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<OrganizationLanguageNameDTO> names = unitOfWork.OrganizationLanguageNamesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == organizationId, includeProperties: "Language")
                    .Select(d => d.ToDTO());
                return Ok(names);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationLanguageNameDTO name = unitOfWork.OrganizationLanguageNamesRepository
                    .Get(d => d.Id == id, includeProperties: "Language").FirstOrDefault().ToDTO();
                return Ok(name);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationLanguageNamesRepository.Update(organizationLanguageName);
                unitOfWork.Save();
                OrganizationLanguageNameDTO dto = unitOfWork.OrganizationLanguageNamesRepository
                    .Get(d => d.Id == id, includeProperties: "Language").FirstOrDefault().ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                organizationLanguageName.Id = organizationLanguageName.NewId(unitOfWork);
                unitOfWork.OrganizationLanguageNamesRepository.Insert(organizationLanguageName);
                unitOfWork.Save();
                OrganizationLanguageNameDTO dto = unitOfWork.OrganizationLanguageNamesRepository
                    .Get(d => d.Id == organizationLanguageName.Id, includeProperties: "Language").FirstOrDefault().ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationLanguageName name = unitOfWork.OrganizationLanguageNamesRepository
                    .Get(d => d.Id == id, includeProperties: "Language").FirstOrDefault();
                name.Deleted = true;
                unitOfWork.OrganizationLanguageNamesRepository.Update(name);
                unitOfWork.Save();
                return Ok(name.ToDTO());
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
