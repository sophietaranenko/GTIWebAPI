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
    //it was just a helper controller for me to fill the table with data

    [RoutePrefix("api/OrganizationLegalForms")]
    public class OrganizationLegalFormsController : ApiController
    {
        /// <summary>
        /// Get one contact for edit by contact id
        /// </summary>
        /// <param name="id">EmployeeContact id</param>
        /// <returns>EmployeeContactEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetOrganizationLegalForm")]
        [ResponseType(typeof(OrganizationLegalFormDTO))]
        public IHttpActionResult GetOrganizationLegalForm(int id)
        {
            OrganizationLegalForm form = new OrganizationLegalForm();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    form = db.OrganizationLegalForms.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (form == null)
            {
                return NotFound();
            }

            OrganizationLegalFormDTO dto = form.ToDTO();
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
        public IHttpActionResult PostOrganizationLegalForm(OrganizationLegalForm form)
        {
            if (form == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (DbMain db = new DbMain(User))
                {
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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            OrganizationLegalFormDTO dto = new OrganizationLegalFormDTO { Id = form.Id, Name = form.Name, Explanation = form.Explanation };
            return CreatedAtRoute("GetOrganizationLegalForm", new { id = dto.Id }, dto);
        }

        private bool OrganizationLegalFormExists(int id)
        {
            using (DbMain db = new DbMain(User))
            { 
                return db.OrganizationLegalForms.Count(e => e.Id == id) > 0;
            }
        }
    }
}
