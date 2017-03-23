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
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Foundation documents are additional documents, that employee has. These documents give employee a right for some privileges. 
    /// </summary>
    [RoutePrefix("api/EmployeeFoundationDocuments")]
    public class EmployeeFoundationDocumentsController : ApiController
    {
        private IRepository<EmployeeFoundationDocument> repo;

        public EmployeeFoundationDocumentsController()
        {
            repo = new EmployeeFoundationDocumentsRepository();
        }

        public EmployeeFoundationDocumentsController(IRepository<EmployeeFoundationDocument> repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// All foundation documents 
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<EmployeeFoundationDocumentDTO>))]
        public IHttpActionResult GetEmployeeFoundationDocumentAll()
        {
            try
            {
                List<EmployeeFoundationDocumentDTO> dtos =
                    repo.GetAll()
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);

            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get employee foundation documents by employee id 
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeFoundationDocDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(List<EmployeeFoundationDocumentDTO>))]
        public IHttpActionResult GetEmployeeFoundationDocumentByEmployee(int employeeId)
        {
            try
            {
                List<EmployeeFoundationDocumentDTO> dtos =
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
            try
            {
                EmployeeFoundationDocumentDTO foundationDoc = 
                    repo.Get(id)
                    .ToDTO();
                return Ok(foundationDoc);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
            if (employeeFoundationDoc == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeFoundationDoc.Id)
            {
                return BadRequest();
            }
            try
            {

                EmployeeFoundationDocumentDTO dto =
                    repo.Edit(employeeFoundationDoc)
                    .ToDTO();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
                EmployeeFoundationDocumentDTO dto = 
                    repo.Add(employeeFoundationDoc)
                    .ToDTO();
                return CreatedAtRoute("GetEmployeeFoundationDocument", new { id = dto.Id }, dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
            try
            {
                EmployeeFoundationDocumentDTO dto = repo.Delete(id).ToDTO();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}