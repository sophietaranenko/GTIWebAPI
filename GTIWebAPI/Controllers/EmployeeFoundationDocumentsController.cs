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
    [RoutePrefix("api/EmployeeFoundationDocuments")]
    public class EmployeeFoundationDocumentsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// All foundationDocs
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeFoundationDocumentDTO> GetAll()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            IEnumerable<EmployeeFoundationDocumentDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeFoundationDocument>, IEnumerable<EmployeeFoundationDocumentDTO>>
                (db.EmployeeFoundationDocuments.Where(p => p.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get employee foundationDoc by employee id for VIEW
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Collection of EmployeeFoundationDocDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeFoundationDocumentDTO>))]
        public IEnumerable<EmployeeFoundationDocumentDTO> GetByEmployee(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            IEnumerable<EmployeeFoundationDocumentDTO> dtos = Mapper
                .Map<IEnumerable<EmployeeFoundationDocument>, IEnumerable<EmployeeFoundationDocumentDTO>>
                (db.EmployeeFoundationDocuments.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get one foundationDoc for edit by id
        /// </summary>
        /// <param name="id">EmployeeFoundationDoc id</param>
        /// <returns>EmployeeFoundationDocEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeFoundationDocument")]
        [ResponseType(typeof(EmployeeFoundationDocumentDTO))]
        public IHttpActionResult GetFoundationDocEdit(int id)
        {
            EmployeeFoundationDocument foundationDoc = db.EmployeeFoundationDocuments.Find(id);
            if (foundationDoc == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            EmployeeFoundationDocumentDTO dto = Mapper.Map<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>(foundationDoc);
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
        public IHttpActionResult PutEmployeeFoundationDoc(int id, EmployeeFoundationDocument employeeFoundationDoc)
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
            db.Entry(employeeFoundationDoc).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeFoundationDocExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            employeeFoundationDoc.FoundationDocument = db.FoundationDocuments.Find(employeeFoundationDoc.FoundationDocumentId);
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            EmployeeFoundationDocumentDTO dto = Mapper.Map<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>(employeeFoundationDoc);
            return Ok(dto);
        }

        /// <summary>
        /// Insert new employee foundationDoc
        /// </summary>
        /// <param name="employeeFoundationDoc">EmployeeFoundationDoc object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeFoundationDocumentDTO))]
        public IHttpActionResult PostEmployeeFoundationDoc(EmployeeFoundationDocument employeeFoundationDoc)
        {
            if (employeeFoundationDoc == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employeeFoundationDoc.Id = employeeFoundationDoc.NewId(db);
            db.EmployeeFoundationDocuments.Add(employeeFoundationDoc);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeFoundationDocExists(employeeFoundationDoc.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            employeeFoundationDoc.FoundationDocument = db.FoundationDocuments.Find(employeeFoundationDoc.FoundationDocumentId);
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            EmployeeFoundationDocumentDTO dto = Mapper.Map<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>(employeeFoundationDoc);
            return CreatedAtRoute("GetEmployeeFoundationDocument", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete foundationDoc
        /// </summary>
        /// <param name="id">FoundationDoc Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeFoundationDocument))]
        public IHttpActionResult DeleteEmployeeFoundationDoc(int id)
        {
            EmployeeFoundationDocument employeeFoundationDoc = db.EmployeeFoundationDocuments.Find(id);
            if (employeeFoundationDoc == null)
            {
                return NotFound();
            }
            employeeFoundationDoc.Deleted = true;
            db.Entry(employeeFoundationDoc).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeFoundationDocExists(id))
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
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            EmployeeFoundationDocumentDTO dto = Mapper.Map<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>(employeeFoundationDoc);
            return Ok(dto);
        }

        ///// <summary>
        ///// Get all types of foundation documents
        ///// </summary>
        ///// <returns></returns>
        //[Route("GetFoundationDocuments")]
        //[HttpGet]
        //public IEnumerable<FoundationDocumentDTO> GetFoundationDocuments()
        //{
        //    List<FoundationDocument> docs = db.FoundationDocument.Where(e => e.Deleted != true).ToList();
        //    Mapper.Initialize(m => 
        //    {
        //        m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
        //    });
        //    IEnumerable<FoundationDocumentDTO> dtos = Mapper.Map<IEnumerable<FoundationDocument>, IEnumerable<FoundationDocumentDTO>>(docs);
        //    return dtos;
        //}

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

        private bool EmployeeFoundationDocExists(int id)
        {
            return db.EmployeeFoundationDocuments.Count(e => e.Id == id) > 0;
        }
    }
}