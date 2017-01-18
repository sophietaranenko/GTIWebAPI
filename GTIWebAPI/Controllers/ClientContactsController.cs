using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Clients;
using GTIWebAPI.Models.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/ClientContacts")]
    public class ClientContactsController : ApiController
    {
        private DbOrganization db = new DbOrganization();

        /// <summary>
        /// All contacts
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<OrganizationContactDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });

            IEnumerable<OrganizationContactDTO> dtos = Mapper
                .Map<IEnumerable<OrganizationContact>, IEnumerable<OrganizationContactDTO>>
                (db.OrganizationContacts.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get organization contact by organization id for VIEW
        /// </summary>
        /// <param name="organizationId">Client Id</param>
        /// <returns>Collection of ClientContactDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactsByClientId")]
        [ResponseType(typeof(IEnumerable<OrganizationContactDTO>))]
        public IEnumerable<OrganizationContactDTO> GetByClient(int organizationId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });
            IEnumerable<OrganizationContactDTO> dtos = Mapper
                .Map<IEnumerable<OrganizationContact>, IEnumerable<OrganizationContactDTO>>
                (db.OrganizationContacts.Where(p => p.Deleted != true && p.ClientId == organizationId).ToList());
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
        [ResponseType(typeof(OrganizationContactDTO))]
        public IHttpActionResult GetContactView(int id)
        {
            OrganizationContact contact = db.OrganizationContacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });
            OrganizationContactDTO dto = Mapper.Map<OrganizationContactDTO>(contact);
            return Ok(dto);
        }

        /// <summary>
        /// Get one contact for view by contact id as USER with organization info 
        /// </summary>
        /// <param name="id">ClientContact id</param>
        /// <returns>ClientContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetClientContactAsUser", Name = "GetClientContactAsUser")]
        [ResponseType(typeof(OrganizationContactUserDTO))]
        public IHttpActionResult GetContactAsUser(int id)
        {
            OrganizationContact contact = db.OrganizationContacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });

            OrganizationContactDTO contactDto = Mapper.Map<OrganizationContactDTO>(contact);

            OrganizationContactUserDTO userDto = new OrganizationContactUserDTO()
            {
                OrganizationContact = contactDto
            };

            userDto.ProfilePicture = "";

            Organization organization = db.Organizations.Find(contactDto.ClientId);
            if (organization != null)
            {
                userDto.Organization = organization.MapToEdit();
            }

            ApplicationDbContext appDb = new ApplicationDbContext();
            ApplicationUser user = appDb.Users.Where(u => u.TableName == "ClientContact" && u.TableId == id).FirstOrDefault();

            string Photo = "";
            if (user != null)
            {
                var Image = appDb.UserImage.Where(i => i.UserId == user.Id).FirstOrDefault();
                if (Image != null)
                {
                    Photo = Image.ImageName;
                }
            }
            userDto.ProfilePicture = Photo;

            return Ok(userDto);
        }


        /// <summary>
        /// Get one contact for edit by contact id
        /// </summary>
        /// <param name="id">ClientContact id</param>
        /// <returns>ClientContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetContactEdit")]
        [ResponseType(typeof(OrganizationContactDTO))]
        public IHttpActionResult GetContactEdit(int id)
        {
            OrganizationContact contact = db.OrganizationContacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });
            OrganizationContactDTO dto = Mapper.Map<OrganizationContact, OrganizationContactDTO>(contact);
            return Ok(dto);
        }

        /// <summary>
        /// Update organization contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="organizationContact">ClientContact object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutContact")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientContact(int id, OrganizationContact organizationContact)
        {
            if (organizationContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationContact.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationContact).State = EntityState.Modified;
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

            OrganizationContact contact = db.OrganizationContacts.Find(organizationContact.Id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });
            OrganizationContactDTO dto = Mapper.Map<OrganizationContactDTO>(contact);
            return Ok(dto);
        }

        /// <summary>
        /// Insert new organization contact
        /// </summary>
        /// <param name="organizationContact">ClientContact object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostContact")]
        [ResponseType(typeof(OrganizationContactDTO))]
        public IHttpActionResult PostClientContact(OrganizationContact organizationContact)
        {
            if (organizationContact == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            organizationContact.Id = organizationContact.NewId(db);
            db.OrganizationContacts.Add(organizationContact);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientContactExists(organizationContact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            OrganizationContact contact = db.OrganizationContacts.Find(organizationContact.Id);
            if (contact == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });
            OrganizationContactDTO dto = Mapper.Map<OrganizationContactDTO>(contact);
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
        [ResponseType(typeof(OrganizationContact))]
        public IHttpActionResult DeleteClientContact(int id)
        {
            OrganizationContact organizationContact = db.OrganizationContacts.Find(id);
            if (organizationContact == null)
            {
                return NotFound();
            }
            organizationContact.Deleted = true;
            db.Entry(organizationContact).State = EntityState.Modified;
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
                m.CreateMap<OrganizationContact, OrganizationContactDTO>();
            });
            OrganizationContactDTO dto = Mapper.Map<OrganizationContact, OrganizationContactDTO>(organizationContact);
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
            return db.OrganizationContacts.Count(e => e.Id == id) > 0;
        }

    }
}
