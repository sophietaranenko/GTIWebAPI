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
    public class OrganizationsController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        [GTIFilter]
        [HttpGet]
        [Route("GetClientsByFilter")]
        public IEnumerable<OrganizationViewDTO> GetClientsByFilter(string filter)
        {
            IEnumerable<OrganizationViewDTO> clientList = db.OrganizationsFilter(filter);
            return clientList;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetClientByEmployeeId")]
        public IEnumerable<OrganizationViewDTO> GetClientsByEmployeeId(int employeeId)
        {
            IEnumerable<OrganizationViewDTO> clientList = db.OrganizationsFilter("")
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
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult GetClientEdit(int id)
        {
            Organization client = db.Organizations.Find(id);
            OrganizationEditDTO dto = client.MapToEdit();
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
        [ResponseType(typeof(OrganizationDTO))]
        public IHttpActionResult GetClient(int id)
        {
            //клиент со всеми прибамбасами 
            Mapper.Initialize(m => m.CreateMap<Address, AddressDTO>());
            Organization client = db.Organizations.Find(id);
            OrganizationDTO dto = new OrganizationDTO()
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

            List<OrganizationContact> contacts = db.OrganizationContacts.Where(c => c.Deleted != true && c.ClientId == id).ToList();
            Mapper.Initialize
            (m =>
                {
                    m.CreateMap<OrganizationContact, OrganizationContactDTO>();
                });
            dto.OrganizationContacts = Mapper.Map<IEnumerable<OrganizationContact>, IEnumerable<OrganizationContactDTO>>(contacts);


            dto.OrganizationGTILinks = db.OrganizationGTILinkList(id);

            List<OrganizationSigner> signers = db.OrganizationSigners.Where(s => s.Deleted != true && s.ClientId == id).ToList();
            Mapper.Initialize
                (
                m =>
                {
                    m.CreateMap<OrganizationSigner, OrganizationSignerDTO>();
                    m.CreateMap<SignerPosition, SignerPositionDTO>();
                }
                );
            dto.ClientSigners = Mapper.Map<IEnumerable<OrganizationSigner>, IEnumerable<OrganizationSignerDTO>>(signers);

            List<OrganizationTaxInfo> taxes = db.OrganizationTaxInfos.Where(c => c.ClientId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<OrganizationGTILink, OrganizationGTILinkDTO>());
            dto.ClientTaxInfos = Mapper.Map<IEnumerable<OrganizationTaxInfo>, IEnumerable<OrganizationTaxInfoDTO>>(taxes);

            return Ok(dto);
        }


        [GTIFilter]
        [HttpPut]
        [Route("PutClient")]
        public IHttpActionResult PutClient(int id, OrganizationEditDTO client)
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
                m.CreateMap<OrganizationEditDTO, Organization>();
                m.CreateMap<OrganizationTypeDTO, OrganizationType>();
                m.CreateMap<AddressDTO, Address>();
            });

            Organization cl = Mapper.Map<Organization>(client);

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
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult PostClient(OrganizationEditDTO postDto)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationEditDTO, Organization>();
                m.CreateMap<OrganizationTypeDTO, OrganizationType>();
                m.CreateMap<AddressDTO, Address>();
            });

            Organization client = Mapper.Map<Organization>(postDto);

            client.Id = client.NewId(db);

            client.Address.Id = client.Address.NewId(db);
            client.AddressId = client.Address.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(client.Address);
            db.Organizations.Add(client);

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
            OrganizationEditDTO dto = client.MapToEdit();
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
            Organization client = db.Organizations.Find(id);
            if (client != null)
            {
                client.Deleted = true;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
            }
            OrganizationEditDTO dto = client.MapToEdit();
            return Ok(dto);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetOrganizationTypes")]
        public IEnumerable<OrganizationTypeDTO> GetOrganizationTypes()
        {
            List<OrganizationType> types = db.OrganizationTypes.OrderBy(o => o.Name).ToList();
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
            return db.Organizations.Count(e => e.Id == id) > 0;
        }
    }
}
