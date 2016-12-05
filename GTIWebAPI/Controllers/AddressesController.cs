using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Service;
using AutoMapper;
using System.Threading;


namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for Addresses
    /// </summary>
   // [Authorize]
    [RoutePrefix("api/Addresses")]
    public class AddressesController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Get Address View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAddressView")]
        [ResponseType(typeof(AddressDTO))]
        public IHttpActionResult GetAddressView(int id)
        {
            var princip = Thread.CurrentPrincipal;
            Address address = db.Address.Find(id);
            if (address == null)
            {
                return NotFound();
            }
            Mapper.Initialize(cfg => cfg.CreateMap<Address, AddressDTO>());
            AddressDTO dto = Mapper.Map<AddressDTO>(address);
            return Ok(dto);
        }

        /// <summary>
        /// Get address for edit
        /// </summary>
        /// <param name="id">Address Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAddressEdit")]
        [ResponseType(typeof(AddressDTO))]
        public IHttpActionResult GetAddressEdit(int id)
        { 
            Address address = db.Address.Find(id);
            if (address == null)
            {
                return NotFound();
            }
            Mapper.Initialize(cfg => cfg.CreateMap<Address, AddressDTO>());
            AddressDTO dto = Mapper.Map<AddressDTO>(address);
            return Ok(dto);
        }

        /// <summary>
        /// Update address
        /// </summary>
        /// <param name="id">Address Id</param>
        /// <param name="address">Address object</param>
        /// <returns></returns>
        [HttpPut]
        [Route("PutAddress")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAddress(int id, Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.Id)
            {
                return BadRequest();
            }

            if (!EnumCheck(address))
            {
                return BadRequest();
            }

            db.Entry(address).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Insert new address
        /// </summary>
        /// <param name="address">Address JSON contains Id = null</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostAddress")]
        [ResponseType(typeof(Address))]
        public IHttpActionResult PostAddress(Address address)
        {
            if (address == null)
            {
                return BadRequest(ModelState);
            }

            address.Id = db.NewId("Address");

            if (!ModelState.IsValid)  
            {
                return BadRequest(ModelState);
            }
            if (!EnumCheck(address))
            {
                return BadRequest(ModelState);
            }

            db.Address.Add(address);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AddressExists(address.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// Get region types
        /// </summary>
        /// <returns>Collection of EnumItem objects</returns>
        [HttpGet]
        [Route("GetRegion")]
        public IEnumerable<EnumItem> GetRegionTypes()
        {
            var regionList = Enum.GetValues(typeof(Region)).Cast<Region>().Select(v => new EnumItem
            {
                Text = v.ToString(),
                Value = (Int32)v
            }).ToList();
            return regionList;
        }

        /// <summary>
        /// Get place types
        /// </summary>
        /// <returns>Collection of EnumItem objects</returns>
        [HttpGet]
        [Route("GetPlace")]
        public IEnumerable<EnumItem> GetPlaceTypes()
        {
            var placeList = Enum.GetValues(typeof(Place)).Cast<Place>().Select(v => new EnumItem
            {
                Text = v.ToString(),
                Value = (Int32)v
            }).ToList();
            return placeList;
        }

        /// <summary>
        /// Get locality types
        /// </summary>
        /// <returns>Collection of EnumItem objects</returns>
        [HttpGet]
        [Route("GetLocality")]
        public IEnumerable<EnumItem> GetLocalityTypes()
        {
            var localityList = Enum.GetValues(typeof(Locality)).Cast<Locality>().Select(v => new EnumItem
            {
                Text = v.ToString(),
                Value = (Int32)v
            }).ToList();
            return localityList;
        }

        /// <summary>
        /// Get village types 
        /// </summary>
        /// <returns>Collection of EnumItem objects</returns>
        [HttpGet]
        [Route("GetVillage")]
        public IEnumerable<EnumItem> GetVillageTypes()
        {
            var villageList = Enum.GetValues(typeof(Village)).Cast<Village>().Select(v => new EnumItem
            {
                Text = v.ToString(),
                Value = (Int32)v
            }).ToList();
            return villageList;
        }

        private bool EnumCheck(Address address)
        {
            bool result = true;
            if (address.RegionType != null)
            {
                if (!Enum.IsDefined(typeof(Region), (int)address.RegionType))
                {
                    result = false;
                }
            }
            if (address.LocalityType != null)
            {
                if (!Enum.IsDefined(typeof(Locality), (int)address.LocalityType))
                {
                    result = false;
                }
            }
            if (address.PlaceType != null)
            {
                if (!Enum.IsDefined(typeof(Place), (int)address.PlaceType))
                {
                    result = false;
                }
            }
            if (address.VillageType != null)
            {
                if (!Enum.IsDefined(typeof(Village), (int)address.VillageType))
                {
                    result = false;
                }
            }
            return result;
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

        private bool AddressExists(int id)
        {
            return db.Address.Count(e => e.Id == id) > 0;
        }
    }
}