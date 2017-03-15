using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Context;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GTIWebAPI.Models.Dictionary;
using AutoMapper;
using GTIWebAPI.Models.Personnel;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private IDbContextFactory factory;

        public EmployeesRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public Employee Delete(int id)
        {
            Employee employee = new Employee();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employee =
                db.Employees
                .Where(d => d.Id == id)
                .Include(d => d.Address)
                .Include(d => d.Address.AddressLocality)
                .Include(d => d.Address.AddressPlace)
                .Include(d => d.Address.AddressRegion)
                .Include(d => d.Address.AddressVillage)
                .Include(d => d.Address.Country)
                .FirstOrDefault();
                if (employee == null)
                {
                    throw new ArgumentException();
                }
                employee.Deleted = true;
                db.MarkAsModified(employee);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employee;
        }

        public Employee GetEdit(int id)
        {
            Employee employee = new Employee();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employee =
                db.Employees
                .Where(d => d.Id == id)
                .Include(d => d.Address)
                .Include(d => d.Address.AddressLocality)
                .Include(d => d.Address.AddressPlace)
                .Include(d => d.Address.AddressRegion)
                .Include(d => d.Address.AddressVillage)
                .Include(d => d.Address.Country)
                .FirstOrDefault();
                if (employee == null)
                {
                    throw new ArgumentException();
                }
            }
            return employee;
        }

        public List<EmployeeView> GetAll(List<int> officeIds)
        {
            List<EmployeeView> employeeList = new List<EmployeeView>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeList = db.EmployeeByOffices(officeIds).ToList();
                if (employeeList != null)
                {
                    foreach (var item in employeeList)
                    {
                        item.EmployeeContacts =
                            db.EmployeeContacts
                            .Where(d => d.Deleted != true && d.EmployeeId == item.Id)
                            .Include(d => d.ContactType)
                            .ToList()
                            .Select(d => d.ToDTO())
                            .ToList();
                    }
                }
            }
            return employeeList;
        }

        //дикая чушь
        //в репозитории нельзя возвращать объекты DTO
        //переписать как будет время 
        //убрать так же автомаппер - тормозит и лучше руками написать ToDTO() и FromDTO() 
        public EmployeeList GetList()
        {
            EmployeeList list = new EmployeeList();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                list.AddressList = AddressList.CreateAddressList(db);
                list.EmployeeLanguageList = EmployeeLanguageList.CreateEmployeeLanguageList(db);
                list.EmployeeOfficeList = new EmployeeOfficeList();
                
                //Employee Office data
                Mapper.Initialize(m =>
                {
                    m.CreateMap<Office, OfficeDTO>();
                });
                list.EmployeeOfficeList.Offices = db.Offices.ToList().Select(d => d.ToDTO());


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
            return list;
        }

        public Employee GetView(int id)
        {
            Employee employee = new Employee();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employee =
                db.Employees
                .Where(d => d.Id == id)
                .Include(d => d.Address)
                .Include(d => d.Address.AddressLocality)
                .Include(d => d.Address.AddressPlace)
                .Include(d => d.Address.AddressRegion)
                .Include(d => d.Address.AddressVillage)
                .Include(d => d.Address.Country)
                .FirstOrDefault();

                if (employee == null)
                {
                    throw new ArgumentException();
                }

                employee.EmployeeOffices =
                    db.EmployeeOffices
                    .Where(o => o.Deleted != true && o.EmployeeId == id)
                    .Include(d => d.Department)
                    .Include(d => d.Office)
                    .Include(d => d.Profession)
                    .ToList();

                employee.EmployeePassports =
                    db.EmployeePassports
                    .Where(p => p.Deleted != true && p.EmployeeId == id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .Include(d => d.Address.Country)
                    .ToList();

                employee.EmployeeMilitaryCards =
                        db.EmployeeMilitaryCards
                        .Where(m => m.Deleted != true && m.EmployeeId == id)
                        .ToList();

                employee.EmployeeLanguages =
                     db.EmployeeLanguages
                     .Where(l => l.Deleted != true && l.EmployeeId == id)
                     .Include(d => d.EmployeeLanguageType)
                     .Include(d => d.Language)
                     .ToList();

                employee.EmployeeInternationalPassports =
                    db.EmployeeInternationalPassports
                    .Where(p => p.Deleted != true && p.EmployeeId == id)
                    .ToList();

                employee.EmployeeGuns =
                    db.EmployeeGun
                    .Where(g => g.Deleted != true && g.EmployeeId == id)
                    .ToList();

                employee.EmployeeFoundationDocuments =
                    db.EmployeeFoundationDocuments
                    .Where(d => d.Deleted != true && d.EmployeeId == id)
                    .Include(d => d.FoundationDocument)
                    .ToList();

                employee.EmployeeEducations =
                    db.EmployeeEducations
                    .Where(e => e.Deleted != true && e.EmployeeId == id)
                    .Include(d => d.EducationStudyForm)
                    .ToList();

                employee.EmployeeDrivingLicenses =
                    db.EmployeeDrivingLicenses
                    .Where(d => d.Deleted != true && d.EmployeeId == id)
                    .ToList();

                employee.EmployeeContacts =
                    db.EmployeeContacts
                    .Where(c => c.Deleted != true && c.EmployeeId == id)
                    .Include(d => d.ContactType)
                    .ToList();

                employee.EmployeeCars =
                    db.EmployeeCars
                    .Where(c => c.Deleted != true && c.EmployeeId == id)
                    .ToList();
            }
            return employee;
        }


        public Employee Add(Employee employee)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employee.Id = employee.NewId(db);
                employee.Address.Id = employee.Address.NewId(db);
                employee.AddressId = employee.Address.Id;
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
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employee =
                db.Employees
                .Where(d => d.Id == employee.Id)
                .Include(d => d.Address)
                .Include(d => d.Address.AddressLocality)
                .Include(d => d.Address.AddressPlace)
                .Include(d => d.Address.AddressRegion)
                .Include(d => d.Address.AddressVillage)
                .Include(d => d.Address.Country)
                .FirstOrDefault();
            }
            return employee;
        }

        public Employee Edit(Employee employee)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employee.Address);
                db.MarkAsModified(employee);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employee =
                db.Employees
                .Where(d => d.Id == employee.Id)
                .Include(d => d.Address)
                .Include(d => d.Address.AddressLocality)
                .Include(d => d.Address.AddressPlace)
                .Include(d => d.Address.AddressRegion)
                .Include(d => d.Address.AddressVillage)
                .Include(d => d.Address.Country)
                .FirstOrDefault();
            }
            return employee;
        }

        private bool EmployeeExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.Employees.Count(e => e.Id == id) > 0;
            }
        }
    }
}
