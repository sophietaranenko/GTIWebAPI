using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeGunsRepository : IRepository<EmployeeGun>
    {
        private IDbContextFactory factory;
        public EmployeeGunsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeGunsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeGun Add(EmployeeGun item)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                item.Id = item.NewId(db);
                db.EmployeeGun.Add(item);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeGunExists(item.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(item.Id);
        }

        public EmployeeGun Delete(int id)
        {
            EmployeeGun employeeGun = new EmployeeGun();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeGun = db.EmployeeGun
                    .Where(d => d.Id == id)
                    .FirstOrDefault();

                if (employeeGun == null)
                {
                    throw new ArgumentException();
                }

                employeeGun.Deleted = true;
                db.MarkAsModified(employeeGun);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeGunExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeGun;
        }

        public EmployeeGun Edit(EmployeeGun employeeGun)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeGun);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeGunExists(employeeGun.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Get(employeeGun.Id);
        }

        public EmployeeGun Get(int id)
        {
            EmployeeGun gun = new EmployeeGun();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                gun = db.EmployeeGun
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
            }
            return gun;
        }

        public List<EmployeeGun> GetAll()
        {
            List<EmployeeGun> guns = new List<EmployeeGun>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                guns = db.EmployeeGun
                    .Where(p => p.Deleted != true)
                    .ToList();
            }
            return guns;
        }

        public List<EmployeeGun> GetByEmployeeId(int employeeId)
        {
            List<EmployeeGun> guns = new List<EmployeeGun>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                guns = db.EmployeeGun
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .ToList();
            }
            return guns;
        }

        private bool EmployeeGunExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeGun.Count(e => e.Id == id) > 0;
            }
        }
    }
}