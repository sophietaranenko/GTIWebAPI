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
    [RoutePrefix("api/OrganizationProperties")]
    public class OrganizationPropertiesController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get employee propertys by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationPropertyDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationPropertyDTO>))]
        public IEnumerable<OrganizationPropertyDTO> GetOrganizationPropertyByOrganizationId(int organizationId)
        {
            List<OrganizationProperty> ptoperties = db.OrganizationProperties
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
            List<OrganizationPropertyDTO> dtos = ptoperties.Select(p => p.ToDTO()).ToList();
            return dtos;
        }

        /// <summary>
        /// Get one property by property id
        /// </summary>
        /// <param name="id">OrganizationProperty id</param>
        /// <returns>OrganizationPropertyEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationProperty")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult GetOrganizationProperty(int id)
        {
            OrganizationProperty property = db.OrganizationProperties.Find(id);
            if (property == null)
            {
                return NotFound();
            }
            OrganizationPropertyDTO dto = property.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee property
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationProperty">OrganizationProperty object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationProperty(int id, OrganizationProperty organizationProperty)
        {
            if (organizationProperty == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationProperty.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationProperty).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationPropertyExists(id))
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
            if (organizationProperty.OrganizationPropertyTypeId != null)
            {
                organizationProperty.OrganizationPropertyType = db.OrganizationPropertyTypes.Find(organizationProperty.OrganizationPropertyTypeId);
            }
            OrganizationPropertyDTO dto = organizationProperty.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee property
        /// </summary>
        /// <param name="organizationProperty">OrganizationProperty object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PostOrganizationProperty(OrganizationProperty organizationProperty)
        {
            if (organizationProperty == null)
            {
                return BadRequest(ModelState);
            }
            organizationProperty.Id = organizationProperty.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.OrganizationProperties.Add(organizationProperty);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationPropertyExists(organizationProperty.Id))
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
            if (organizationProperty.OrganizationPropertyTypeId != null)
            {
                organizationProperty.OrganizationPropertyType = db.OrganizationPropertyTypes.Find(organizationProperty.OrganizationPropertyTypeId);
            }
            OrganizationPropertyDTO dto = organizationProperty.ToDTO();
            return CreatedAtRoute("GetOrganizationProperty", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete property
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationProperty))]
        public IHttpActionResult DeleteOrganizationProperty(int id)
        {
            OrganizationProperty organizationProperty = db.OrganizationProperties.Find(id);
            if (organizationProperty == null)
            {
                return NotFound();
            }
            organizationProperty.Deleted = true;
            db.Entry(organizationProperty).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationPropertyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            OrganizationPropertyDTO dto = organizationProperty.ToDTO();
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

        private bool OrganizationPropertyExists(int id)
        {
            return db.OrganizationProperties.Count(e => e.Id == id) > 0;
        }
    }
}
