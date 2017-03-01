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
    [RoutePrefix("api/OrganizationTaxAddresses")]
    public class OrganizationTaxAddressesController : ApiController
    {

        /// <summary>
        /// Get employee addresss by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationTaxAddressDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationTaxAddressDTO>))]
        public IHttpActionResult GetOrganizationTaxAddressByOrganizationId(int organizationId)
        {
            List<OrganizationTaxAddress> addresses = new List<OrganizationTaxAddress>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    addresses = db.OrganizationTaxAddresses
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                    .Include(d => d.Address)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .Include(d => d.Address.Country)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<OrganizationTaxAddressDTO> dtos = addresses.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Get one address by address id
        /// </summary>
        /// <param name="id">OrganizationTaxAddress id</param>
        /// <returns>OrganizationTaxAddressEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationTaxAddress")]
        [ResponseType(typeof(OrganizationTaxAddressDTO))]
        public IHttpActionResult GetOrganizationTaxAddress(int id)
        {
            OrganizationTaxAddress organizationTaxAddress = new OrganizationTaxAddress();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationTaxAddress = db.OrganizationTaxAddresses.Find(id);
                    if (organizationTaxAddress != null)
                    {
                        db.Entry(organizationTaxAddress).Reference(d => d.Address).Load();
                        if (organizationTaxAddress.Address != null)
                        {
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressLocality).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressPlace).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressRegion).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressVillage).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.Country).Load();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (organizationTaxAddress == null)
            {
                return NotFound();
            }

            OrganizationTaxAddressDTO dto = organizationTaxAddress.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee address
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationTaxAddress">OrganizationTaxAddress object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationTaxAddress(int id, OrganizationTaxAddress organizationTaxAddress)
        {
            if (organizationTaxAddress == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationTaxAddress.Id)
            {
                return BadRequest();
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(organizationTaxAddress.Address).State = EntityState.Modified;
                    db.Entry(organizationTaxAddress).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OrganizationTaxAddressExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (organizationTaxAddress != null)
                    {
                        db.Entry(organizationTaxAddress).Reference(d => d.Address).Load();
                        if (organizationTaxAddress.Address != null)
                        {
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressLocality).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressPlace).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressRegion).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressVillage).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.Country).Load();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }


            OrganizationTaxAddressDTO dto = organizationTaxAddress.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee address
        /// </summary>
        /// <param name="organizationTaxAddress">OrganizationTaxAddress object</param>
        /// <returns></returns>
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
                using (DbMain db = new DbMain(User))
                {
                    organizationTaxAddress.Id = organizationTaxAddress.NewId(db);
                    organizationTaxAddress.Address.Id = organizationTaxAddress.Address.NewId(db);
                    organizationTaxAddress.AddressId = organizationTaxAddress.Address.Id;
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.Addresses.Add(organizationTaxAddress.Address);
                    db.OrganizationTaxAddresses.Add(organizationTaxAddress);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (OrganizationTaxAddressExists(organizationTaxAddress.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (organizationTaxAddress != null)
                    {
                        db.Entry(organizationTaxAddress).Reference(d => d.Address).Load();
                        if (organizationTaxAddress.Address != null)
                        {
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressLocality).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressPlace).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressRegion).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressVillage).Load();
                            db.Entry(organizationTaxAddress.Address).Reference(d => d.Country).Load();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }


            OrganizationTaxAddressDTO dto = organizationTaxAddress.ToDTO();
            return CreatedAtRoute("GetOrganizationTaxAddress", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationTaxAddress))]
        public IHttpActionResult DeleteOrganizationTaxAddress(int id)
        {
            OrganizationTaxAddress organizationTaxAddress = new OrganizationTaxAddress();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    organizationTaxAddress = db.OrganizationTaxAddresses.Find(id);
                    if (organizationTaxAddress == null)
                    {
                        return NotFound();
                    }
                    db.Entry(organizationTaxAddress).Reference(d => d.Address).Load();
                    if (organizationTaxAddress.Address != null)
                    {
                        db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(organizationTaxAddress.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(organizationTaxAddress.Address).Reference(d => d.Country).Load();
                    }

                    organizationTaxAddress.Deleted = true;
                    db.Entry(organizationTaxAddress).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OrganizationTaxAddressExists(id))
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

            OrganizationTaxAddressDTO dto = organizationTaxAddress.ToDTO();
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

        private bool OrganizationTaxAddressExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.OrganizationTaxAddresses.Count(e => e.Id == id) > 0;
            }
        }

    }
}
