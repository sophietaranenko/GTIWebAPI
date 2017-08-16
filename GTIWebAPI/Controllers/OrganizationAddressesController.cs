using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository;
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
        private IDbContextFactory factory;

        public OrganizationAddressesController()
        {
            factory = new DbContextFactory();
        }

        public OrganizationAddressesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationAddressDTO>))]
        public IHttpActionResult GetOrganizationAddressByOrganizationId(int organizationId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<OrganizationAddressDTO> dtos = unitOfWork.OrganizationAddressesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == organizationId, 
                    includeProperties: "Address,Address.Country,OrganizationAddressType,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
                    .Select(d => d.ToDTO());
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationAddressDTO dto = unitOfWork.OrganizationAddressesRepository
               .Get(d => d.Id == id, includeProperties: "Address,Address.Country,OrganizationAddressType,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
               .FirstOrDefault()
               .ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationAddressesRepository.Update(organizationAddress);
                unitOfWork.Save();
                OrganizationAddressDTO dto = unitOfWork.OrganizationAddressesRepository
               .Get(d => d.Id == id, includeProperties: "Address,Address.Country,OrganizationAddressType,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
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

        /// <summary>
        /// Insert new employee address
        /// </summary>
        /// <param name="organizationAddress">OrganizationAddress object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationAddressDTO))]
        public IHttpActionResult PostOrganizationAddress(OrganizationAddressDTO organizationAddress)
        {
            if (organizationAddress == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizationAddress address = organizationAddress.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                address.Id = address.NewId(unitOfWork);
                address.Address.Id = address.Address.NewId(unitOfWork);
                address.AddressId = address.Address.Id;
                unitOfWork.OrganizationAddressesRepository.Insert(address);
                unitOfWork.Save();
                OrganizationAddressDTO dto = unitOfWork.OrganizationAddressesRepository
                    .Get(d => d.Id == address.Id, includeProperties: "Address,Address.Country,OrganizationAddressType,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
                    .FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetOrganizationAddress", new { id = dto.Id }, dto);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationAddress address = unitOfWork.OrganizationAddressesRepository
               .Get(d => d.Id == id, includeProperties: "Address,Address.Country,OrganizationAddressType,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
               .FirstOrDefault();
                address.Deleted = true;
                unitOfWork.OrganizationAddressesRepository.Update(address);
                unitOfWork.Save();
                OrganizationAddressDTO dto = address.ToDTO();
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

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
