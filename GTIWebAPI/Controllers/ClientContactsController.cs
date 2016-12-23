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
    /// </summary>
    [RoutePrefix("api/ClientContacts")]
    public class ClientContactsController : ApiController
    {
        private DbClient db = new DbClient();

        /// <summary>
        /// All contacts
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<ClientContactDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientContact, ClientContactDTO>();
            });

            IEnumerable<ClientContactDTO> dtos = Mapper
                .Map<IEnumerable<ClientContact>, IEnumerable<ClientContactDTO>>
                (db.ClientContact.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get client contact by client id for VIEW
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>Collection of ClientContactDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactsByClientId")]
        [ResponseType(typeof(IEnumerable<ClientContactDTO>))]
        public IEnumerable<ClientContactDTO> GetByClient(int clientId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            IEnumerable<ClientContactDTO> dtos = Mapper
                .Map<IEnumerable<ClientContact>, IEnumerable<ClientContactDTO>>
                (db.ClientContact.Where(p => p.Deleted != true && p.ClientId == clientId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one contact for view by contact id
        /// </summary>
        /// <param name="id">ClientContact id</param>
        /// <returns>ClientContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetClientContactView", Name = "GetClientContactView")]
        [ResponseType(typeof(ClientContactDTO))]
        public IHttpActionResult GetContactView(int id)
        {
            ClientContact contact = db.ClientContact.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            ClientContactDTO dto = Mapper.Map<ClientContactDTO>(contact);
            return Ok(dto);
        }

        /// <summary>
        /// Get one contact for edit by contact id
        /// </summary>
        /// <param name="id">ClientContact id</param>
        /// <returns>ClientContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactEdit")]
        [ResponseType(typeof(ClientContactDTO))]
        public IHttpActionResult GetContactEdit(int id)
        {
            ClientContact contact = db.ClientContact.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            ClientContactDTO dto = Mapper.Map<ClientContact, ClientContactDTO>(contact);
            return Ok(dto);
        }

        /// <summary>
        /// Update client contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="clientContact">ClientContact object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutContact")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientContact(int id, ClientContact clientContact)
        {
            if (clientContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != clientContact.Id)
            {
                return BadRequest();
            }
            db.Entry(clientContact).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientContactExists(id))
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
        /// Insert new client contact
        /// </summary>
        /// <param name="clientContact">ClientContact object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostContact")]
        [ResponseType(typeof(ClientContactDTO))]
        public IHttpActionResult PostClientContact(ClientContact clientContact)
        {
            if (clientContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            clientContact.Id = clientContact.NewId(db);
            db.ClientContact.Add(clientContact);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientContactExists(clientContact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            ClientContact contact = db.ClientContact.Find(clientContact.Id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            ClientContactDTO dto = Mapper.Map<ClientContactDTO>(contact);
            return CreatedAtRoute("GetContactView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteContact")]
        [ResponseType(typeof(ClientContact))]
        public IHttpActionResult DeleteClientContact(int id)
        {
            ClientContact clientContact = db.ClientContact.Find(id);
            if (clientContact == null)
            {
                return NotFound();
            }
            clientContact.Deleted = true;
            db.Entry(clientContact).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientContactExists(id))
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
                m.CreateMap<ClientContact, ClientContactDTO>();
            });
            ClientContactDTO dto = Mapper.Map<ClientContact, ClientContactDTO>(clientContact);
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

        private bool ClientContactExists(int id)
        {
            return db.ClientContact.Count(e => e.Id == id) > 0;
        }

    }
}
