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
    [RoutePrefix("api/OrganizationProperties")]
    public class OrganizationPropertiesController : ApiController
    {
        private IOrganizationRepository<OrganizationProperty> repo;

        public OrganizationPropertiesController()
        {
            repo = new OrganizationPropertiesRepository();
        }

        public OrganizationPropertiesController(IOrganizationRepository<OrganizationProperty> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(List<OrganizationPropertyDTO>))]
        public IHttpActionResult GetOrganizationPropertyByOrganizationId(int organizationId)
        {
            try
            {
                List<OrganizationPropertyDTO> dtos =
                    repo.GetByOrganizationId(organizationId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationProperty")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult GetOrganizationProperty(int id)
        {
            try
            {
                OrganizationPropertyDTO dto = repo.Get(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PutOrganizationProperty(int id, OrganizationProperty organizationProperty)
        {
            if (organizationProperty == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationProperty.Id)
            {
                return BadRequest();
            }
            try
            {
                OrganizationPropertyDTO dto = repo.Edit(organizationProperty).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

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
                OrganizationPropertyDTO dto = repo.Add(organizationProperty).ToDTO();
                return CreatedAtRoute("GetOrganizationProperty", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPost]
        [Route("PostConstant")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PostArrayOrganizationProperty(IEnumerable<OrganizationPropertyConstant> properties)
        {
            if (properties == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            List<OrganizationPropertyDTO> propertiesToReturn = new List<OrganizationPropertyDTO>();
            try
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
                        OrganizationPropertyDTO dto = repo.Add(organizationProperty).ToDTO();
                        propertiesToReturn.Add(dto);
                    }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok(propertiesToReturn);
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult DeleteOrganizationProperty(int id)
        {
            try
            {
                OrganizationPropertyDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
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
