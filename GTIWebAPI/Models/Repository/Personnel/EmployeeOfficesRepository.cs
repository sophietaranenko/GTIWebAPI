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
    public class EmployeeOfficesRepository : IRepository<EmployeeOffice>
    {
        private IDbContextFactory factory;
        public EmployeeOfficesRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeOfficesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeOffice Add(EmployeeOffice employeeOffice)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeOffice.Id = employeeOffice.NewId(db);
                db.EmployeeOffices.Add(employeeOffice);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeOfficeExists(employeeOffice.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeOffice = employeeOffice = db.EmployeeOffices.Where(e => e.Id == employeeOffice.Id)
                    .Include(d => d.Profession)
                    .Include(d => d.Office)
                    .Include(d => d.Department)
                    .FirstOrDefault();
            }
            return employeeOffice;
        }

        public EmployeeOffice Delete(int id)
        {
            EmployeeOffice employeeOffice = new EmployeeOffice();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeOffice = employeeOffice = db.EmployeeOffices.Where(e => e.Id == id)
                    .Include(d => d.Profession)
                    .Include(d => d.Office)
                    .Include(d => d.Department)
                    .FirstOrDefault();

                if (employeeOffice == null)
                {
                    throw new ArgumentException();
                }
                employeeOffice.Deleted = true;
                db.MarkAsModified(employeeOffice);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeOfficeExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeOffice;
        }

        public EmployeeOffice Edit(EmployeeOffice employeeOffice)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeOffice);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeOfficeExists(employeeOffice.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeOffice = db.EmployeeOffices.Where(e => e.Id == employeeOffice.Id)
                    .Include(d => d.Profession)
                    .Include(d => d.Office)
                    .Include(d => d.Department)
                    .FirstOrDefault();
            }
            return employeeOffice;
        }

        public EmployeeOffice Get(int id)
        {
            EmployeeOffice employeeOffice = new EmployeeOffice();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeOffice = db.EmployeeOffices.Where(e => e.Id == id)
                    .Include(d => d.Profession)
                    .Include(d => d.Office)
                    .Include(d => d.Department)
                    .FirstOrDefault();
            }
            return employeeOffice;
        }

        public List<EmployeeOffice> GetAll()
        {
            List<EmployeeOffice> list = new List<EmployeeOffice>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeOffices.Where(e => e.Deleted != true)
                    .Include(d => d.Profession)
                    .Include(d => d.Office)
                    .Include(d => d.Department)
                    .ToList();
            }
            return list;
        }

        public List<EmployeeOffice> GetByEmployeeId(int employeeId)
        {
            List<EmployeeOffice> list = new List<EmployeeOffice>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeOffices.Where(e => e.Deleted != true && e.EmployeeId == employeeId)
                    .Include(d => d.Profession)
                    .Include(d => d.Office)
                    .Include(d => d.Department)
                    .ToList();
            }
            return list;
        }
        private bool EmployeeOfficeExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeOffices.Count(e => e.Id == id) > 0;
            }
        }
    }
}
