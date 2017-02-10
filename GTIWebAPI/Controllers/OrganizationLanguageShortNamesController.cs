using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
    [RoutePrefix("api/OrganizationLanguageShortNames")]
    public class OrganizationLanguageShortNamesController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get employee languageNames by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationLanguageShortNameDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationLanguageShortNameDTO>))]
        public IEnumerable<OrganizationLanguageShortNameDTO> GetOrganizationLanguageShortNameByOrganizationId(int organizationId)
        {
            List<OrganizationLanguageShortName> languageNames = db.OrganizationLanguageShortNames
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
            List<OrganizationLanguageShortNameDTO> dtos = languageNames.Select(p => p.ToDTO()).ToList();
            return dtos;
        }

        /// <summary>
        /// Get one languageName by languageName id
        /// </summary>
        /// <param name="id">OrganizationLanguageShortName id</param>
        /// <returns>OrganizationLanguageShortNameEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationLanguageShortName")]
        [ResponseType(typeof(OrganizationLanguageShortNameDTO))]
        public IHttpActionResult GetOrganizationLanguageShortName(int id)
        {
            OrganizationLanguageShortName languageName = db.OrganizationLanguageShortNames.Find(id);
            if (languageName == null)
            {
                return NotFound();
            }
            OrganizationLanguageShortNameDTO dto = languageName.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee languageName
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationShortLanguageName">OrganizationLanguageShortName object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationLanguageShortName(int id, OrganizationLanguageShortName organizationShortLanguageName)
        {
            if (organizationShortLanguageName == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationShortLanguageName.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationShortLanguageName).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationLanguageShortNameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually

            if (organizationShortLanguageName.LanguageId != null)
            {
                organizationShortLanguageName.Language = db.Languages.Find(organizationShortLanguageName.LanguageId);
            }
            OrganizationLanguageShortNameDTO dto = organizationShortLanguageName.ToDTO();

            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee languageName
        /// </summary>
        /// <param name="organizationShortLanguageName">OrganizationLanguageShortName object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationLanguageShortNameDTO))]
        public IHttpActionResult PostOrganizationLanguageShortName(OrganizationLanguageShortName organizationShortLanguageName)
        {
            if (organizationShortLanguageName == null)
            {
                return BadRequest(ModelState);
            }
            organizationShortLanguageName.Id = organizationShortLanguageName.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.OrganizationLanguageShortNames.Add(organizationShortLanguageName);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationLanguageShortNameExists(organizationShortLanguageName.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually


            if (organizationShortLanguageName.LanguageId != null)
            {
                organizationShortLanguageName.Language = db.Languages.Find(organizationShortLanguageName.LanguageId);
            }
            OrganizationLanguageShortNameDTO dto = organizationShortLanguageName.ToDTO();
            return CreatedAtRoute("GetOrganizationLanguageShortName", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete languageName
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationLanguageShortName))]
        public IHttpActionResult DeleteOrganizationLanguageShortName(int id)
        {
            OrganizationLanguageShortName organizationShortLanguageName = db.OrganizationLanguageShortNames.Find(id);
            if (organizationShortLanguageName == null)
            {
                return NotFound();
            }
            organizationShortLanguageName.Deleted = true;
            db.Entry(organizationShortLanguageName).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationLanguageShortNameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            OrganizationLanguageShortNameDTO dto = organizationShortLanguageName.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrganizationLanguageShortNameExists(int id)
        {
            return db.OrganizationLanguageShortNames.Count(e => e.Id == id) > 0;
        }
    }
}
