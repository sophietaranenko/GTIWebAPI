﻿using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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

        /// <summary>
        /// Get employee links by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of OrganizationGTILinkDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationGTILinkDTO>))]
        public IEnumerable<OrganizationGTILinkDTO> GetOrganizationGTILinkByOrganizationId(int organizationId)
        {
            List<OrganizationGTILink> links = db.OrganizationGTILinks
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();
            foreach (var link in links)
            {
                if (link.GTIId != null)
                {
                    link.OrganizationGTI = db.GTIOrganizations.Find(link.GTIId);
                    if (link.OrganizationGTI != null)
                    {
                        link.OrganizationGTI.Office = db.Offices.Find(link.OrganizationGTI.OfficeId);
                    }
                }
            }
            List<OrganizationGTILinkDTO> dtos = links.Select(p => p.ToDTO()).ToList();
            return dtos;
        }

        /// <summary>
        /// Get one link by link id
        /// </summary>
        /// <param name="id">OrganizationGTILink id</param>
        /// <returns>OrganizationGTILinkEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationGTILink")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult GetOrganizationGTILink(int id)
        {
            OrganizationGTILink link = db.OrganizationGTILinks.Find(id);
            if (link == null)
            {
                return NotFound();
            }
            if (link.GTIId != null)
            {
                link.OrganizationGTI = db.GTIOrganizations.Find(link.GTIId);
                if (link.OrganizationGTI != null)
                {
                    link.OrganizationGTI.Office = db.Offices.Find(link.OrganizationGTI.OfficeId);
                }
            }
            OrganizationGTILinkDTO dto = link.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee link
        /// </summary>
        /// <param name="id">Passport id</param>
        /// <param name="organizationGTILink">OrganizationGTILink object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrganizationGTILink(int id, OrganizationGTILink organizationGTILink)
        {
            if (organizationGTILink == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationGTILink.Id)
            {
                return BadRequest();
            }
            db.Entry(organizationGTILink).State = EntityState.Modified;
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

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually
            if (organizationGTILink.GTIId != null)
            {
                organizationGTILink.OrganizationGTI = db.GTIOrganizations.Find(organizationGTILink.GTIId);
                if (organizationGTILink.OrganizationGTI != null)
                {
                    organizationGTILink.OrganizationGTI.Office = db.Offices.Find(organizationGTILink.OrganizationGTI.OfficeId);
                }
            }
            OrganizationGTILinkDTO dto = organizationGTILink.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee link
        /// </summary>
        /// <param name="organizationGTILink">OrganizationGTILink object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationGTILinkDTO))]
        public IHttpActionResult PostOrganizationGTILink(OrganizationGTILink organizationGTILink)
        {
            if (organizationGTILink == null)
            {
                return BadRequest(ModelState);
            }
            organizationGTILink.Id = organizationGTILink.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.OrganizationGTILinks.Add(organizationGTILink);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationGTILinkExists(organizationGTILink.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //Reload method of db context doesn't work
            //Visitor extension of dbContext doesn't wotk
            //that's why we reload related entities manually
            if (organizationGTILink.GTIId != null)
            {
                organizationGTILink.OrganizationGTI = db.GTIOrganizations.Find(organizationGTILink.GTIId);
                if (organizationGTILink.OrganizationGTI != null)
                {
                    organizationGTILink.OrganizationGTI.Office = db.Offices.Find(organizationGTILink.OrganizationGTI.OfficeId);
                }
            }
            OrganizationGTILinkDTO dto = organizationGTILink.ToDTO();
            return CreatedAtRoute("GetOrganizationGTILink", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete link
        /// </summary>
        /// <param name="id">Passport Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationGTILink))]
        public IHttpActionResult DeleteOrganizationGTILink(int id)
        {
            OrganizationGTILink organizationGTILink = db.OrganizationGTILinks.Find(id);
            if (organizationGTILink == null)
            {
                return NotFound();
            }
            organizationGTILink.Deleted = true;
            db.Entry(organizationGTILink).State = EntityState.Modified;
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
            if (organizationGTILink.GTIId != null)
            {
                organizationGTILink.OrganizationGTI = db.GTIOrganizations.Find(organizationGTILink.GTIId);
                if (organizationGTILink.OrganizationGTI != null)
                {
                    organizationGTILink.OrganizationGTI.Office = db.Offices.Find(organizationGTILink.OrganizationGTI.OfficeId);
                }
            }
            OrganizationGTILinkDTO dto = organizationGTILink.ToDTO();
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
