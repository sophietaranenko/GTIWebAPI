using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeDrivingLicensesRepository : IRepository<EmployeeDrivingLicense>
    {
        private IDbContextFactory factory;

        public EmployeeDrivingLicensesRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeDrivingLicensesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeDrivingLicense Add(EmployeeDrivingLicense employeeDrivingLicense)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeDrivingLicense.Id = employeeDrivingLicense.NewId(db);
                db.EmployeeDrivingLicenses.Add(employeeDrivingLicense);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeDrivingLicenseExists(employeeDrivingLicense.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(employeeDrivingLicense.Id);
        }

        public EmployeeDrivingLicense Delete(int id)
        {
            EmployeeDrivingLicense employeeDrivingLicense = new EmployeeDrivingLicense();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeDrivingLicense = Get(id);
                if (employeeDrivingLicense == null)
                {
                    throw new ArgumentException();
                }
                employeeDrivingLicense.Deleted = true;
                db.MarkAsModified(employeeDrivingLicense);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeDrivingLicenseExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(id);
        }

        public EmployeeDrivingLicense Edit(EmployeeDrivingLicense employeeDrivingLicense)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeDrivingLicense);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeDrivingLicenseExists(employeeDrivingLicense.Id))
                    {
                        throw new ArgumentException();   
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(employeeDrivingLicense.Id);
        }

        public EmployeeDrivingLicense Get(int id)
        {
            EmployeeDrivingLicense license = new EmployeeDrivingLicense();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                license = db.EmployeeDrivingLicenses.Where(p => p.Id == id).FirstOrDefault();
            }
            return license;
        }

        public List<EmployeeDrivingLicense> GetAll()
        {
            List<EmployeeDrivingLicense> licenses = new List<EmployeeDrivingLicense>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                licenses = db.EmployeeDrivingLicenses
                    .Where(p => p.Deleted != true)
                    .ToList();
            }
            return licenses;
        }

        public List<EmployeeDrivingLicense> GetByEmployeeId(int employeeId)
        {
            List<EmployeeDrivingLicense> licenses = new List<EmployeeDrivingLicense>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                licenses = db.EmployeeDrivingLicenses
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .ToList();
            }
            return licenses;
        }

        private bool EmployeeDrivingLicenseExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeDrivingLicenses.Count(e => e.Id == id) > 0;
            }
        }
    }
}
