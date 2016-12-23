using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Clients;
using GTIWebAPI.Models.Context;
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
    [RoutePrefix("api/ClientGTIClients")]
    public class ClientGTIClientsController : ApiController
    {
        private DbClient db = new DbClient();

        /// <summary>
        /// All gtiClients
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<ClientGTIClientDTO> GetAll()
        {
            // тут процедурка 
            //надо вообще оно или нет? вряд ли
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientGTIClient, ClientGTIClientDTO>();
            });
            IEnumerable<ClientGTIClientDTO> dtos = Mapper
                .Map<IEnumerable<ClientGTIClient>, IEnumerable<ClientGTIClientDTO>>
                (db.ClientGTIClient.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get client gtiClient by client id for VIEW
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Collection of ClientGTIClientDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetGTIClientsByClientId")]
        [ResponseType(typeof(IEnumerable<ClientGTIClientView>))]
        public IEnumerable<ClientGTIClientView> GetByClient(int clientId)
        {
            IEnumerable<ClientGTIClientView> clientList = new List<ClientGTIClientView>();
            clientList = db.ClientGTIClientList(clientId);
            return clientList;       
        }

        /// <summary>
        /// Get one gtiClient for view by gtiClient id
        /// </summary>
        /// <param name="id">ClientGTIClient id</param>
        /// <returns>ClientGTIClientEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetGTIClientView", Name = "GetGTIClientView")]
        [ResponseType(typeof(ClientGTIClientDTO))]
        public IHttpActionResult GetGTIClientView(int id)
        {
            ClientGTIClient gtiClient = db.ClientGTIClient.Find(id);
            if (gtiClient == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientGTIClient, ClientGTIClientDTO>();
            });
            ClientGTIClientDTO dto = Mapper.Map<ClientGTIClientDTO>(gtiClient);
            return Ok(dto);
        }

        /// <summary>
        /// Get one gtiClient for edit by gtiClient id
        /// </summary>
        /// <param name="id">ClientGTIClient id</param>
        /// <returns>ClientGTIClientEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetGTIClientEdit")]
        [ResponseType(typeof(ClientGTIClientDTO))]
        public IHttpActionResult GetGTIClientEdit(int id)
        {
            ClientGTIClient gtiClient = db.ClientGTIClient.Find(id);
            if (gtiClient == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientGTIClient, ClientGTIClientDTO>();
            });
            ClientGTIClientDTO dto = Mapper.Map<ClientGTIClient, ClientGTIClientDTO>(gtiClient);
            return Ok(dto);
        }

        /// <summary>
        /// Update client gtiClient
        /// </summary>
        /// <param name="id">GTIClient id</param>
        /// <param name="clientGTIClient">ClientGTIClient object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutGTIClient")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientGTIClient(int id, ClientGTIClient clientGTIClient)
        {
            if (clientGTIClient == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != clientGTIClient.Id)
            {
                return BadRequest();
            }
            db.Entry(clientGTIClient).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientGTIClientExists(id))
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
        /// Insert new client gtiClient
        /// </summary>
        /// <param name="clientGTIClient">ClientGTIClient object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostGTIClient")]
        [ResponseType(typeof(ClientGTIClientDTO))]
        public IHttpActionResult PostClientGTIClient(ClientGTIClient clientGTIClient)
        {
            if (clientGTIClient == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            clientGTIClient.Id = clientGTIClient.NewId(db);
            db.ClientGTIClient.Add(clientGTIClient);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientGTIClientExists(clientGTIClient.Id))
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
                m.CreateMap<ClientGTIClient, ClientGTIClientDTO>();
            });
            ClientGTIClientDTO dto = Mapper.Map<ClientGTIClient, ClientGTIClientDTO>(clientGTIClient);
            return CreatedAtRoute("GetGTIClientView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete gtiClient
        /// </summary>
        /// <param name="id">GTIClient Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteGTIClient")]
        [ResponseType(typeof(ClientGTIClient))]
        public IHttpActionResult DeleteClientGTIClient(int id)
        {
            ClientGTIClient clientGTIClient = db.ClientGTIClient.Find(id);
            if (clientGTIClient == null)
            {
                return NotFound();
            }
            clientGTIClient.Deleted = true;
            db.Entry(clientGTIClient).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientGTIClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientGTIClient, ClientGTIClientDTO>();
            });
            ClientGTIClientDTO dto = Mapper.Map<ClientGTIClient, ClientGTIClientDTO>(clientGTIClient);
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

        private bool ClientGTIClientExists(int id)
        {
            return db.ClientGTIClient.Count(e => e.Id == id) > 0;
        }

    }
}
