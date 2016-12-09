using AutoMapper;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
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
    [RoutePrefix("api/EmployeeOffices")]
    public class EmployeeOfficeController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeOfficeDTO> GetEmployeeOffice()
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
                m.CreateMap<Office, OfficeDTO>();
            });

            IEnumerable<EmployeeOfficeDTO> dtos =
                Mapper.Map<IEnumerable<EmployeeOffice>, IEnumerable<EmployeeOfficeDTO>>
                (db.EmployeeOffice.Where(e => e.Deleted != true).ToList());
            return dtos;
        }

        /// <summary>
        /// Get positions of current employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetOfficesByEmployeeId")]
        public IEnumerable<EmployeeOfficeDTO> GetEmployeeOfficeByEmployeeId(int employeeId)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
                m.CreateMap<Office, OfficeDTO>();
            });
            IEnumerable<EmployeeOfficeDTO> dtos =
                Mapper.Map<IEnumerable<EmployeeOffice>, IEnumerable<EmployeeOfficeDTO>>
                (db.EmployeeOffice.Where(e => e.Deleted != true && e.EmployeeId == employeeId).ToList());
            return dtos;
        }

        /// <summary>
        /// Get position View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetOfficeView", Name = "GetOfficeView")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult GetEmployeeOfficeView(int id)
        {
            EmployeeOffice employeeOffice = db.EmployeeOffice.Find(id);
            if (employeeOffice == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
                m.CreateMap<Office, OfficeDTO>();
            });
            EmployeeOfficeDTO dto = Mapper.Map<EmployeeOfficeDTO>(employeeOffice);
            return Ok(dto);
        }

        /// <summary>
        /// Get position Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetOfficeEdit")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult GetEmployeeOfficeEdit(int id)
        {
            EmployeeOffice employeeOffice = db.EmployeeOffice.Find(id);
            if (employeeOffice == null)
            {
                return NotFound();
            }
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
                m.CreateMap<Office, OfficeDTO>();
            });
            EmployeeOfficeDTO dto = Mapper.Map<EmployeeOfficeDTO>(employeeOffice);
            return Ok(dto);
        }

        /// <summary>
        /// Update employee position
        /// </summary>
        /// <param name="id">id of position</param>
        /// <param name="employeeOffice">EmployeeOffice</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutOffice")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeOffice(int id, EmployeeOffice employeeOffice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employeeOffice.Id)
            {
                return BadRequest();
            }
            db.Entry(employeeOffice).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeOfficeExists(id))
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
        /// <param name="employeeOffice">EmployeeOffice object contains id = 0</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostOffice")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult PostEmployeeOffice(EmployeeOffice employeeOffice)
        {
            employeeOffice.Id = employeeOffice.NewId(db);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.EmployeeOffice.Add(employeeOffice);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeOfficeExists(employeeOffice.Id))
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
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
                m.CreateMap<Office, OfficeDTO>();
            });
            EmployeeOfficeDTO dto = Mapper.Map<EmployeeOfficeDTO>(employeeOffice);
            //return Ok();
            return CreatedAtRoute("GetOfficeView", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Dleete education document
        /// </summary>
        /// <param name="id">id of document</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteOffice")]
        [ResponseType(typeof(EmployeeOfficeDTO))]
        public IHttpActionResult DeleteEmployeeOffice(int id)
        {
            EmployeeOffice employeeOffice = db.EmployeeOffice.Find(id);
            if (employeeOffice == null)
            {
                return NotFound();
            }

            employeeOffice.Deleted = true;
            db.Entry(employeeOffice).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeOfficeExists(id))
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
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
                m.CreateMap<Office, OfficeDTO>();
            });
            EmployeeOfficeDTO dto = Mapper.Map<EmployeeOfficeDTO>(employeeOffice);

            return Ok(dto);
        }

        /// <summary>
        /// Get study form types
        /// </summary>
        /// <returns>Collection of EnumItem objects</returns>
        [HttpGet]
        [Route("GetOffices")]
        public IEnumerable<OfficeDTO> GetOffices()
        {
            List<Office> offices = db.Offices.ToList();
            Mapper.Initialize(m => m.CreateMap<Office, OfficeDTO>());
            IEnumerable<OfficeDTO> dtos = Mapper.Map<IEnumerable<Office>, IEnumerable<OfficeDTO>>(offices);
            return dtos;
        }


        [HttpGet]
        [Route("GetProfessions")]
        public IEnumerable<ProfessionDTO> GetProfessions()
        {
            List<Profession> professions = db.Profession.ToList();
            Mapper.Initialize(m => m.CreateMap<Profession, ProfessionDTO>());
            IEnumerable<ProfessionDTO> dtos = Mapper.Map<IEnumerable<Profession>, IEnumerable<ProfessionDTO>>(professions);
            return dtos;
        }

        [HttpGet]
        [Route("GetDepartments")]
        public IEnumerable<DepartmentDTO> GetDepartments()
        {
            List<Department> departments = db.Department.ToList();
            Mapper.Initialize(m => m.CreateMap<Department, DepartmentDTO>());
            IEnumerable<DepartmentDTO> dtos = Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDTO>>(departments);
            return dtos;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeOfficeExists(int id)
        {
            return db.EmployeeOffice.Count(e => e.Id == id) > 0;
        }
    }
}
