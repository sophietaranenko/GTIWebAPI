
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

namespace GTIWebAPI.Controllers
{

    [RoutePrefix("api/ClientTaxInfo")]
    public class ClientTaxInfosController : ApiController
    {
        private DbClient db = new DbClient();

        [GTIFilter]
        [Route("GetClientTaxInfos")]
        public IEnumerable<ClientTaxInfoDTO> GetClientTaxInfos()
        {
            IEnumerable<ClientTaxInfo> taxInfos = db.ClientTaxInfo.Where(s => s.Deleted != true).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientTaxInfoDTO, ClientTaxInfo>();
            });
            IEnumerable<ClientTaxInfoDTO> dtos = Mapper.
                Map<IEnumerable<ClientTaxInfo>, IEnumerable<ClientTaxInfoDTO>>(taxInfos);
            return dtos;
        }

        [GTIFilter]
        [Route("GetClientTaxInfosByClientId")]
        public IEnumerable<ClientTaxInfoDTO> GetClientTaxInfosByClientId(int clientId)
        {
            IEnumerable<ClientTaxInfo> taxInfos = db.ClientTaxInfo.Where(s => s.Deleted != true
            && s.ClientId == clientId).ToList();

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientTaxInfoDTO, ClientTaxInfo>();
            });

            IEnumerable<ClientTaxInfoDTO> dtos = Mapper.
                Map<IEnumerable<ClientTaxInfo>, IEnumerable<ClientTaxInfoDTO>>(taxInfos);

            return dtos;
        }



        [GTIFilter]
        [Route("GetClientTaxInfo", Name = "GetClientTaxInfo")]
        // GET: api/ClientTaxInfos/5
        [ResponseType(typeof(ClientTaxInfo))]
        public IHttpActionResult GetClientTaxInfo(int id)
        {
            ClientTaxInfo clientTaxInfo = db.ClientTaxInfo.Find(id);

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientTaxInfoDTO, ClientTaxInfo>();
            });

            ClientTaxInfoDTO dto = Mapper.Map<ClientTaxInfoDTO>(clientTaxInfo);

            if (clientTaxInfo == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [GTIFilter]
        [Route("PutClientTaxInfo")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientTaxInfo(int id, ClientTaxInfoDTO inDto)
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
                m.CreateMap<ClientTaxInfoDTO, ClientTaxInfo>();
            });
            ClientTaxInfo clientTaxInfo = Mapper.Map<ClientTaxInfo>(inDto);
            db.Entry(clientTaxInfo).State = EntityState.Modified;
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
        [ResponseType(typeof(ClientTaxInfo))]
        public IHttpActionResult PostClientTaxInfo(ClientTaxInfoDTO inDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientTaxInfoDTO, ClientTaxInfo>();
            });

            ClientTaxInfo clientTaxInfo = Mapper.Map<ClientTaxInfo>(inDto);

            clientTaxInfo.Id = clientTaxInfo.NewId(db);
            db.ClientTaxInfo.Add(clientTaxInfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientTaxInfoExists(clientTaxInfo.Id))
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
                m.CreateMap<ClientTaxInfo, ClientTaxInfoDTO>();
            });
            ClientTaxInfoDTO dto = Mapper.Map<ClientTaxInfoDTO>(clientTaxInfo);

            return CreatedAtRoute("GetClientTaxInfo", new { id = dto.Id }, dto);
        }

        [GTIFilter]
        [Route("DeleteClientTaxInfo")]
        [ResponseType(typeof(ClientTaxInfo))]
        public IHttpActionResult DeleteClientTaxInfo(int id)
        {
            ClientTaxInfo clientTaxInfo = db.ClientTaxInfo.Find(id);
            if (clientTaxInfo == null)
            {
                return NotFound();
            }

            clientTaxInfo.Deleted = true;
            db.Entry(clientTaxInfo).State = EntityState.Modified;
            db.SaveChanges();
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientTaxInfo, ClientTaxInfoDTO>();
            });
            ClientTaxInfoDTO dto = Mapper.Map<ClientTaxInfoDTO>(clientTaxInfo);
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
            return db.ClientTaxInfo.Count(e => e.Id == id) > 0;
        }
    }
}

