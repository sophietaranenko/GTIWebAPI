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
    [RoutePrefix("api/OrganizationTaxTaxAddresses")]
    public class OrganizationTaxAddressesController : ApiController
    {

        private IOrganizationRepository<OrganizationTaxAddress> repo;

        public OrganizationTaxAddressesController()
        {
            repo = new OrganizationTaxAddressesRepository();
        }

        public OrganizationTaxAddressesController(IOrganizationRepository<OrganizationTaxAddress> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationTaxAddressDTO>))]
        public IHttpActionResult GetOrganizationTaxAddressByOrganizationId(int organizationId)
        {
            try
            {
                List<OrganizationTaxAddressDTO> dtos =
                    repo.GetByOrganizationId(organizationId)
                    .Select(p => p.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationTaxAddress")]
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult GetOrganizationTaxAddress(int id)
        {
            try
            {
                OrganizationTaxAddressDTO dto = repo.Get(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult PutOrganizationTaxAddress(int id, OrganizationTaxAddress organizationTaxAddress)
        {
            if (organizationTaxAddress == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationTaxAddress.Id)
            {
                return BadRequest();
            }
            try
            {
                OrganizationTaxAddressDTO dto = repo.Edit(organizationTaxAddress).ToDTO();
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
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult PostOrganizationTaxAddress(OrganizationTaxAddress organizationTaxAddress)
        {
            if (organizationTaxAddress == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizationTaxAddressDTO dto = repo.Add(organizationTaxAddress).ToDTO();
                return CreatedAtRoute("GetOrganizationTaxAddress", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult DeleteOrganizationTaxAddress(int id)
        {
            try
            {
                OrganizationTaxAddressDTO dto = repo.Delete(id).ToDTO();
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
