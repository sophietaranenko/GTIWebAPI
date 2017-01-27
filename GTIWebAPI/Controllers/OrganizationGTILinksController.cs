using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
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
    [RoutePrefix("api/OrganizationGTILinks")]
    public class OrganizationGTILinksController : ApiController
    {
        private DbOrganization db = new DbOrganization();


        ///// <summary>
        ///// Get organization gtiClient by organization id for VIEW
        ///// </summary>
        ///// <param name="organizationId">Client Id</param>
        ///// <returns>Collection of OrganizationGTILinkDTO</returns>
        //[GTIFilter]
        //[HttpGet]
        //[Route("GetGTIClientsByClientId")]
        //[ResponseType(typeof(IEnumerable<OrganizationGTILinkView>))]
        //public IEnumerable<OrganizationGTILinkView> GetByClient(int organizationId)
        //{
        //    IEnumerable<OrganizationGTILinkView> organizationList = new List<OrganizationGTILinkView>();
        //    organizationList = db.OrganizationGTILinkList(organizationId);
        //    return organizationList;       
        //}

        /// <summary>
        /// Get one gtiClient for view by gtiClient id
        /// </summary>
        /// <param name="id">OrganizationGTILink id</param>
        /// <returns>OrganizationGTILinkEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetGTIClientView", Name = "GetGTIClientView")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult GetGTIClientView(int id)
        {
            OrganizationGTILink gtiClient = db.OrganizationGTILinks.Find(id);
            if (gtiClient == null)
            {
                return NotFound();
            }

            OrganizationGTILinkDTO dto = new OrganizationGTILinkDTO
            {
              OrganizationId = gtiClient.OrganizationId,
              OrganizationGTIId = gtiClient.GTIId,
              Id = gtiClient.Id
            };

            OrganizationGTI clGTI = db.GTIOrganizations.Find(gtiClient.GTIId);
            clGTI.Office = db.Offices.Find(clGTI.OfficeId);

            OrganizationGTIDTO organizationGtiDto = new OrganizationGTIDTO
            {
                Address = clGTI.Address,
                FullName = clGTI.FullName,
                Email = clGTI.Email,
                Id = clGTI.Id,
                OfficeId = clGTI.OfficeId,
                Phone = clGTI.Phone,
                ShortName = clGTI.ShortName,
                Office = new OfficeDTO
                {
                    DealIndex = clGTI.Office.DealIndex,
                    Id = clGTI.Office.Id,
                    ShortName = clGTI.Office.ShortName
                }                
            };

            dto.OrganizationGTI = organizationGtiDto;

            return Ok(dto);
        }

        /// <summary>
        /// Get one gtiClient for edit by gtiClient id
        /// </summary>
        /// <param name="id">OrganizationGTILink id</param>
        /// <returns>OrganizationGTILinkEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetGTIClientEdit")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult GetGTIClientEdit(int id)
        {
            OrganizationGTILink gtiClient = db.OrganizationGTILinks.Find(id);
            if (gtiClient == null)
            {
                return NotFound();
            }

            OrganizationGTILinkDTO dto = new OrganizationGTILinkDTO
            {
                OrganizationId = gtiClient.OrganizationId,
                OrganizationGTIId = gtiClient.GTIId,
                Id = gtiClient.Id
            };

            OrganizationGTI clGTI = db.GTIOrganizations.Find(gtiClient.GTIId);
            clGTI.Office = db.Offices.Find(clGTI.OfficeId);

            OrganizationGTIDTO organizationGtiDto = new OrganizationGTIDTO
            {
                Address = clGTI.Address,
                FullName = clGTI.FullName,
                Email = clGTI.Email,
                Id = clGTI.Id,
                OfficeId = clGTI.OfficeId,
                Phone = clGTI.Phone,
                ShortName = clGTI.ShortName,
                Office = new OfficeDTO
                {
                    DealIndex = clGTI.Office.DealIndex,
                    Id = clGTI.Office.Id,
                    ShortName = clGTI.Office.ShortName
                }
            };

            dto.OrganizationGTI = organizationGtiDto;

            return Ok(dto);
        }

        /// <summary>
        /// Update organization gtiClient
        /// </summary>
        /// <param name="id">GTIClient id</param>
        /// <param name="clientGTIClient">OrganizationGTILink object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutGTIClient")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationGTILink(int id, OrganizationGTILink link)
        {
            if (link == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != link.Id)
            {
                return BadRequest();
            }
            db.Entry(link).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationGTILinkExists(id))
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
        /// Insert new organization gtiClient
        /// </summary>
        /// <param name="link">OrganizationGTILink object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostOrganizationGTILink")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult PostLink(OrganizationGTILink link)
        {
            if (link == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            link.Id = link.NewId(db);
            db.OrganizationGTILinks.Add(link);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationGTILinkExists(link.Id))
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
                m.CreateMap<OrganizationGTILink, OrganizationGTILinkDTO>();
            });
            OrganizationGTILinkDTO dto = Mapper.Map<OrganizationGTILink, OrganizationGTILinkDTO>(link);
            return CreatedAtRoute("GetOrganizationGTILink", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete gtiClient
        /// </summary>
        /// <param name="id">GTIClient Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteGTIClient")]
        [ResponseType(typeof(OrganizationGTILink))]
        public IHttpActionResult DeleteOrganizationGTILink(int id)
        {
            OrganizationGTILink organizationGTI = db.OrganizationGTILinks.Find(id);
            if (organizationGTI == null)
            {
                return NotFound();
            }
            organizationGTI.Deleted = true;
            db.Entry(organizationGTI).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationGTILinkExists(id))
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
                m.CreateMap<OrganizationGTILink, OrganizationGTILinkDTO>();
            });
            OrganizationGTILinkDTO dto = Mapper.Map<OrganizationGTILink, OrganizationGTILinkDTO>(organizationGTI);
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

        private bool OrganizationGTILinkExists(int id)
        {
            return db.OrganizationGTILinks.Count(e => e.Id == id) > 0;
        }

    }
}
