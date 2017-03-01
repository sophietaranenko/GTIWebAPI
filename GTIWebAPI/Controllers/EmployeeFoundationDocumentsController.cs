using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Filters;
using AutoMapper;
using GTIWebAPI.Models.Personnel;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Foundation documents are additional documents, that employee has. These documents give employee a right for some privileges. 
    /// </summary>
    [RoutePrefix("api/EmployeeFoundationDocuments")]
    public class EmployeeFoundationDocumentsController : ApiController
    {
        /// <summary>
        /// All foundation documents 
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeFoundationDocumentDTO>))]
        public IHttpActionResult GetEmployeeFoundationDocumentAll()
        {
            IEnumerable<EmployeeFoundationDocumentDTO> dtos = new List<EmployeeFoundationDocumentDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeFoundationDocuments.Where(p => p.Deleted != true).Include(d=> d.FoundationDocument).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get employee foundation documents by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeFoundationDocDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeFoundationDocumentDTO>))]
        public IHttpActionResult GetEmployeeFoundationDocumentByEmployee(int employeeId)
        {
            IEnumerable<EmployeeFoundationDocumentDTO> dtos = new List<EmployeeFoundationDocumentDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.EmployeeFoundationDocuments.Where(p => p.Deleted != true && p.EmployeeId == employeeId).Include(d => d.FoundationDocument).ToList()
                        .Select(d => d.ToDTO()).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get one foundation document by id
        /// </summary>
        /// <param name="id">EmployeeFoundationDoc id</param>
        /// <returns>EmployeeFoundationDocEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeFoundationDocument")]
        [ResponseType(typeof(EmployeeFoundationDocumentDTO))]
        public IHttpActionResult GetEmployeeFoundationDocument(int id)
        {
            EmployeeFoundationDocument foundationDoc = new EmployeeFoundationDocument();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    foundationDoc = db.EmployeeFoundationDocuments.Find(id);
                    if (foundationDoc != null)
                    {
                        db.Entry(foundationDoc).Reference(d => d.FoundationDocument).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            if (foundationDoc == null)
            {
                return NotFound();
            }

            EmployeeFoundationDocumentDTO dto = foundationDoc.ToDTO();

            return Ok(dto);
        }

        /// <summary>
        /// Update employee foundationDoc
        /// </summary>
        /// <param name="id">FoundationDoc id</param>
        /// <param name="employeeFoundationDoc">EmployeeFoundationDoc object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeFoundationDocument(int id, EmployeeFoundationDocument employeeFoundationDoc)
        {
            if (employeeFoundationDoc == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeFoundationDoc.Id)
            {
                return BadRequest();
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(employeeFoundationDoc).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeFoundationDocumentExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    employeeFoundationDoc.FoundationDocument = db.FoundationDocuments.Find(employeeFoundationDoc.FoundationDocumentId);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeFoundationDocumentDTO dto = employeeFoundationDoc.ToDTO();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee foundation document
        /// </summary>
        /// <param name="employeeFoundationDoc">EmployeeFoundationDoc object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeFoundationDocumentDTO))]
        public IHttpActionResult PostEmployeeFoundationDocument(EmployeeFoundationDocument employeeFoundationDoc)
        {
            if (employeeFoundationDoc == null)
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
                    employeeFoundationDoc.Id = employeeFoundationDoc.NewId(db);
                    db.EmployeeFoundationDocuments.Add(employeeFoundationDoc);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeFoundationDocumentExists(employeeFoundationDoc.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    employeeFoundationDoc.FoundationDocument = db.FoundationDocuments.Find(employeeFoundationDoc.FoundationDocumentId);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
            EmployeeFoundationDocumentDTO dto = employeeFoundationDoc.ToDTO();
            return CreatedAtRoute("GetEmployeeFoundationDocument", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete foundation document 
        /// </summary>
        /// <param name="id">FoundationDoc Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeFoundationDocument))]
        public IHttpActionResult DeleteEmployeeFoundationDocument(int id)
        {
            EmployeeFoundationDocument employeeFoundationDoc = new EmployeeFoundationDocument();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeFoundationDoc = db.EmployeeFoundationDocuments.Find(id);
                    if (employeeFoundationDoc == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employeeFoundationDoc).Reference(d => d.FoundationDocument).Load();

                    employeeFoundationDoc.Deleted = true;
                    db.Entry(employeeFoundationDoc).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeFoundationDocumentExists(id))
                        {
                            return NotFound();
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

            EmployeeFoundationDocumentDTO dto = employeeFoundationDoc.ToDTO();
            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeFoundationDocumentExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeFoundationDocuments.Count(e => e.Id == id) > 0;
            }
        }
    }
}