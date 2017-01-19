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
    public class OrganizationSignersController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        [GTIFilter]
        [Route("GetClientSigners")]
        public IEnumerable<OrganizationSignerDTO> GetClientSigners()
        {
            IEnumerable<OrganizationSigner> signers = db.OrganizationSigners.Where(s => s.Deleted != true).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationSignerDTO, OrganizationSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });
            IEnumerable<OrganizationSignerDTO> dtos = Mapper.
                Map<IEnumerable<OrganizationSigner>, IEnumerable<OrganizationSignerDTO>>(signers);
            return dtos;
        }

        [GTIFilter]
        [Route("GetClientSignersByClientId")]
        public IEnumerable<OrganizationSignerDTO> GetClientSignersByClientId(int clientId)
        {
            IEnumerable<OrganizationSigner> signers = db.OrganizationSigners.Where(s => s.Deleted != true
            && s.ClientId == clientId).ToList();

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationSignerDTO, OrganizationSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });

            IEnumerable<OrganizationSignerDTO> dtos = Mapper.
                Map<IEnumerable<OrganizationSigner>, IEnumerable<OrganizationSignerDTO>>(signers);

            return dtos;
        }



        [GTIFilter]
        [Route("GetClientSigner", Name = "GetClientSigner")]
        // GET: api/ClientSigners/5
        [ResponseType(typeof(OrganizationSigner))]
        public IHttpActionResult GetClientSigner(int id)
        {
            OrganizationSigner clientSigner = db.OrganizationSigners.Find(id);

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationSignerDTO, OrganizationSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });

            OrganizationSignerDTO dto = Mapper.Map<OrganizationSignerDTO>(clientSigner);

            if (clientSigner == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [GTIFilter]
        [Route("PutClientSigner")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientSigner(int id, OrganizationSignerDTO inDto)
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
                m.CreateMap<OrganizationSignerDTO, OrganizationSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });
            OrganizationSigner clientSigner = Mapper.Map<OrganizationSigner>(inDto);
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
        [ResponseType(typeof(OrganizationSigner))]
        public IHttpActionResult PostClientSigner(OrganizationSignerDTO inDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationSignerDTO, OrganizationSigner>();
                m.CreateMap<SignerPositionDTO, SignerPosition>();
            });

            OrganizationSigner clientSigner = Mapper.Map<OrganizationSigner>(inDto);

            clientSigner.Id = clientSigner.NewId(db);
            db.OrganizationSigners.Add(clientSigner);

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
                m.CreateMap<OrganizationSigner, OrganizationSignerDTO>();
                m.CreateMap<SignerPosition, SignerPositionDTO>();
            });
            OrganizationSignerDTO dto = Mapper.Map<OrganizationSignerDTO>(clientSigner);

            return CreatedAtRoute("GetClientSigner", new { id = dto.Id }, dto);
        }

        [GTIFilter]
        [Route("DeleteClientSigner")]
        [ResponseType(typeof(OrganizationSigner))]
        public IHttpActionResult DeleteClientSigner(int id)
        {
            OrganizationSigner clientSigner = db.OrganizationSigners.Find(id);
            if (clientSigner == null)
            {
                return NotFound();
            }

            clientSigner.Deleted = true;
            db.Entry(clientSigner).State = EntityState.Modified;
            db.SaveChanges();
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationSigner, OrganizationSignerDTO>();
                m.CreateMap<SignerPosition, SignerPositionDTO>();
            });
            OrganizationSignerDTO dto = Mapper.Map<OrganizationSignerDTO>(clientSigner);
            return Ok(dto);
        }

        [GTIFilter]
        [Route("GetSignerPositions")]
        public IEnumerable<SignerPositionDTO> GetSignerPositions()
        {
            IEnumerable<SignerPosition> pos = db.SignerPositions.ToList();
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
            return db.OrganizationSigners.Count(e => e.Id == id) > 0;
        }
    }
}