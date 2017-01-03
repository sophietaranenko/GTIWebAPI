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
    
    [RoutePrefix("api/ClientSigners")]
    public class ClientSignersController : ApiController
    {
        private DbClient db = new DbClient();

        [GTIFilter]
        [Route("GetClientSigners")]
        public IEnumerable<ClientSignerDTO> GetClientSigners()
        {
            IEnumerable<ClientSigner> signers = db.ClientSigner.Where(s => s.Deleted != true).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientSignerDTO, ClientSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });
            IEnumerable<ClientSignerDTO> dtos = Mapper.
                Map<IEnumerable<ClientSigner>, IEnumerable<ClientSignerDTO>>(signers);
            return dtos;
        }

        [GTIFilter]
        [Route("GetClientSignersByClientId")]
        public IEnumerable<ClientSignerDTO> GetClientSignersByClientId(int clientId)
        {
            IEnumerable<ClientSigner> signers = db.ClientSigner.Where(s => s.Deleted != true
            && s.ClientId == clientId).ToList();

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientSignerDTO, ClientSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });

            IEnumerable<ClientSignerDTO> dtos = Mapper.
                Map<IEnumerable<ClientSigner>, IEnumerable<ClientSignerDTO>>(signers);

            return dtos;
        }



        [GTIFilter]
        [Route("GetClientSigner", Name = "GetClientSigner")]
        // GET: api/ClientSigners/5
        [ResponseType(typeof(ClientSigner))]
        public IHttpActionResult GetClientSigner(int id)
        {
            ClientSigner clientSigner = db.ClientSigner.Find(id);

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientSignerDTO, ClientSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });

            ClientSignerDTO dto = Mapper.Map<ClientSignerDTO>(clientSigner);

            if (clientSigner == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [GTIFilter]
        [Route("PutClientSigner")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientSigner(int id, ClientSignerDTO inDto)
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
                m.CreateMap<ClientSignerDTO, ClientSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });
            ClientSigner clientSigner = Mapper.Map<ClientSigner>(inDto);
            db.Entry(clientSigner).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientSignerExists(id))
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
        [Route("PostClientSigner")]
        [ResponseType(typeof(ClientSigner))]
        public IHttpActionResult PostClientSigner(ClientSignerDTO inDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientSignerDTO, ClientSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });

            ClientSigner clientSigner = Mapper.Map<ClientSigner>(inDto);

            clientSigner.Id = clientSigner.NewId(db);
            db.ClientSigner.Add(clientSigner);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientSignerExists(clientSigner.Id))
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
                m.CreateMap<ClientSigner, ClientSignerDTO>();
                m.CreateMap<SignerPosition, SignerPositionDTO>();
            });
            ClientSignerDTO dto = Mapper.Map<ClientSignerDTO>(clientSigner);

            return CreatedAtRoute("GetClientSigner", new { id = dto.Id }, dto);
        }

        [GTIFilter]
        [Route("DeleteClientSigner")]
        [ResponseType(typeof(ClientSigner))]
        public IHttpActionResult DeleteClientSigner(int id)
        {
            ClientSigner clientSigner = db.ClientSigner.Find(id);
            if (clientSigner == null)
            {
                return NotFound();
            }

            clientSigner.Deleted = true;
            db.Entry(clientSigner).State = EntityState.Modified;
            db.SaveChanges();
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientSigner, ClientSignerDTO>();
                m.CreateMap<SignerPosition, SignerPositionDTO>();
            });
            ClientSignerDTO dto = Mapper.Map<ClientSignerDTO>(clientSigner);
            return Ok(dto);
        }

        [GTIFilter]
        [Route("GetSignerPositions")]
        public IEnumerable<SignerPositionDTO> GetSignerPositions()
        {
            IEnumerable<SignerPosition> pos = db.SignerPosition.ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<SignerPosition, SignerPositionDTO>();
            });
            IEnumerable<SignerPositionDTO> dtos = Mapper.Map<IEnumerable<SignerPosition>, IEnumerable<SignerPositionDTO>>(pos);
            return dtos;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientSignerExists(int id)
        {
            return db.ClientSigner.Count(e => e.Id == id) > 0;
        }
    }
}