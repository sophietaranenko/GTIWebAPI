using GTIWebAPI.Filters;
using GTIWebAPI.Models.Clients;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GTIWebAPI.Models.Dictionary;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for clients
    /// </summary>
    [RoutePrefix("api/Clients")]
    public class ClientsController : ApiController
    {
        private DbClient db = new DbClient();


        [GTIFilter]
        [HttpGet]
        [Route("GetClientsByFilter")]
        public IEnumerable<ClientViewDTO> GetClientsByFilter(string filter)
        {
            IEnumerable<ClientViewDTO> clientList = db.ClientFilter(filter);
            return clientList;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetClientByEmployeeId")]
        public IEnumerable<ClientViewDTO> GetClientsByEmployeeId(int employeeId)
        {
            IEnumerable<ClientViewDTO> clientList = db.ClientFilter("")
                .Where(c => c.CreatorId == employeeId).ToList();
            return clientList;
        }

        /// <summary>
        /// Get one client by client Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetClient", Name = "GetClient")]
        public IHttpActionResult GetClient(int id)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<Client, ClientDTO>();
                m.CreateMap<Address, AddressDTO>();
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            //клиент со всеми прибамбасами 
            Client client = db.Client.Find(id);
            ClientDTO dto = new ClientDTO();
            if (client != null)
            {
                dto = Mapper.Map<ClientDTO>(client);
            }
            return Ok(dto);            
        }


        [GTIFilter]
        [HttpPut]
        [Route("PutClient")]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != client.Id)
            {
                return BadRequest();
            }
            db.Entry(client.Address).State = EntityState.Modified;
          //  db.Entry(client.AddressPhysical).State = EntityState.Modified;
            db.Entry(client).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ClientExists(id))
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
        [HttpPost]
        [Route("PostClient")]
        [ResponseType(typeof(ClientDTO))]
        public IHttpActionResult PostClient(Client client)
        {
            client.Id = client.NewId(db);
            client.Address.Id = client.Address.NewId(db);
           

            client.AddressId = client.Address.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Address.Add(client.Address);
            db.Client.Add(client);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Id))
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
                m.CreateMap<ClientDTO, Client>();
                m.CreateMap<Address, AddressDTO>();
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            ClientDTO dto = Mapper.Map<ClientDTO>(client);
            return CreatedAtRoute("GetClient", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete client
        /// </summary>
        /// <param name="id">client id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteClient")]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client != null)
            {
                client.Deleted = true;
                db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Ok(client);
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

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.Id == id) > 0;
        }
    }
}
