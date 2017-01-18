
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
using GTIWebAPI.Models.Clients;
using GTIWebAPI.Models.Context;
using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Dictionary;

namespace GTIWebAPI.Controllers
{

    [RoutePrefix("api/ClientTaxInfo")]
    public class ClientTaxInfosController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        [GTIFilter]
        [Route("GetClientTaxInfos")]
        public IEnumerable<OrganizationTaxInfoDTO> GetClientTaxInfos()
        {
            IEnumerable<OrganizationTaxInfo> taxInfos = db.OrganizationTaxInfos.Where(s => s.Deleted != true).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfo, OrganizationTaxInfoDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            IEnumerable<OrganizationTaxInfoDTO> dtos = Mapper.
                Map<IEnumerable<OrganizationTaxInfo>, IEnumerable<OrganizationTaxInfoDTO>>(taxInfos);
            return dtos;
        }

        [GTIFilter]
        [Route("GetClientTaxInfosByClientId")]
        public IEnumerable<OrganizationTaxInfoDTO> GetClientTaxInfosByClientId(int organizationId)
        {
            IEnumerable<OrganizationTaxInfo> taxInfos = db.OrganizationTaxInfos.Where(s => s.Deleted != true
            && s.ClientId == organizationId).ToList();

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfo, OrganizationTaxInfoDTO>();
                m.CreateMap<Address, AddressDTO>();
            });

            IEnumerable<OrganizationTaxInfoDTO> dtos = Mapper.
                Map<IEnumerable<OrganizationTaxInfo>, IEnumerable<OrganizationTaxInfoDTO>>(taxInfos);

            return dtos;
        }



        [GTIFilter]
        [Route("GetClientTaxInfo", Name = "GetClientTaxInfo")]
        // GET: api/ClientTaxInfos/5
        [ResponseType(typeof(OrganizationTaxInfo))]
        public IHttpActionResult GetClientTaxInfo(int id)
        {
            OrganizationTaxInfo organizationTaxInfo = db.OrganizationTaxInfos.Find(id);

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfo, OrganizationTaxInfoDTO>();
                m.CreateMap<Address, AddressDTO>();
            });

            OrganizationTaxInfoDTO dto = Mapper.Map<OrganizationTaxInfoDTO>(organizationTaxInfo);

            if (organizationTaxInfo == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [GTIFilter]
        [Route("PutClientTaxInfo")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientTaxInfo(int id, OrganizationTaxInfoDTO inDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != inDto.Id)
            {
                return BadRequest();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfoDTO, OrganizationTaxInfo>();
                m.CreateMap<AddressDTO, Address>();
            });
            OrganizationTaxInfo organizationTaxInfo = Mapper.Map<OrganizationTaxInfo>(inDto);
            db.Entry(organizationTaxInfo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientTaxInfoExists(id))
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

        [GTIFilter]
        [Route("PostClientTaxInfo")]
        [ResponseType(typeof(OrganizationTaxInfo))]
        public IHttpActionResult PostClientTaxInfo(OrganizationTaxInfoDTO inDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfoDTO, OrganizationTaxInfo>();
                m.CreateMap<AddressDTO, Address>();
            });

            OrganizationTaxInfo organizationTaxInfo = Mapper.Map<OrganizationTaxInfo>(inDto);

            organizationTaxInfo.Id = organizationTaxInfo.NewId(db);
            organizationTaxInfo.Address.Id = organizationTaxInfo.Address.NewId(db);
            organizationTaxInfo.AddressId = organizationTaxInfo.Address.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(organizationTaxInfo.Address);
            db.OrganizationTaxInfos.Add(organizationTaxInfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientTaxInfoExists(organizationTaxInfo.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfo, OrganizationTaxInfoDTO>();
                m.CreateMap<Address, AddressDTO>();
            });

            OrganizationTaxInfoDTO dto = Mapper.Map<OrganizationTaxInfoDTO>(organizationTaxInfo);

            return CreatedAtRoute("GetClientTaxInfo", new { id = dto.Id }, dto);
        }

        [GTIFilter]
        [Route("DeleteClientTaxInfo")]
        [ResponseType(typeof(OrganizationTaxInfoDTO))]
        public IHttpActionResult DeleteClientTaxInfo(int id)
        {
            OrganizationTaxInfo organizationTaxInfo = db.OrganizationTaxInfos.Find(id);
            if (organizationTaxInfo == null)
            {
                return NotFound();
            }

            organizationTaxInfo.Deleted = true;
            db.Entry(organizationTaxInfo).State = EntityState.Modified;
            db.SaveChanges();
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationTaxInfo, OrganizationTaxInfoDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            OrganizationTaxInfoDTO dto = Mapper.Map<OrganizationTaxInfoDTO>(organizationTaxInfo);
            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientTaxInfoExists(int id)
        {
            return db.OrganizationTaxInfos.Count(e => e.Id == id) > 0;
        }
    }
}

