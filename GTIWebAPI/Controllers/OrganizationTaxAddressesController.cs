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
    [RoutePrefix("api/OrganizationTaxAddresses")]
    public class OrganizationTaxAddressesController : ApiController
    {

        private IDbContextFactory factory;

        public OrganizationTaxAddressesController()
        {
            factory = new DbContextFactory();
        }

        public OrganizationTaxAddressesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationTaxAddressDTO>))]
        public IHttpActionResult GetOrganizationTaxAddressByOrganizationId(int organizationId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<OrganizationTaxAddressDTO> addresses = unitOfWork.OrganizationTaxAddressesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == organizationId, includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
                    .Select(d => d.ToDTO());
                return Ok(addresses);
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
        [Route("Get", Name = "GetOrganizationTaxAddress")]
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult GetOrganizationTaxAddress(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationTaxAddressDTO dto = unitOfWork.OrganizationTaxAddressesRepository
                    .Get(d => d.Id == id, includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationTaxAddressesRepository.Update(organizationTaxAddress);
                unitOfWork.Save();
                OrganizationTaxAddressDTO dto = unitOfWork.OrganizationTaxAddressesRepository
                    .Get(d => d.Id == id, includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
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
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult PostOrganizationTaxAddress(OrganizationTaxAddress organizationTaxAddress)
        {
            if (organizationTaxAddress == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                organizationTaxAddress.Id = organizationTaxAddress.NewId(unitOfWork);
                organizationTaxAddress.Address.Id = organizationTaxAddress.Address.NewId(unitOfWork);
                organizationTaxAddress.AddressId = organizationTaxAddress.Address.Id;
                unitOfWork.OrganizationTaxAddressesRepository.Insert(organizationTaxAddress);
                unitOfWork.Save();

                OrganizationTaxAddressDTO dto = unitOfWork.OrganizationTaxAddressesRepository
                    .Get(d => d.Id == organizationTaxAddress.Id, includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
                    .FirstOrDefault().ToDTO();

                return CreatedAtRoute("GetOrganizationTaxAddress", new { id = dto.Id }, dto);
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
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult DeleteOrganizationTaxAddress(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                OrganizationTaxAddress address = unitOfWork.OrganizationTaxAddressesRepository
                    .Get(d => d.Id == id, includeProperties: "Address,Address.Country,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage")
                    .FirstOrDefault();

                address.Deleted = true;
                unitOfWork.OrganizationTaxAddressesRepository.Update(address);
                unitOfWork.Save();
                return Ok(address.ToDTO());
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
