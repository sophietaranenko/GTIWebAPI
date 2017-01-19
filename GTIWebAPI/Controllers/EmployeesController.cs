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

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller manipulating employees
    /// </summary>
 //   [Authorize]
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// </summary>
        /// <param name="filter">
        /// "filter" is a one string contains different number of filters f.e., "Formag Администрация", "Софья Тараненко", "Тараненко Софья Verdeco"
        /// </param>
        /// <returns>a collection on EmployeeViewDTO objects</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeViewDTO> GetAll(string filter)
        {
            IEnumerable<EmployeeView> employeeList = db.EmployeeFilter(filter);
            IEnumerable<EmployeeViewDTO> dtos = employeeList.Select(e =>
            new EmployeeViewDTO
            {
                Id = e.Id,
                Age = e.Age.ToString(),
                DateOfBirth = e.DateOfBirth,
                AgeCount = e.AgeCount,
                FirstName = e.FirstName,
                IdentityCode = e.IdentityCode,
                Position = e.Position,
                PositionLines = e.PositionLines,
                SecondName = e.SecondName,
                ShortAddress = e.ShortAddress,
                Surname = e.Surname,
                UserName = e.UserName
            });
            return dtos;
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
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.AddressId != null)
            {
                employee.Address = db.Addresses.Find(employee.AddressId);
            }



            EmployeeDTO employeeDTO = employee.ToDTOView();

            List<EmployeeOffice> offices = db.EmployeeOffices.Where(o => o.Deleted != true && o.EmployeeId == id).ToList();
            employeeDTO.EmployeeOffice = offices.Select(o => o.ToDTO());

            List<EmployeePassport> passports = db.EmployeePassports.Where(p => p.Deleted != true && p.EmployeeId == id).ToList();
            employeeDTO.EmployeePassport = passports.Select(p => p.ToDTO()).ToList();
                
            List<EmployeeMilitaryCard> cards = db.EmployeeMilitaryCards.Where(m => m.Deleted != true && m.EmployeeId == id).ToList();
            employeeDTO.EmployeeMilitaryCard = cards.Select(m => m.ToDTO()).ToList();
                


            List<EmployeeLanguage> languages = db.EmployeeLanguages.Where(l => l.Deleted != true && l.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeLanguage, EmployeeLanguageDTO>();
                m.CreateMap<Language, LanguageDTO>();
                m.CreateMap<EmployeeLanguageType, EmployeeLanguageTypeDTO>();
            });

            employeeDTO.EmployeeLanguage = Mapper
                .Map<IEnumerable<EmployeeLanguage>, IEnumerable<EmployeeLanguageDTO>>
                (languages);



            List<EmployeeInternationalPassport> iPassports = db.EmployeeInternationalPassports.Where(p => p.Deleted != true && p.EmployeeId == id).ToList();
            employeeDTO.EmployeeInternationalPassport = iPassports.Select(i =>
                new EmployeeInternationalPassportDTO
                {
                    Id = i.Id,
                    CountryCode = i.CountryCode,
                    DateOfExpiry = i.DateOfExpiry,
                    EmployeeId = i.EmployeeId,
                    GivenNames = i.GivenNames,
                    IssuedBy = i.IssuedBy,
                    IssuedWhen = i.IssuedWhen,
                    Nationality = i.Nationality,
                    Number = i.Number,
                    PersonalNo = i.PersonalNo,
                    Seria = i.Seria,
                    Surname = i.Surname,
                    Type = i.Type
                });



            List<EmployeeGun> guns = db.EmployeeGun.Where(g => g.Deleted != true && g.EmployeeId == id).ToList();
            employeeDTO.EmployeeGun = guns.Select(g =>
                new EmployeeGunDTO
                {
                    Id = g.Id,
                    DateEnd = g.DateEnd,
                    Description = g.Description,
                    EmployeeId = g.EmployeeId,
                    IssuedBy = g.IssuedBy,
                    IssuedWhen = g.IssuedWhen,
                    Number = g.Number,
                    Seria = g.Seria
                });


            List<EmployeeFoundationDocument> docs = db.EmployeeFoundationDocuments.Where(d => d.Deleted != true && d.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDocument, EmployeeFoundationDocumentDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            employeeDTO.EmployeeFoundationDoc = Mapper.Map<IEnumerable<EmployeeFoundationDocumentDTO>>(docs);

            List<EmployeeEducation> edu = db.EmployeeEducations.Where(e => e.Deleted != true && e.EmployeeId == id).ToList();
            employeeDTO.EmployeeEducation = edu.Select(e => e.ToDTO());

            List<EmployeeDrivingLicense> licenses = db.EmployeeDrivingLicenses.Where(d => d.Deleted != true && d.EmployeeId == id).ToList();
            employeeDTO.EmployeeDrivingLicense = licenses.Select(l =>
                 new EmployeeDrivingLicenseDTO
                 {
                     Category = l.Category,
                     EmployeeId = l.EmployeeId,
                     ExpiryDate = l.ExpiryDate,
                     Id = l.Id,
                     IssuedBy = l.IssuedBy,
                     IssuedWhen = l.IssuedWhen,
                     Number = l.Number,
                     Seria = l.Seria
                 });

            List<EmployeeContact> contacts = db.EmployeeContacts.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            employeeDTO.EmployeeContact = Mapper.Map<IEnumerable<EmployeeContactDTO>>(contacts);

            List<EmployeeCar> cars = db.EmployeeCars.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
            employeeDTO.EmployeeCar = cars.Select(c => c.ToDTO());

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
        public IHttpActionResult GetEdit(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.AddressId != null && employee.AddressId != 0)
            {
                employee.Address = db.Addresses.Find(employee.AddressId);
            }
            if (employee.AddressId != null)
            {
                employee.Address = db.Addresses.Find(employee.AddressId);
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
            
            if (employee.AddressId != null)
            {
                employee.Address = db.Addresses.Find(employee.AddressId);

                if (employee.Address.PlaceId != null)
                {
                    employee.Address.AddressPlace = db.Places.Find(employee.Address.PlaceId);
                }
                if (employee.Address.LocalityId != null)
                {
                    employee.Address.AddressLocality = db.Localities.Find(employee.Address.LocalityId);
                }
                if (employee.Address.VillageId != null)
                {
                    employee.Address.AddressVillage = db.Villages.Find(employee.Address.VillageId);
                }
                if (employee.Address.RegionId != null)
                {
                    employee.Address.AddressRegion = db.Regions.Find(employee.Address.RegionId);
                }
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
        public IHttpActionResult EmployeeInsert(Employee employee)
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

            if (employee.AddressId != null)
            {
                employee.Address = db.Addresses.Find(employee.AddressId);

                if (employee.Address.PlaceId != null)
                {
                    employee.Address.AddressPlace = db.Places.Find(employee.Address.PlaceId);
                }
                if (employee.Address.LocalityId != null)
                {
                    employee.Address.AddressLocality = db.Localities.Find(employee.Address.LocalityId);
                }
                if (employee.Address.VillageId != null)
                {
                    employee.Address.AddressVillage = db.Villages.Find(employee.Address.VillageId);
                }
                if (employee.Address.RegionId != null)
                {
                    employee.Address.AddressRegion = db.Regions.Find(employee.Address.RegionId);
                }
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
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<Employee, EmployeeEditDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeeEditDTO dto = Mapper.Map<EmployeeEditDTO>(employee);
            return Ok(dto);
        }

        /// <summary>
        /// List of Text and Value of enum Sex
        /// </summary>
        /// <returns>Collection of Json objects</returns>
        [HttpGet]
        [Route("GetSex")]
        public IEnumerable<EnumItem> GetSexTypes()
        {
            var sexList = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => new EnumItem
            {
                Text = v.ToString(),
                Value = (Int32)v
            }).ToList();
            return sexList;
        }

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

            Mapper.Initialize(m =>
            {
                m.CreateMap<AddressLocality, AddressLocalityDTO>();
            });
            list.AddressLocalities =
                Mapper.Map<IEnumerable<AddressLocality>, IEnumerable<AddressLocalityDTO>>(db.Localities.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<AddressPlace, AddressPlaceDTO>();
            });
            list.AddressPlaces =
                Mapper.Map<IEnumerable<AddressPlace>, IEnumerable<AddressPlaceDTO>>(db.Places.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<AddressRegion, AddressRegionDTO>();
            });
            list.AddressRegions =
                Mapper.Map<IEnumerable<AddressRegion>, IEnumerable<AddressRegionDTO>>(db.Regions.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<AddressVillage, AddressVillageDTO>();
            });
            list.AddressVillages =
                Mapper.Map<IEnumerable<AddressVillage>, IEnumerable<AddressVillageDTO>>(db.Villages.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            list.ContactTypes =
                Mapper.Map<IEnumerable<ContactType>, IEnumerable<ContactTypeDTO>>(db.ContactTypes.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<Department, DepartmentDTO>();
            });
            list.Departments =
                Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDTO>>(db.Departments.ToList());

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

            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeLanguageType, EmployeeLanguageTypeDTO>();
            });
            list.EmployeeLanguageTypes =
                Mapper.Map<IEnumerable<EmployeeLanguageType>, IEnumerable<EmployeeLanguageTypeDTO>>(db.EmployeeLanguageTypes.ToList());


            Mapper.Initialize(m =>
            {
                m.CreateMap<Language, LanguageDTO>();
            });
            list.Languages =
                Mapper.Map<IEnumerable<Language>, IEnumerable<LanguageDTO>>(db.Languages.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<Office, OfficeDTO>();
            });
            list.Offices =
                Mapper.Map<IEnumerable<Office>, IEnumerable<OfficeDTO>>(db.Offices.ToList());

            Mapper.Initialize(m =>
            {
                m.CreateMap<Profession, ProfessionDTO>();
            });
            list.Professions =
                Mapper.Map<IEnumerable<Profession>, IEnumerable<ProfessionDTO>>(db.Professions.ToList());

            return Ok(list);
        }

        /// <summary>
        /// Dispose Conroller (to destroy DbContect connection)
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

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }
    }
}