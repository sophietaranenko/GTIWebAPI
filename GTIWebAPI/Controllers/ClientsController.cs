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
        [Route("GetClientEdit", Name = "GetClientEdit")]
        [ResponseType(typeof(ClientEditDTO))]
        public IHttpActionResult GetClientEdit(int id)
        {
            Client client = db.Client.Find(id);
            ClientEditDTO dto = client.MapToEdit();
            return Ok(dto);
        }
        /// <summary>
        /// Get one client by client Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetClient", Name = "GetClient")]
        [ResponseType(typeof(ClientDTO))]
        public IHttpActionResult GetClient(int id)
        {
            //клиент со всеми прибамбасами 
            Mapper.Initialize(m => m.CreateMap<Address, AddressDTO>());
            Client client = db.Client.Find(id);
            ClientDTO dto = new ClientDTO()
            {
                Address = Mapper.Map<AddressDTO>(client.Address),
                AddressId = client.AddressId,
                Email = client.Email,
                EmployeeId = client.EmployeeId,
                EnglishName = client.EnglishName,
                FaxNumber = client.FaxNumber,
                Id = client.Id,
                IdentityCode = client.IdentityCode,
                NativeName = client.NativeName,
                OrganizationTypeId = client.OrganizationTypeId,
                PhoneNumber = client.PhoneNumber,
                RussianName = client.RussianName,
                Website = client.Website
            };
            Mapper.Initialize(m => m.CreateMap<OrganizationType, OrganizationTypeDTO>());
            dto.OrganizationType = Mapper.Map<OrganizationTypeDTO>(client.OrganizationType);

            List<ClientContact> contacts = db.ClientContact.Where(c => c.Deleted != true && c.ClientId == id).ToList();
            Mapper.Initialize
            (m =>
                {
                    m.CreateMap<ClientContact, ClientContactDTO>();
                });
            dto.ClientContacts = Mapper.Map<IEnumerable<ClientContact>, IEnumerable<ClientContactDTO>>(contacts);


            dto.ClientGTIClients = db.ClientGTIClientList(id);

            List<ClientSigner> signers = db.ClientSigner.Where(s => s.Deleted != true && s.ClientId == id).ToList();
            Mapper.Initialize
                (
                m =>
                {
                    m.CreateMap<ClientSigner, ClientSignerDTO>();
                    m.CreateMap<SignerPosition, SignerPositionDTO>();
                }
                );
            dto.ClientSigners = Mapper.Map<IEnumerable<ClientSigner>, IEnumerable<ClientSignerDTO>>(signers);

            List<ClientTaxInfo> taxes = db.ClientTaxInfo.Where(c => c.ClientId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<ClientGTIClient, ClientGTIClientDTO>());
            dto.ClientTaxInfos = Mapper.Map<IEnumerable<ClientTaxInfo>, IEnumerable<ClientTaxInfoDTO>>(taxes);

            return Ok(dto);
        }


        [GTIFilter]
        [HttpPut]
        [Route("PutClient")]
        public IHttpActionResult PutClient(int id, ClientEditDTO client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != client.Id)
            {
                return BadRequest();
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientEditDTO, Client>();
                m.CreateMap<OrganizationTypeDTO, OrganizationType>();
                m.CreateMap<AddressDTO, Address>();
            });

            Client cl = Mapper.Map<Client>(client);

            db.Entry(cl.Address).State = EntityState.Modified;
            db.Entry(cl).State = EntityState.Modified;

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
        [ResponseType(typeof(ClientEditDTO))]
        public IHttpActionResult PostClient(ClientEditDTO postDto)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<ClientEditDTO, Client>();
                m.CreateMap<OrganizationTypeDTO, OrganizationType>();
                m.CreateMap<AddressDTO, Address>();
            });

            Client client = Mapper.Map<Client>(postDto);

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
            ClientEditDTO dto = client.MapToEdit();
            return CreatedAtRoute("GetClientEdit", new { id = dto.Id }, dto);
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
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
            }
            ClientEditDTO dto = client.MapToEdit();
            return Ok(dto);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetOrganizationTypes")]
        public IEnumerable<OrganizationTypeDTO> GetOrganizationTypes()
        {
            List<OrganizationType> types = db.OrganizationType.OrderBy(o => o.Name).ToList();
            Mapper.Initialize(m => m.CreateMap<OrganizationType, OrganizationTypeDTO>());
            IEnumerable<OrganizationTypeDTO> dtos = Mapper.Map<IEnumerable<OrganizationType>, IEnumerable<OrganizationTypeDTO>>(types);
            return dtos;
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
