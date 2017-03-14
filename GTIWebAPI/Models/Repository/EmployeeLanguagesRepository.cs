using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeLanguagesRepository : IRepository<EmployeeLanguage>
    {
        private IDbContextFactory factory;
        public EmployeeLanguagesRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeLanguagesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeLanguage Add(EmployeeLanguage employeeLanguage)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeLanguage.Id = employeeLanguage.NewId(db);
                db.EmployeeLanguages.Add(employeeLanguage);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeLanguageExists(employeeLanguage.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeLanguage = db.EmployeeLanguages
                    .Where(d => d.Id == employeeLanguage.Id)
                    .Include(d => d.Language)
                    .Include(d => d.EmployeeLanguageType)
                    .FirstOrDefault();
            }
            return employeeLanguage;
        }

        public EmployeeLanguage Delete(int id)
        {
            EmployeeLanguage employeeLanguage = new EmployeeLanguage();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeLanguage = db.EmployeeLanguages
                    .Where(d => d.Id == employeeLanguage.Id)
                    .Include(d => d.Language)
                    .Include(d => d.EmployeeLanguageType)
                    .FirstOrDefault();

                if (employeeLanguage == null)
                {
                    throw new ArgumentException();
                }

                employeeLanguage.Deleted = true;
                db.MarkAsModified(employeeLanguage);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeLanguageExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeLanguage = db.EmployeeLanguages
                    .Where(d => d.Id == employeeLanguage.Id)
                    .Include(d => d.Language)
                    .Include(d => d.EmployeeLanguageType)
                    .FirstOrDefault();
            }
            return employeeLanguage;
        }

        public EmployeeLanguage Edit(EmployeeLanguage employeeLanguage)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeLanguage);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeLanguageExists(employeeLanguage.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeLanguage = db.EmployeeLanguages
                                    .Where(d => d.Id == employeeLanguage.Id)
                                    .Include(d => d.Language)
                                    .Include(d => d.EmployeeLanguageType)
                                    .FirstOrDefault();
            }
            return employeeLanguage;
        }

        public EmployeeLanguage Get(int id)
        {
            EmployeeLanguage employeeLanguage = new EmployeeLanguage();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeLanguage = db.EmployeeLanguages
                    .Where(d => d.Id == employeeLanguage.Id)
                    .Include(d => d.Language)
                    .Include(d => d.EmployeeLanguageType)
                    .FirstOrDefault();
            }
            return employeeLanguage;
        }

        public List<EmployeeLanguage> GetAll()
        {
            List<EmployeeLanguage> list = new List<EmployeeLanguage>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeLanguages
                    .Where(p => p.Deleted != true)
                    .Include(d => d.EmployeeLanguageType)
                    .Include(d => d.Language)
                    .ToList();
            }
            return list;
        }

        public List<EmployeeLanguage> GetByEmployeeId(int employeeId)
        {
            List<EmployeeLanguage> list = new List<EmployeeLanguage>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeLanguages
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .Include(d => d.EmployeeLanguageType)
                    .Include(d => d.Language)
                    .ToList();
            }
            return list;
        }

        private bool EmployeeLanguageExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeLanguages.Count(e => e.Id == id) > 0;
            }
        }
    }
}
