using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/OrganizationLegalForms")]
    public class OrganizationLegalFormController : ApiController
    {
        DbOrganization db = new DbOrganization();

        /// <summary>
        /// Get one contact for edit by contact id
        /// </summary>
        /// <param name="id">EmployeeContact id</param>
        /// <returns>EmployeeContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationLegalForm")]
        [ResponseType(typeof(OrganizationLegalFormDTO))]
        public IHttpActionResult GetContactEdit(int id)
        {
            OrganizationLegalForm form = db.OrganizationLegalForms.Find(id);
            if (form == null)
            {
                return NotFound();
            }
            AutoMapper.Mapper.Initialize(m =>
            {
                m.CreateMap<OrganizationLegalForm, OrganizationLegalFormDTO>();
            });

            OrganizationLegalFormDTO dto = AutoMapper.Mapper.Map<OrganizationLegalForm, OrganizationLegalFormDTO>(form);
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee contact
        /// </summary>
        /// <param name="form">EmployeeContact object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationLegalForm))]
        public IHttpActionResult Post(OrganizationLegalForm form)
        {
            if (form == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            form.Id = form.NewId(db);
            db.OrganizationLegalForms.Add(form);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrganizationLegalFormExists(form.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            OrganizationLegalFormDTO dto = new OrganizationLegalFormDTO { Id = form.Id, Name = form.Name, Explanation = form.Explanation };
            return CreatedAtRoute("GetOrganizationLegalForm", new { id = dto.Id }, dto);
        }

        private bool OrganizationLegalFormExists(int id)
        {
            return db.OrganizationLegalForms.Count(e => e.Id == id) > 0;
        }
    }
}
