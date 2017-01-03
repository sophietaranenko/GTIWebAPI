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
        /// Get all filtered employees
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
        [Route("GetEmployeeView")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult GetEmployeeView(int id)
        {
            //метод без использования маппера
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            EmployeeDTO employeeDTO =
                new EmployeeDTO()
                {
                    Id = employee.Id,
                    IdentityCode = employee.IdentityCode,
                    Sex = employee.Sex,
                    DateOfBirth = employee.DateOfBirth,
                    Age = employee.Age.ToString(),
                    ProfilePicture = employee.ProfilePicture,
                    AddressId = employee.AddressId
                };
            Address address = db.Address.Find(employee.AddressId);
            employeeDTO.Address = new AddressDTO
            {
                Id = address.Id,
                Apartment = address.Apartment,
                BuildingNumber = address.BuildingNumber,
                Country = address.Country,
                Housing = address.Housing,
                LocalityName = address.LocalityName,
                LocalityType = address.LocalityType,
                LocalityTypeString = address.LocalityTypeString,
                PlaceName = address.PlaceName,
                PlaceType = address.PlaceType,
                PlaceTypeString = address.PlaceTypeString,
                PostIndex = address.PostIndex,
                RegionName = address.RegionName,
                RegionType = address.RegionType,
                RegionTypeString = address.RegionTypeString,
                VillageName = address.VillageName,
                VillageType = address.VillageType,
                VillageTypeString = address.VillageTypeString
            };

            List<EmployeeOffice> offices = db.EmployeeOffice.Where(o => o.Deleted != true && o.EmployeeId == id).ToList();
            employeeDTO.EmployeeOffice = offices.Select(o =>
                new EmployeeOfficeDTO
                {
                    Id = o.Id,
                    Office = new OfficeDTO { Id = o.Office.Id, ShortName = o.Office.NativeName },
                    DateBegin = o.DateBegin,
                    DateEnd = o.DateEnd,
                    Department = new DepartmentDTO { Id = o.Department.Id, Name = o.Department.Name },
                    EmployeeId = o.EmployeeId,
                    Profession = new ProfessionDTO { Id = o.Profession.Id, Name = o.Profession.Name },
                    Remark = o.Remark
                });

            List<EmployeePassport> passports = db.EmployeePassport.Where(p => p.Deleted != true && p.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            employeeDTO.EmployeePassport = Mapper.Map<IEnumerable<EmployeePassportDTO>>(passports);

            List<EmployeeMilitaryCard> cards = db.EmployeeMilitaryCard.Where(m => m.Deleted != true && m.EmployeeId == id).ToList();
            employeeDTO.EmployeeMilitaryCard = cards.Select(m =>
                new EmployeeMilitaryCardDTO
                {
                    Category = m.Category,
                    Corps = m.Corps,
                    EmployeeId = m.EmployeeId,
                    Id = m.Id,
                    Number = m.Number,
                    Office = m.Office,
                    OfficeDate = m.OfficeDate,
                    Rank = m.Rank,
                    Seria = m.Seria,
                    Specialty = m.Specialty,
                    SpecialtyNumber = m.SpecialtyNumber,
                    TypeGroup = m.TypeGroup
                });

            List<EmployeeLanguage> languages = db.EmployeeLanguage.Where(l => l.Deleted != true && l.EmployeeId == id).ToList();
            employeeDTO.EmployeeLanguage = languages.Select(l =>
                new EmployeeLanguageDTO
                {
                    Id = l.Id,
                    DateBegin = l.DateBegin,
                    DateEnd = l.DateEnd,
                    Definition = l.Definition,
                    EmployeeId = l.EmployeeId,
                    Language = new LanguageDTO { Id = l.Language.Id, Name = l.Language.Name },
                    Remark = l.Remark,
                    Type = l.Type
                });



            List<EmployeeInternationalPassport> iPassports = db.EmployeeInternationalPassport.Where(p => p.Deleted != true && p.EmployeeId == id).ToList();
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


            List<EmployeeFoundationDoc> docs = db.EmployeeFoundationDoc.Where(d => d.Deleted != true && d.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDoc, EmployeeFoundationDocDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            employeeDTO.EmployeeFoundationDoc = Mapper.Map<IEnumerable<EmployeeFoundationDocDTO>>(docs);

            List<EmployeeEducation> edu = db.EmployeeEducation.Where(e => e.Deleted != true && e.EmployeeId == id).ToList();
            employeeDTO.EmployeeEducation = edu.Select(e =>
                new EmployeeEducationDTO
                {
                    EmployeeId = e.EmployeeId,
                    Id = e.Id,
                    Institution = e.Institution,
                    Number = e.Number,
                    Qualification = e.Qualification,
                    Seria = e.Seria,
                    Specialty = e.Specialty,
                    StudyForm = e.StudyForm,
                    Year = e.Year
                });

            List<EmployeeDrivingLicense> licenses = db.EmployeeDrivingLicense.Where(d => d.Deleted != true && d.EmployeeId == id).ToList();
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

            List<EmployeeContact> contacts = db.EmployeeContact.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            employeeDTO.EmployeeContact = Mapper.Map<IEnumerable<EmployeeContactDTO>>(contacts);

            List<EmployeeCar> cars = db.EmployeeCar.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
            employeeDTO.EmployeeCar = cars.Select(c =>
                new EmployeeCarDTO
                {
                    Capacity = c.Capacity,
                    Colour = c.Colour,
                    Description = c.Description,
                    VehicleCategory = c.VehicleCategory,
                    EmployeeId = c.EmployeeId,
                    FuelType = c.FuelType,
                    GivenName = c.GivenName,
                    Id = c.Id,
                    IdentificationNumber = c.IdentificationNumber,
                    IssuedBy = c.IssuedBy,
                    Make = c.Make,
                    MassInService = c.MassInService,
                    MassMax = c.MassMax,
                    Number = c.Number,
                    NumberOfSeats = c.NumberOfSeats,
                    Owner = c.Owner,
                    Ownership = c.Ownership,
                    PeriodOfValidity = c.PeriodOfValidity,
                    RegistrationDate = c.RegistrationDate,
                    RegistrationNumber = c.RegistrationNumber,
                    RegistrationYear = c.RegistrationYear,
                    Seria = c.Seria,
                    Type = c.Type
                });

            return Ok(employeeDTO);
        }

        /// <summary>
        /// Get employee for VIEW by id (with mapper using)
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeeDTO, contains info abount employee and its documents</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEmployeeMapperView")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult GetEmployeeViewWithMapper(int id)
        {
            //метод с использованием маппера
            //маппер нельзя использовать в EmployeeDTO - он попытается замапить все внутриенние IEnumerable
            //а нам надо сделать Deleted != true
            //маппер добавляет ~ 150ms

            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            EmployeeDTO employeeDTO =
                new EmployeeDTO()
                {
                    Id = employee.Id,
                    IdentityCode = employee.IdentityCode,
                    Sex = employee.Sex,
                    DateOfBirth = employee.DateOfBirth,
                    Age = employee.Age.ToString(),
                    ProfilePicture = employee.ProfilePicture,
                    //UserId = employee.UserId
                };

            Address address = db.Address.Find(employee.AddressId);
            employeeDTO.Address = new AddressDTO
            {
                Id = address.Id,
                Apartment = address.Apartment,
                BuildingNumber = address.BuildingNumber,
                Country = address.Country,
                Housing = address.Housing,
                LocalityName = address.LocalityName,
                LocalityType = address.LocalityType,
                LocalityTypeString = address.LocalityTypeString,
                PlaceName = address.PlaceName,
                PlaceType = address.PlaceType,
                PlaceTypeString = address.PlaceTypeString,
                PostIndex = address.PostIndex,
                RegionName = address.RegionName,
                RegionType = address.RegionType,
                RegionTypeString = address.RegionTypeString,
                VillageName = address.VillageName,
                VillageType = address.VillageType,
                VillageTypeString = address.VillageTypeString
            };

            List<EmployeeOffice> offices = db.EmployeeOffice.Where(o => o.Deleted != true && o.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeOffice, EmployeeOfficeDTO>();
                m.CreateMap<Office, OfficeDTO>();
                m.CreateMap<Department, DepartmentDTO>();
                m.CreateMap<Profession, ProfessionDTO>();
            });
            employeeDTO.EmployeeOffice = Mapper.Map<IEnumerable<EmployeeOfficeDTO>>(offices);

            List<EmployeePassport> passports = db.EmployeePassport.Where(p => p.Deleted != true && p.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeePassport, EmployeePassportDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            employeeDTO.EmployeePassport = Mapper.Map<IEnumerable<EmployeePassportDTO>>(passports);

            List<EmployeeMilitaryCard> cards = db.EmployeeMilitaryCard.Where(m => m.Deleted != true && m.EmployeeId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<EmployeeMilitaryCard, EmployeeMilitaryCardDTO>());
            employeeDTO.EmployeeMilitaryCard = Mapper.Map<IEnumerable<EmployeeMilitaryCardDTO>>(cards);


            List<EmployeeLanguage> languages = db.EmployeeLanguage.Where(l => l.Deleted != true && l.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeLanguage, EmployeeLanguageDTO>();
                m.CreateMap<Language, LanguageDTO>();
            });
            employeeDTO.EmployeeLanguage = Mapper.Map<IEnumerable<EmployeeLanguageDTO>>(languages);

            List<EmployeeInternationalPassport> iPassports = db.EmployeeInternationalPassport.Where(p => p.Deleted != true && p.EmployeeId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<EmployeeInternationalPassport, EmployeeInternationalPassportDTO>());
            employeeDTO.EmployeeInternationalPassport = Mapper.Map<IEnumerable<EmployeeInternationalPassportDTO>>(iPassports);

            List<EmployeeGun> guns = db.EmployeeGun.Where(g => g.Deleted != true && g.EmployeeId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<EmployeeGun, EmployeeGunDTO>());
            employeeDTO.EmployeeGun = Mapper.Map<IEnumerable<EmployeeGunDTO>>(guns);

            List<EmployeeFoundationDoc> docs = db.EmployeeFoundationDoc.Where(d => d.Deleted != true && d.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeFoundationDoc, EmployeeFoundationDocDTO>();
                m.CreateMap<FoundationDocument, FoundationDocumentDTO>();
            });
            employeeDTO.EmployeeFoundationDoc = Mapper.Map<IEnumerable<EmployeeFoundationDocDTO>>(docs);

            List<EmployeeEducation> edu = db.EmployeeEducation.Where(e => e.Deleted != true && e.EmployeeId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<EmployeeEducation, EmployeeEducationDTO>());
            employeeDTO.EmployeeEducation = Mapper.Map<IEnumerable<EmployeeEducationDTO>>(edu);


            List<EmployeeDrivingLicense> licenses = db.EmployeeDrivingLicense.Where(d => d.Deleted != true && d.EmployeeId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<EmployeeDrivingLicense, EmployeeDrivingLicenseDTO>());
            employeeDTO.EmployeeDrivingLicense = Mapper.Map<IEnumerable<EmployeeDrivingLicenseDTO>>(licenses);

            List<EmployeeContact> contacts = db.EmployeeContact.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<EmployeeContact, EmployeeContactDTO>();
                m.CreateMap<ContactType, ContactTypeDTO>();
            });
            employeeDTO.EmployeeContact = Mapper.Map<IEnumerable<EmployeeContactDTO>>(contacts);

            List<EmployeeCar> cars = db.EmployeeCar.Where(c => c.Deleted != true && c.EmployeeId == id).ToList();
            Mapper.Initialize(m => m.CreateMap<EmployeeCar, EmployeeCarDTO>());
            employeeDTO.EmployeeCar = Mapper.Map<IEnumerable<EmployeeCarDTO>>(cars);

            return Ok(employeeDTO);
        }

        /// <summary>
        /// Get employee for EDIT
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeeEditDTO, contains only info about Employee</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEmployeeEdit", Name = "GetEmployeeEdit")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult GetEdit(int id)
        {
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.AddressId != null && employee.AddressId != 0)
            {
                employee.Address = db.Address.Find(employee.AddressId);
            }
            Mapper.Initialize(c =>
            {
                c.CreateMap<Employee, EmployeeEditDTO>();
                c.CreateMap<Address, AddressDTO>();
            });
            EmployeeEditDTO dto = Mapper.Map<EmployeeEditDTO>(employee);
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
        [Route("PutEmployee")]
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
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Insert new Employee
        /// </summary>
        /// <param name="employee">Employee data with Id = 0</param>
        /// <returns>route api/employees/{id}</returns>
        [GTIFilter]
        [HttpPost]
        [Route("PostEmployee")]
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
            //if (!Enum.IsDefined(typeof(Sex), employee.Sex))
            //{
            //    return BadRequest(ModelState);
            //}
            db.Address.Add(employee.Address);
            db.Employee.Add(employee);
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
            Mapper.Initialize(m =>
            {
                m.CreateMap<Employee, EmployeeEditDTO>();
                m.CreateMap<Address, AddressDTO>();
            });
            EmployeeEditDTO dto = Mapper.Map<EmployeeEditDTO>(employee);
            return CreatedAtRoute("GetEmployeeEdit", new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Delete employee by id
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteEmployee")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employee.Find(id);
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

        [HttpGet]
        [Route("GetLists")]
        [ResponseType(typeof(EmployeeList))]
        public IHttpActionResult GetList()
        {
            EmployeeList list = new EmployeeList();

            List<Office> offices = db.Offices.ToList();
            Mapper.Initialize(m => m.CreateMap<Office, OfficeDTO>());
            list.Offices = Mapper.Map<IEnumerable<Office>, IEnumerable<OfficeDTO>>(offices);

            List<Profession> professions = db.Profession.Where(p => p.Deleted != true).ToList();
            Mapper.Initialize(m => m.CreateMap<Profession, ProfessionDTO>());
            list.Professions = Mapper.Map<IEnumerable<Profession>, IEnumerable<ProfessionDTO>>(professions);

            List<Department> departments = db.Department.Where(d => d.Deleted != true).ToList();
            Mapper.Initialize(m => m.CreateMap<Department, DepartmentDTO>());
            list.Departments = Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDTO>>(departments);

            List<FoundationDocument> docs = db.FoundationDocument.Where(d => d.Deleted != true).ToList();
            Mapper.Initialize(m => m.CreateMap<FoundationDocument, FoundationDocumentDTO>());
            list.DocumentTypes = Mapper.Map<IEnumerable<FoundationDocument>, IEnumerable<FoundationDocumentDTO>>(docs);

            List<ContactType> types = db.ContactType.Where(c => c.Deleted != true).ToList();
            Mapper.Initialize(m => m.CreateMap<ContactType, ContactTypeDTO>());
            list.ContactTypes = Mapper.Map<IEnumerable<ContactType>, IEnumerable<ContactTypeDTO>>(types);

            List<Language> languages = db.Language.ToList();
            Mapper.Initialize(m =>
            {
                m.CreateMap<Language, LanguageDTO>();
            });
            list.Languages = Mapper.Map<IEnumerable<Language>, IEnumerable<LanguageDTO>>(languages);

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
            return db.Employee.Count(e => e.Id == id) > 0;
        }
    }
}