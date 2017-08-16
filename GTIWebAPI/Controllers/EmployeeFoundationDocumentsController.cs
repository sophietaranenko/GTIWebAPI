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
        private IDbContextFactory factory;

        public EmployeeFoundationDocumentsController()
        {
            factory = new DbContextFactory();
        }

        public EmployeeFoundationDocumentsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }
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
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeFoundationDocumentDTO> dtos = unitOfWork.EmployeeFoundationDocumentsRepository.Get(d => d.Deleted != true, includeProperties: "FoundationDocument").Select(d => d.ToDTO());

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
        [ResponseType(typeof(IEnumerable<EmployeeFoundationDocumentDTO>))]
        public IHttpActionResult GetEmployeeFoundationDocumentByEmployee(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeFoundationDocumentDTO> dtos = unitOfWork.EmployeeFoundationDocumentsRepository.Get(d => d.Deleted != true && d.EmployeeId == employeeId, includeProperties: "FoundationDocument").Select(d => d.ToDTO());
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeFoundationDocumentDTO foundationDoc = unitOfWork.EmployeeFoundationDocumentsRepository.Get(d => d.Id == id, includeProperties: "FoundationDocument").FirstOrDefault().ToDTO();
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
        public IHttpActionResult PutEmployeeFoundationDocument(int id, EmployeeFoundationDocumentDTO employeeFoundationDoc)
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
                EmployeeFoundationDocument doc = employeeFoundationDoc.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.EmployeeFoundationDocumentsRepository.Update(doc);
                unitOfWork.Save();
                EmployeeFoundationDocumentDTO dto = unitOfWork.EmployeeFoundationDocumentsRepository.Get(d => d.Id == id, includeProperties: "FoundationDocument").FirstOrDefault().ToDTO();
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
        public IHttpActionResult PostEmployeeFoundationDocument(EmployeeFoundationDocumentDTO employeeFoundationDoc)
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
                EmployeeFoundationDocument doc = employeeFoundationDoc.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                doc.Id = doc.NewId(unitOfWork);
                unitOfWork.EmployeeFoundationDocumentsRepository.Insert(doc);
                unitOfWork.Save();
                EmployeeFoundationDocumentDTO dto = unitOfWork.EmployeeFoundationDocumentsRepository.Get(d => d.Id == doc.Id, includeProperties: "FoundationDocument").FirstOrDefault().ToDTO();
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeFoundationDocument document = unitOfWork.EmployeeFoundationDocumentsRepository.Get(d => d.Id == id, includeProperties: "FoundationDocument").FirstOrDefault();
                document.Deleted = true;
                unitOfWork.EmployeeFoundationDocumentsRepository.Update(document);
                unitOfWork.Save();
                EmployeeFoundationDocumentDTO dto = document.ToDTO();
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