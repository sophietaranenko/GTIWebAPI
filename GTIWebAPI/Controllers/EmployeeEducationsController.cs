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
using AutoMapper;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Filters;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for employee educations
    /// </summary>
 //   [Authorize]
    [RoutePrefix("api/EmployeeEducations")]
    public class EmployeeEducationsController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Get all education documents
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeEducationDTO> GetEmployeeEducation()
        {
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            IEnumerable<EmployeeEducationDTO> dtos =
                Mapper.Map<IEnumerable<EmployeeEducation>, IEnumerable<EmployeeEducationDTO>>
                (db.EmployeeEducation.Where(e => e.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get education documents of current employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEducationsByEmployeeId")]
        public IEnumerable<EmployeeEducationDTO> GetEmployeeEducationByEmployeeId(int employeeId)
        {
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            IEnumerable<EmployeeEducationDTO> dtos =
                Mapper.Map<IEnumerable<EmployeeEducation>, IEnumerable<EmployeeEducationDTO>>
                (db.EmployeeEducation.Where(e => e.Deleted != true && e.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get education document View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEducationView", Name = "GetEducationView")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult GetEmployeeEducationView(int id)
        {
            EmployeeEducation employeeEducation = db.EmployeeEducation.Find(id);
            if (employeeEducation == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            EmployeeEducationDTO dto = Mapper.Map<EmployeeEducationDTO>(employeeEducation);
            return Ok(dto);
        }

        /// <summary>
        /// Get education document Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEducationEdit")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult GetEmployeeEducationEdit(int id)
        {
            EmployeeEducation employeeEducation = db.EmployeeEducation.Find(id);
            if (employeeEducation == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            EmployeeEducationDTO dto = Mapper.Map<EmployeeEducationDTO>(employeeEducation);
            return Ok(dto);
        }

        /// <summary>
        /// Update EmployeeEducation
        /// </summary>
        /// <param name="id">id of document</param>
        /// <param name="employeeEducation">EmployeeEducation document</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutEducation")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeEducation(int id, EmployeeEducation employeeEducation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeEducation.Id)
            {
                return BadRequest();
            }
            db.Entry(employeeEducation).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeEducationExists(id))
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
        /// Insert new employee education document
        /// </summary>
        /// <param name="employeeEducation">EmployeeEducation object contains id = 0</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostEducation")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult PostEmployeeEducation(EmployeeEducation employeeEducation)
        {
            employeeEducation.Id = employeeEducation.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.EmployeeEducation.Add(employeeEducation);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeEducationExists(employeeEducation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            EmployeeEducationDTO dto = Mapper.Map<EmployeeEducationDTO>(employeeEducation);
            //return Ok();
            return CreatedAtRoute("GetEducationView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Dleete education document
        /// </summary>
        /// <param name="id">id of document</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteEducation")]
        [ResponseType(typeof(EmployeeEducationDTO))]
        public IHttpActionResult DeleteEmployeeEducation(int id)
        {
            EmployeeEducation employeeEducation = db.EmployeeEducation.Find(id);
            if (employeeEducation == null)
            {
                return NotFound();
            }

            employeeEducation.Deleted = true;
            db.Entry(employeeEducation).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeEducationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            EmployeeEducationDTO dto = Mapper.Map<EmployeeEducationDTO>(employeeEducation);
            return Ok(dto);
        }

        /// <summary>
        /// Get study form types
        /// </summary>
        /// <returns>Collection of EnumItem objects</returns>
        [HttpGet]
        [Route("GetStudyForms")]
        public IEnumerable<EnumItem> GetStudyForms()
        {
            var studyList = Enum.GetValues(typeof(FormStudy)).Cast<FormStudy>().Select(v => new EnumItem
            {
                Text = v.ToString(),
                Value = (Int32)v
            }).ToList();
            return studyList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeEducationExists(int id)
        {
            return db.EmployeeEducation.Count(e => e.Id == id) > 0;
        }
    }
}