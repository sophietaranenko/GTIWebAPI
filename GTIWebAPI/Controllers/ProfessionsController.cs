using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/Professions")]
    public class ProfessionsController : ApiController
    {
        IDbContextFactory factory;

        public ProfessionsController()
        {
            factory = new DbContextFactory();
        }

        public ProfessionsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<ProfessionDTO>))]
        public IHttpActionResult GetProfessionAll()
        {
            try
            {
                UnitOfWork unitOFWork = new UnitOfWork(factory);
                IEnumerable<ProfessionDTO> professions = unitOFWork.ProfessionsRepository
                    .Get(d => d.Deleted != true, includeProperties: "Country")
                    .Select(d => d.ToDTO());
                return Ok(professions);
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
        [Route("GetByCountryId")]
        [ResponseType(typeof(IEnumerable<ProfessionDTO>))]
        public IHttpActionResult GetProfessionByCountryId(int countryId)
        {
            try
            {
                UnitOfWork unitOFWork = new UnitOfWork(factory);
                IEnumerable<ProfessionDTO> professions = unitOFWork.ProfessionsRepository
                    .Get(d => d.Deleted != true && d.CountryId == countryId, includeProperties: "Country")
                    .Select(d => d.ToDTO());
                return Ok(professions);
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
        [Route("GetByOfficeId")]
        [ResponseType(typeof(IEnumerable<ProfessionDTO>))]
        public IHttpActionResult GetProfessionByOfficeId(int officeId)
        {
            try
            {
                UnitOfWork unitOFWork = new UnitOfWork(factory);
                int countryId = unitOFWork.OfficesRepository.Get(d => d.Id == officeId).Select(d => d.CountryId).FirstOrDefault();
                if (countryId == null || countryId == 0)
                {
                    return BadRequest("Empty country");
                }
                IEnumerable<ProfessionDTO> professions = unitOFWork.ProfessionsRepository
                    .Get(d => d.Deleted != true && d.CountryId == countryId, includeProperties: "Country")
                    .Select(d => d.ToDTO());
                return Ok(professions);
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
        [Route("Get", Name = "GetProfession")]
        [ResponseType(typeof(ProfessionDTO))]
        public IHttpActionResult GetProfessionById(int id)
        {
            try
            {
                UnitOfWork unitOFWork = new UnitOfWork(factory);
                IEnumerable<ProfessionDTO> professions = unitOFWork.ProfessionsRepository
                    .Get(d => d.Id == id, includeProperties: "Country")
                    .Select(d => d.ToDTO());
                return Ok(professions);
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
        [ResponseType(typeof(ProfessionDTO))]
        public IHttpActionResult PutProfession(int id, ProfessionDTO profession)
        {
            if (profession == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != profession.Id)
            {
                return BadRequest();
            }
            try
            {
                Profession prof = profession.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.ProfessionsRepository.Update(prof);
                unitOfWork.Save();
                ProfessionDTO dto = unitOfWork.ProfessionsRepository
               .Get(d => d.Id == id, includeProperties: "Country")
               .FirstOrDefault().ToDTO();
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
        [ResponseType(typeof(ProfessionDTO))]
        public IHttpActionResult PostProfession(ProfessionDTO profession)
        {
            if (profession == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Profession prof = profession.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                prof.Id = prof.NewId(unitOfWork);
                unitOfWork.ProfessionsRepository.Insert(prof);
                unitOfWork.Save();
                ProfessionDTO dto = unitOfWork.ProfessionsRepository
                    .Get(d => d.Id == prof.Id, includeProperties: "Country")
                    .FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetProfession", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(ProfessionDTO))]
        public IHttpActionResult DeleteProfession(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                Profession profession = unitOfWork.ProfessionsRepository
               .Get(d => d.Id == id, includeProperties: "Country")
               .FirstOrDefault();
                profession.Deleted = true;
                unitOfWork.ProfessionsRepository.Update(profession);
                unitOfWork.Save();
                ProfessionDTO dto = profession.ToDTO();
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

        [HttpGet]
        [Route("GetCountries")]
        [ResponseType(typeof(IEnumerable<CountryDTO>))]
        public IHttpActionResult GetCountriesForProfession()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                return Ok(unitOfWork.GetCountries());
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
