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
        /// <summary>
        /// Get employee addresss by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationAddressDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationAddressDTO>))]
        public IHttpActionResult GetOrganizationAddressByOrganizationId(int organizationId)
        {
            List<OrganizationAddress> addresses = new List<OrganizationAddress>();

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    addresses = db.OrganizationAddresses
                        .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                        .Include(d => d.Address)
                        .Include(d => d.Address.Country)
                        .Include(d => d.OrganizationAddressType)
                        .Include(d => d.Address.AddressLocality)
                        .Include(d => d.Address.AddressPlace)
                        .Include(d => d.Address.AddressRegion)
                        .Include(d => d.Address.AddressVillage)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            List<OrganizationAddressDTO> dtos = addresses.Select(p => p.ToDTO()).ToList();
            return Ok(dtos);
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
            OrganizationAddress organizationAddress = new OrganizationAddress();


            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    organizationAddress = db.OrganizationAddresses.Find(id);
                    if (organizationAddress != null)
                    {
                        db.Entry(organizationAddress).Reference(d => d.OrganizationAddressType).Load();
                        db.Entry(organizationAddress).Reference(d => d.Address).Load();
                        if (organizationAddress.Address != null)
                        {
                            db.Entry(organizationAddress.Address).Reference(d => d.AddressLocality).Load();
                            db.Entry(organizationAddress.Address).Reference(d => d.AddressPlace).Load();
                            db.Entry(organizationAddress.Address).Reference(d => d.AddressVillage).Load();
                            db.Entry(organizationAddress.Address).Reference(d => d.AddressRegion).Load();
                            db.Entry(organizationAddress.Address).Reference(d => d.Country).Load();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (organizationAddress == null)
            {
                return NotFound();
            }

            OrganizationAddressDTO dto = organizationAddress.ToDTO();
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

            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
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

                    db.Entry(organizationAddress).Reference(d => d.OrganizationAddressType).Load();
                    db.Entry(organizationAddress).Reference(d => d.Address).Load();
                    if (organizationAddress.Address != null)
                    {
                        db.Entry(organizationAddress.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(organizationAddress.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(organizationAddress.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(organizationAddress.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(organizationAddress.Address).Reference(d => d.Country).Load();
                    }

                }

            }
            catch (Exception e)
            {
                return BadRequest();
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

            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
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

                db.Entry(organizationAddress).Reference(d => d.OrganizationAddressType).Load();
                db.Entry(organizationAddress).Reference(d => d.Address).Load();
                if (organizationAddress.Address != null)
                {
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressLocality).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressPlace).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressVillage).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressRegion).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.Country).Load();
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
            OrganizationAddress organizationAddress = new OrganizationAddress();

            using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
            {
                organizationAddress = db.OrganizationAddresses.Find(id);
                if (organizationAddress == null)
                {
                    return NotFound();
                }
                db.Entry(organizationAddress).Reference(d => d.OrganizationAddressType).Load();
                db.Entry(organizationAddress).Reference(d => d.Address).Load();
                if (organizationAddress.Address != null)
                {
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressLocality).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressPlace).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressVillage).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.AddressRegion).Load();
                    db.Entry(organizationAddress.Address).Reference(d => d.Country).Load();
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
