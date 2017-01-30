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
    [RoutePrefix("api/OrganizationAddresses")]
    public class OrganizationAddressesController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get employee addresss by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationAddressDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationAddressDTO>))]
        public IEnumerable<OrganizationAddressDTO> GetOrganizationAddressByOrganizationId(int organizationId)
        {
            List<OrganizationAddress> addresses = db.OrganizationAddresses
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
            List<OrganizationAddressDTO> dtos = addresses.Select(p => p.ToDTO()).ToList();
            return dtos;
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
            OrganizationAddress address = db.OrganizationAddresses.Find(id);
            if (address == null)
            {
                return NotFound();
            }
            OrganizationAddressDTO dto = address.ToDTO();
            return Ok(dto);
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationAddress(int id, OrganizationAddress organizationAddress)
        {
            if (organizationAddress == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationAddress.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationAddress.Address).State = EntityState.Modified;
            db.Entry(organizationAddress).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationAddressExists(id))
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

            if (organizationAddress.Address != null)
            {
                if (organizationAddress.Address.PlaceId != null)
                {
                    organizationAddress.Address.AddressPlace = db.Places.Find(organizationAddress.Address.PlaceId);
                }
                if (organizationAddress.Address.LocalityId != null)
                {
                    organizationAddress.Address.AddressLocality = db.Localities.Find(organizationAddress.Address.LocalityId);
                }
                if (organizationAddress.Address.VillageId != null)
                {
                    organizationAddress.Address.AddressVillage = db.Villages.Find(organizationAddress.Address.VillageId);
                }
                if (organizationAddress.Address.RegionId != null)
                {
                    organizationAddress.Address.AddressRegion = db.Regions.Find(organizationAddress.Address.RegionId);
                }
                if (organizationAddress.Address.CountryId != null)
                {
                    organizationAddress.Address.Country = db.Countries.Find(organizationAddress.Address.CountryId);
                }
            }

            OrganizationAddressDTO dto = organizationAddress.ToDTO();
            return Ok(dto);
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
            organizationAddress.Id = organizationAddress.NewId(db);
            organizationAddress.Address.Id = organizationAddress.Address.NewId(db);
            organizationAddress.AddressId = organizationAddress.Address.Id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Addresses.Add(organizationAddress.Address);
            db.OrganizationAddresses.Add(organizationAddress);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationAddressExists(organizationAddress.Id))
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

            if (organizationAddress.Address != null)
            {
                if (organizationAddress.Address.PlaceId != null)
                {
                    organizationAddress.Address.AddressPlace = db.Places.Find(organizationAddress.Address.PlaceId);
                }
                if (organizationAddress.Address.LocalityId != null)
                {
                    organizationAddress.Address.AddressLocality = db.Localities.Find(organizationAddress.Address.LocalityId);
                }
                if (organizationAddress.Address.VillageId != null)
                {
                    organizationAddress.Address.AddressVillage = db.Villages.Find(organizationAddress.Address.VillageId);
                }
                if (organizationAddress.Address.RegionId != null)
                {
                    organizationAddress.Address.AddressRegion = db.Regions.Find(organizationAddress.Address.RegionId);
                }
                if (organizationAddress.Address.CountryId != null)
                {
                    organizationAddress.Address.Country = db.Countries.Find(organizationAddress.Address.CountryId);
                }
            }
            OrganizationAddressDTO dto = organizationAddress.ToDTO();
            return CreatedAtRoute("GetOrganizationAddress", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationAddress))]
        public IHttpActionResult DeleteOrganizationAddress(int id)
        {
            OrganizationAddress organizationAddress = db.OrganizationAddresses.Find(id);
            if (organizationAddress == null)
            {
                return NotFound();
            }
            organizationAddress.Deleted = true;
            db.Entry(organizationAddress).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationAddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            OrganizationAddressDTO dto = organizationAddress.ToDTO();
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

        private bool OrganizationAddressExists(int id)
        {
            return db.OrganizationAddresses.Count(e => e.Id == id) > 0;
        }
    }
}
