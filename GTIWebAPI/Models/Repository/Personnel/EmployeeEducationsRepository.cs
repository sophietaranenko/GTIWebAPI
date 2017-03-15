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
    public class EmployeeEducationsRepository : IRepository<EmployeeEducation>
    {
        private IDbContextFactory factory;
        public EmployeeEducationsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeEducationsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeEducation Add(EmployeeEducation employeeEducation)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeEducation.Id = employeeEducation.NewId(db);
                db.EmployeeEducations.Add(employeeEducation);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeEducationExists(employeeEducation.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(employeeEducation.Id);
        }

        public EmployeeEducation Delete(int id)
        {
            EmployeeEducation employeeEducation = new EmployeeEducation();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeEducation = db.EmployeeEducations
                    .Where(d => d.Id == id)
                    .Include(d => d.EducationStudyForm)
                    .FirstOrDefault();

                if (employeeEducation == null)
                {
                    throw new ArgumentException();
                }
                employeeEducation.Deleted = true;
                db.MarkAsModified(employeeEducation);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeEducationExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeEducation;
        }

        public EmployeeEducation Edit(EmployeeEducation employeeEducation)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeEducation);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeEducationExists(employeeEducation.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(employeeEducation.Id);
        }

        public EmployeeEducation Get(int id)
        {
            EmployeeEducation employeeEducation = new EmployeeEducation();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeEducation = db.EmployeeEducations
                .Where(d => d.Id == id)
                .Include(d => d.EducationStudyForm)
                .FirstOrDefault();
            }
            return employeeEducation;

        }

        public List<EmployeeEducation> GetAll()
        {
            List<EmployeeEducation> educations = new List<EmployeeEducation>();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                educations = db.EmployeeEducations
                    .Where(e => e.Deleted != true)
                    .Include(d => d.EducationStudyForm)
                    .ToList();
            }
            return educations;
        }

        public List<EmployeeEducation> GetByEmployeeId(int employeeId)
        {
            List<EmployeeEducation> educations = new List<EmployeeEducation>();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                educations = db.EmployeeEducations
                    .Where(e => e.Deleted != true && e.EmployeeId == employeeId)
                    .Include(d => d.EducationStudyForm)
                    .ToList();
            }

            return educations;
        }

        private bool EmployeeEducationExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeEducations.Count(e => e.Id == id) > 0;
            }
        }
    }
}
