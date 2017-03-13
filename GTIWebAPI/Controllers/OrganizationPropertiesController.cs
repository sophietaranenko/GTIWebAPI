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
        /// <summary>
        /// Get employee propertys by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationPropertyDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationPropertyDTO>))]
        public IHttpActionResult GetOrganizationPropertyByOrganizationId(int organizationId)
        {
            List<OrganizationProperty> properties = new List<OrganizationProperty>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    properties = db.OrganizationProperties
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                    .Include(d => d.OrganizationPropertyType)
                    .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<OrganizationPropertyDTO> dtos = properties.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
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
            OrganizationProperty organizationProperty = new OrganizationProperty();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationProperty = db.OrganizationProperties.Find(id);
                    if (organizationProperty != null)
                    {
                        db.Entry(organizationProperty).Reference(d => d.OrganizationPropertyType).Load();
                        if (organizationProperty.OrganizationPropertyType != null)
                        {
                            db.Entry(organizationProperty.OrganizationPropertyType).Reference(d => d.OrganizationPropertyTypeAlias).Load();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (organizationProperty == null)
            {
                return NotFound();
            }
            OrganizationPropertyDTO dto = organizationProperty.ToDTO();
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

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {

                    OrganizationPropertyType propertyType = db.OrganizationPropertyTypes.Find(organizationProperty.OrganizationPropertyTypeId);
                    int? propertyCountryId = propertyType.CountryId;
                    Organization organization = db.Organizations.Find(organizationProperty.OrganizationId);
                    int? organizationCountryId = organization.CountryId;
                    if (propertyCountryId != organizationCountryId)
                    {
                        return BadRequest("Country that property belogs to, doesn't match the Organization registration country");
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
                    if (organizationProperty != null)
                    {
                        db.Entry(organizationProperty).Reference(d => d.OrganizationPropertyType).Load();
                        if (organizationProperty.OrganizationPropertyType != null)
                        {
                            db.Entry(organizationProperty.OrganizationPropertyType).Reference(d => d.OrganizationPropertyTypeAlias).Load();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
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

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    OrganizationPropertyType propertyType = db.OrganizationPropertyTypes.Find(organizationProperty.OrganizationPropertyTypeId);
                    int? propertyCountryId = propertyType.CountryId;
                    Organization organization = db.Organizations.Find(organizationProperty.OrganizationId);
                    int? organizationCountryId = organization.CountryId;
                    if (propertyCountryId != organizationCountryId)
                    {
                        return BadRequest("Country that property belogs to, doesn't match the Organization registration country");
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

                    if (organizationProperty != null)
                    {
                        db.Entry(organizationProperty).Reference(d => d.OrganizationPropertyType).Load();
                        if (organizationProperty.OrganizationPropertyType != null)
                        {
                            db.Entry(organizationProperty.OrganizationPropertyType).Reference(d => d.OrganizationPropertyTypeAlias).Load();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            OrganizationPropertyDTO dto = organizationProperty.ToDTO();
            return CreatedAtRoute("GetOrganizationProperty", new { id = dto.Id }, dto);
        }

        [GTIFilter]
        [HttpPost]
        [Route("PostConstant")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PostArrayOrganizationProperty(IEnumerable<OrganizationPropertyConstant> properties)
        {
            if (properties == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<OrganizationPropertyDTO> propertiesToReturn = new List<OrganizationPropertyDTO>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    foreach (var item in properties)
                    {
                        OrganizationProperty organizationProperty = new OrganizationProperty()
                        {
                            DateBegin = null,
                            DateEnd = null,
                            Deleted = null,
                            OrganizationId = item.OrganizationId,
                            OrganizationPropertyTypeId = item.OrganizationPropertyTypeId,
                            Value = item.Value
                        };
                        organizationProperty.Id = organizationProperty.NewId(db);
                        db.OrganizationProperties.Add(organizationProperty);


                        if (organizationProperty != null)
                        {
                            db.Entry(organizationProperty).Reference(d => d.OrganizationPropertyType).Load();
                            if (organizationProperty.OrganizationPropertyType != null)
                            {
                                db.Entry(organizationProperty.OrganizationPropertyType).Reference(d => d.OrganizationPropertyTypeAlias).Load();
                            }
                        }
                        OrganizationPropertyDTO dto = organizationProperty.ToDTO();
                        propertiesToReturn.Add(dto);
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok(propertiesToReturn);
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
            OrganizationProperty organizationProperty = new OrganizationProperty();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationProperty = db.OrganizationProperties.Find(id);
                    if (organizationProperty == null)
                    {
                        return NotFound();
                    }

                    db.Entry(organizationProperty).Reference(d => d.OrganizationPropertyType).Load();
                    if (organizationProperty.OrganizationPropertyType != null)
                    {
                        db.Entry(organizationProperty.OrganizationPropertyType).Reference(d => d.OrganizationPropertyTypeAlias).Load();
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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
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
            base.Dispose(disposing);
        }

        private bool OrganizationPropertyExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.OrganizationProperties.Count(e => e.Id == id) > 0;
            }
        }
    }
}
