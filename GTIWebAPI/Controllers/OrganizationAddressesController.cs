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
    /// Physic addresses of organization 
    /// </summary>
    [RoutePrefix("api/OrganizationAddresses")]
    public class OrganizationAddressesController : ApiController
    {
        private IOrganizationRepository<OrganizationAddress> repo;

        public OrganizationAddressesController()
        {
            repo = new OrganizationAddressesRepository();
        }

        public OrganizationAddressesController(IOrganizationRepository<OrganizationAddress> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(List<OrganizationAddressDTO>))]
        public IHttpActionResult GetOrganizationAddressByOrganizationId(int organizationId)
        {
            try
            {
                List<OrganizationAddressDTO> dtos = 
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

        /// <summary>
        /// Get one address by address id
        /// </summary>
        /// <param name="id">OrganizationAddress id</param>
        /// <returns>OrganizationAddressEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationAddress")]
        [ResponseType(typeof(OrganizationAddressDTO))]
        public IHttpActionResult GetOrganizationAddress(int id)
        {
            try
            {
                OrganizationAddressDTO dto = repo.Get(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update employee address
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationAddress">OrganizationAddress object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(OrganizationAddressDTO))]
        public IHttpActionResult PutOrganizationAddress(int id, OrganizationAddress organizationAddress)
        {
            if (organizationAddress == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationAddress.Id)
            {
                return BadRequest();
            }
            try
            {
                OrganizationAddressDTO dto = repo.Edit(organizationAddress).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Insert new employee address
        /// </summary>
        /// <param name="organizationAddress">OrganizationAddress object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationAddressDTO))]
        public IHttpActionResult PostOrganizationAddress(OrganizationAddress organizationAddress)
        {
            if (organizationAddress == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizationAddressDTO dto = repo.Add(organizationAddress).ToDTO();
                return CreatedAtRoute("GetOrganizationAddress", new { id = dto.Id }, dto);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationAddressDTO))]
        public IHttpActionResult DeleteOrganizationAddress(int id)
        {
            try
            {
                OrganizationAddressDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool OrganizationAddressExists(int id)
        {
            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                return db.OrganizationAddresses.Count(e => e.Id == id) > 0;
            }
        }
    }
}
