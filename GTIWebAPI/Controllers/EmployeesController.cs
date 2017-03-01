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
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Service;
using AutoMapper;
using GTIWebAPI.Filters;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using GTIWebAPI.Models.Account;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller manipulating employees
    /// </summary>
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        /// <summary>
        /// </summary>
        /// <param name="filter">
        /// "filter" is a one string contains different number of filters f.e., "Formag Администрация", "Софья Тараненко", "Тараненко Софья Verdeco"
        /// </param>
        /// <returns>a collection on EmployeeViewDTO objects</returns>
        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeViewDTO>))]
        public IHttpActionResult GetEmployeeAll(string officeIds)
        {
            IEnumerable<int> OfficeIds = QueryParser.Parse(officeIds, ',');

            IEnumerable<EmployeeView> employeeList = new List<EmployeeView>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employeeList = db.EmployeeByOffices(OfficeIds);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            IEnumerable<EmployeeViewDTO> dtos = employeeList.Select(c => c.ToDTO());
            return Ok(dtos);
        }

        /// <summary>
        /// Get employee for VIEW by id (without Mapper using, faster then with Mapper ~ 150 ms)
        /// </summary>
        /// <param name="id">id of Employee</param>
        /// <returns>EmployeeDTO</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetView")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult GetEmployeeView(int id)
        {
            EmployeeDTO employeeDTO = new EmployeeDTO();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    Employee employee = db.Employees.Find(id);

                    if (employee == null)
                    {
                        return NotFound();
                    }

                    db.Entry(employee).Reference(d => d.Address).Load();
                    if (employee.Address != null)
                    {
                        db.Entry(employee.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(employee.Address).Reference(d => d.Country).Load();

                    }

                    employeeDTO = employee.ToDTOView();

                    List<EmployeeOffice> offices = 
                        db.EmployeeOffices
                        .Where(o => o.Deleted != true && o.EmployeeId == id)
                        .Include(d => d.Department)
                        .Include(d => d.Office)
                        .Include(d => d.Profession)
                        .ToList();
                    employeeDTO.EmployeeOffice = offices.Select(o => o.ToDTO());

                    List<EmployeePassport> passports = 
                        db.EmployeePassports
                        .Where(p => p.Deleted != true && p.EmployeeId == id)
                        .Include(d => d.Address)
                        .Include(d => d.Address.AddressLocality)
                        .Include(d => d.Address.AddressPlace)
                        .Include(d => d.Address.AddressRegion)
                        .Include(d => d.Address.AddressVillage)
                        .Include(d => d.Address.Country)
                        .ToList();
                    employeeDTO.EmployeePassport = passports.Select(p => p.ToDTO()).ToList();

                    List<EmployeeMilitaryCard> cards = 
                        db.EmployeeMilitaryCards
                        .Where(m => m.Deleted != true && m.EmployeeId == id)
                        .ToList();
                    employeeDTO.EmployeeMilitaryCard = cards.Select(m => m.ToDTO()).ToList();

                    List<EmployeeLanguage> languages = 
                        db.EmployeeLanguages
                        .Where(l => l.Deleted != true && l.EmployeeId == id)
                        .Include(d => d.EmployeeLanguageType)
                        .Include(d => d.Language)
                        .ToList();
                    employeeDTO.EmployeeLanguage = languages.Select(l => l.ToDTO());

                    List<EmployeeInternationalPassport> iPassports = 
                        db.EmployeeInternationalPassports
                        .Where(p => p.Deleted != true && p.EmployeeId == id)
                        .ToList();
                    employeeDTO.EmployeeInternationalPassport = iPassports.Select(i => i.ToDTO());

                    List<EmployeeGun> guns = 
                        db.EmployeeGun
                        .Where(g => g.Deleted != true && g.EmployeeId == id)
                        .ToList();
                    employeeDTO.EmployeeGun = guns.Select(g => g.ToDTO());

                    List<EmployeeFoundationDocument> docs = 
                        db.EmployeeFoundationDocuments
                        .Where(d => d.Deleted != true && d.EmployeeId == id)
                        .Include(d => d.FoundationDocument)
                        .ToList();
                    employeeDTO.EmployeeFoundationDoc = docs.Select(d => d.ToDTO());

                    List<EmployeeEducation> edu = 
                        db.EmployeeEducations
                        .Where(e => e.Deleted != true && e.EmployeeId == id)
                        .Include(d => d.EducationStudyForm)
                        .ToList();
                    employeeDTO.EmployeeEducation = edu.Select(e => e.ToDTO());

                    List<EmployeeDrivingLicense> licenses = 
                        db.EmployeeDrivingLicenses
                        .Where(d => d.Deleted != true && d.EmployeeId == id)
                        .ToList();
                    employeeDTO.EmployeeDrivingLicense = licenses.Select(l => l.ToDTO());

                    List<EmployeeContact> contacts = 
                        db.EmployeeContacts
                        .Where(c => c.Deleted != true && c.EmployeeId == id)
                        .Include(d => d.ContactType)
                        .ToList();
                    employeeDTO.EmployeeContact = contacts.Select(l => l.ToDTO());

                    List<EmployeeCar> cars = db.EmployeeCars.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
                    employeeDTO.EmployeeCar = cars.Select(c => c.ToDTO());
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(employeeDTO);
        }


        /// <summary>
        /// Get employee for EDIT
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeeEditDTO, contains only info about Employee</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEdit", Name = "GetEmployeeEdit")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult GetEmployeeEdit(int id)
        {
            Employee employee = new Employee();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employee).Reference(d => d.Address).Load();
                    if (employee.Address != null)
                    {
                        db.Entry(employee.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(employee.Address).Reference(d => d.Country).Load();
                    }

                    if (employee.AddressId != null && employee.AddressId != 0)
                    {
                        employee.Address = db.Addresses.Find(employee.AddressId);
                    }
                    if (employee.AddressId != null)
                    {
                        employee.Address = db.Addresses.Find(employee.AddressId);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeEditDTO dto = employee.ToDTOEdit();
            return Ok(dto);
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <param name="employee">Json employee object</param>
        /// <returns>204 (HttpStatusCode.NoContent)</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employee.Id)
            {
                return BadRequest();
            }

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    db.Entry(employee.Address).State = EntityState.Modified;
                    db.Entry(employee).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        if (!EmployeeExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    employee = db.Employees.Find(employee.Id);
                    db.Entry(employee).Reference(d => d.Address).Load();
                    if (employee.Address != null)
                    {
                        db.Entry(employee.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(employee.Address).Reference(d => d.Country).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            EmployeeEditDTO dto = employee.ToDTOEdit();
            return Ok(dto);
        }

        /// <summary>
        /// Insert new Employee
        /// </summary>
        /// <param name="employee">Employee data with Id = 0</param>
        /// <returns>route api/employees/{id}</returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employee.Id = employee.NewId(db);
                    employee.Address.Id = employee.Address.NewId(db);
                    employee.AddressId = employee.Address.Id;

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    db.Addresses.Add(employee.Address);
                    db.Employees.Add(employee);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (EmployeeExists(employee.Id))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    employee = db.Employees.Find(employee.Id);
                    db.Entry(employee).Reference(d => d.Address).Load();
                    if (employee.Address != null)
                    {
                        db.Entry(employee.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(employee.Address).Reference(d => d.Country).Load();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            EmployeeEditDTO dto = employee.ToDTOEdit();
            return CreatedAtRoute("GetEmployeeEdit", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete employee by id
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = new Employee();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return NotFound();
                    }
                    db.Entry(employee).Reference(d => d.Address).Load();
                    if (employee.Address != null)
                    {
                        db.Entry(employee.Address).Reference(d => d.AddressLocality).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressPlace).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressRegion).Load();
                        db.Entry(employee.Address).Reference(d => d.AddressVillage).Load();
                        db.Entry(employee.Address).Reference(d => d.Country).Load();
                    }
                    employee.Deleted = true;
                    db.Entry(employee).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(id))
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

            EmployeeEditDTO dto = employee.ToDTOEdit();  
            return Ok(dto);
        }

        //do we need this?
        ///// <summary>
        ///// List of Text and Value of enum Sex
        ///// </summary>
        ///// <returns>Collection of Json objects</returns>
        //[HttpGet]
        //[Route("GetSex")]
        //public IEnumerable<EnumItem> GetSexTypes()
        //{
        //    var sexList = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => new EnumItem
        //    {
        //        Text = v.ToString(),
        //        Value = (Int32)v
        //    }).ToList();
        //    return sexList;
        //}

        /// <summary>
        /// List of Text and Value of enum Sex
        /// </summary>
        /// <returns>Collection of Json objects</returns>
        [HttpGet]
        [Route("GetLists")]
        [ResponseType(typeof(EmployeeList))]
        public IHttpActionResult GetEmployeeLists()
        {
            EmployeeList list = new EmployeeList();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    list.AddressList = AddressList.CreateAddressList(db);
                    list.EmployeeLanguageList = EmployeeLanguageList.CreateEmployeeLanguageList(db);
                    list.EmployeeOfficeList = new EmployeeOfficeList();
                    //Employee Office data
                    Mapper.Initialize(m =>
                    {
                        m.CreateMap<Office, OfficeDTO>();
                    });
                    list.EmployeeOfficeList.Offices =
                        Mapper.Map<IEnumerable<Office>, IEnumerable<OfficeDTO>>(db.Offices.ToList());
                    Mapper.Initialize(m =>
                    {
                        m.CreateMap<Profession, ProfessionDTO>();
                    });
                    list.EmployeeOfficeList.Professions =
                        Mapper.Map<IEnumerable<Profession>, IEnumerable<ProfessionDTO>>(db.Professions.ToList());
                    Mapper.Initialize(m =>
                    {
                        m.CreateMap<Department, DepartmentDTO>();
                    });
                    list.EmployeeOfficeList.Departments =
                        Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDTO>>(db.Departments.ToList());


                    Mapper.Initialize(m =>
                    {
                        m.CreateMap<ContactType, ContactTypeDTO>();
                    });
                    list.ContactTypes =
                        Mapper.Map<IEnumerable<ContactType>, IEnumerable<ContactTypeDTO>>(db.ContactTypes.ToList());


                    Mapper.Initialize(m =>
                    {
                        m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
                    });

                    list.FoundationDocuments =
                        Mapper.Map<IEnumerable<FoundationDocument>, IEnumerable<FoundationDocumentDTO>>(db.FoundationDocuments.ToList());


                    Mapper.Initialize(m =>
                    {
                        m.CreateMap<EducationStudyForm, EducationStudyFormDTO>();
                    });
                    list.EducationStudyForms =
                        Mapper.Map<IEnumerable<EducationStudyForm>, IEnumerable<EducationStudyFormDTO>>(db.EducationStudyForms.ToList());
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(list);
        }

        /// <summary>
        /// Dispose Conroller (to destroy DbContect connection)
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.Employees.Count(e => e.Id == id) > 0;
            }
        }
    }
}