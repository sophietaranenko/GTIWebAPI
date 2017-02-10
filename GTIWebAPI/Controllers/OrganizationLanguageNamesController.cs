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
    [RoutePrefix("api/OrganizationLanguageNames")]
    public class OrganizationLanguageNamesController : ApiController
    {

        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get employee languageNames by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationLanguageNameDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationLanguageNameDTO>))]
        public IEnumerable<OrganizationLanguageNameDTO> GetOrganizationLanguageNameByOrganizationId(int organizationId)
        {
            List<OrganizationLanguageName> languageNames = db.OrganizationLanguageNames
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
            List<OrganizationLanguageNameDTO> dtos = languageNames.Select(p => p.ToDTO()).ToList();
            return dtos;
        }

        /// <summary>
        /// Get one languageName by languageName id
        /// </summary>
        /// <param name="id">OrganizationLanguageName id</param>
        /// <returns>OrganizationLanguageNameEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationLanguageName")]
        [ResponseType(typeof(OrganizationLanguageNameDTO))]
        public IHttpActionResult GetOrganizationLanguageName(int id)
        {
            OrganizationLanguageName languageName = db.OrganizationLanguageNames.Find(id);
            if (languageName == null)
            {
                return NotFound();
            }
            OrganizationLanguageNameDTO dto = languageName.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee languageName
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationLanguageName">OrganizationLanguageName object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
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
            db.Entry(organizationLanguageName).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationLanguageNameExists(id))
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

            if (organizationLanguageName.LanguageId != null)
            {
                organizationLanguageName.Language = db.Languages.Find(organizationLanguageName.LanguageId);
            }
            OrganizationLanguageNameDTO dto = organizationLanguageName.ToDTO();

            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee languageName
        /// </summary>
        /// <param name="organizationLanguageName">OrganizationLanguageName object</param>
        /// <returns></returns>
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
            organizationLanguageName.Id = organizationLanguageName.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.OrganizationLanguageNames.Add(organizationLanguageName);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationLanguageNameExists(organizationLanguageName.Id))
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


            if (organizationLanguageName.LanguageId != null)
            {
                organizationLanguageName.Language = db.Languages.Find(organizationLanguageName.LanguageId);
            }
            OrganizationLanguageNameDTO dto = organizationLanguageName.ToDTO();
            return CreatedAtRoute("GetOrganizationLanguageName", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete languageName
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationLanguageName))]
        public IHttpActionResult DeleteOrganizationLanguageName(int id)
        {
            OrganizationLanguageName organizationLanguageName = db.OrganizationLanguageNames.Find(id);
            if (organizationLanguageName == null)
            {
                return NotFound();
            }
            organizationLanguageName.Deleted = true;
            db.Entry(organizationLanguageName).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationLanguageNameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            OrganizationLanguageNameDTO dto = organizationLanguageName.ToDTO();
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

        private bool OrganizationLanguageNameExists(int id)
        {
            return db.OrganizationLanguageNames.Count(e => e.Id == id) > 0;
        }

    }
}
