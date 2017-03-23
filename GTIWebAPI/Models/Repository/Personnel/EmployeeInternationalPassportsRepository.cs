using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeInternationalPassportsRepository : IRepository<EmployeeInternationalPassport>
    {
        private IDbContextFactory factory;
        public EmployeeInternationalPassportsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeInternationalPassportsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeInternationalPassport Add(EmployeeInternationalPassport employeeInternationalPassport)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeInternationalPassport.Id = employeeInternationalPassport.NewId(db);
                db.EmployeeInternationalPassports.Add(employeeInternationalPassport);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeInternationalPassportExists(employeeInternationalPassport.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeInternationalPassport = 
                    db.EmployeeInternationalPassports
                    .Where(d => d.Id == employeeInternationalPassport.Id)
                    .FirstOrDefault();
            }
            return employeeInternationalPassport;
        }

        public EmployeeInternationalPassport Delete(int id)
        {
            EmployeeInternationalPassport employeeInternationalPassport = new EmployeeInternationalPassport();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeInternationalPassport = 
                    db.EmployeeInternationalPassports
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
                if (employeeInternationalPassport == null)
                {
                    throw new NotFoundException();
                }
                employeeInternationalPassport.Deleted = true;
                db.MarkAsModified(employeeInternationalPassport);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeInternationalPassportExists(id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeInternationalPassport;
        }

        public EmployeeInternationalPassport Edit(EmployeeInternationalPassport employeeInternationalPassport)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeInternationalPassport);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeInternationalPassportExists(employeeInternationalPassport.Id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeInternationalPassport =
                    db.EmployeeInternationalPassports
                    .Where(d => d.Id == employeeInternationalPassport.Id)
                    .FirstOrDefault();
            }
            return employeeInternationalPassport;
        }

        public EmployeeInternationalPassport Get(int id)
        {
            EmployeeInternationalPassport employeeInternationalPassport = new EmployeeInternationalPassport();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeInternationalPassport = 
                    db.EmployeeInternationalPassports
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
            }
            if (employeeInternationalPassport == null)
            {
                throw new NotFoundException();
            }
            return employeeInternationalPassport;
        }

        public List<EmployeeInternationalPassport> GetAll()
        {
            List<EmployeeInternationalPassport> passports = new List<EmployeeInternationalPassport>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passports = db.EmployeeInternationalPassports.Where(p => p.Deleted != true).ToList();
            }
            if (passports == null)
            {
                throw new NotFoundException();
            }
            return passports;
        }

        public List<EmployeeInternationalPassport> GetByEmployeeId(int employeeId)
        {
            List<EmployeeInternationalPassport> passports = new List<EmployeeInternationalPassport>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passports =
                 db.EmployeeInternationalPassports
                 .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                 .ToList(); 
            }
            if (passports == null)
            {
                throw new NotFoundException();
            }
            return passports;
        }

        private bool EmployeeInternationalPassportExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeInternationalPassports.Count(e => e.Id == id) > 0;
            }
        }
    }
}
