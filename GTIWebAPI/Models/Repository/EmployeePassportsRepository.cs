using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Context;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeePassportsRepository : IEmployeePassportsRepository
    {
        private IDbContextFactory factory;
        public EmployeePassportsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeePassportsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeePassport Add(EmployeePassport passport)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passport.Address.Id = passport.Address.NewId(db);
                db.Addresses.Add(passport.Address);
                passport.Id = passport.NewId(db);
                db.EmployeePassports.Add(passport);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeePassportExists(passport.Id))
                    {
                        throw new DbUpdateException("Entry with the same Id already exists");
                    }
                    else
                    {
                        throw;
                    }
                }
                passport = Get(passport.Id);
            }
            return passport;
        }

        public EmployeePassport Delete(int id)
        {
            EmployeePassport passport = new EmployeePassport();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passport = Get(id);
                if (passport == null)
                {
                    throw new DbUpdateException("Entry with current id doesn't exist");
                }
                passport.Deleted = true;
                db.MarkAsModified(passport);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeePassportExists(id))
                    {
                        throw new DbUpdateException("Entry with current id doesn't exist");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return passport;
        }

        public EmployeePassport Edit(EmployeePassport passport)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(passport.Address);
                db.MarkAsModified(passport);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeePassportExists(passport.Id))
                    {
                        throw new DbUpdateException("Entry with current Id doesn't exist");
                    }
                    else
                    {
                        throw;
                    }
                }
                passport = Get(passport.Id);
            }
            return passport;
        }

        public EmployeePassport Get(int id)
        {
            EmployeePassport passport = new EmployeePassport();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passport = db.EmployeePassports.Where(p => p.Id == id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
            }
            if (passport == null)
            {
                throw new ArgumentException("Given Id not found.", "id");
            }
            return passport;
        }

        public List<EmployeePassport> GetAll()
        {
            List<EmployeePassport> passports = new List<EmployeePassport>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passports = db.EmployeePassports.Where(p => p.Deleted != true)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .ToList();
            }
            return passports;
        }

        public List<EmployeePassport> GetByEmployeeId(int employeeId)
        {
            List<EmployeePassport> passports = new List<EmployeePassport>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                passports = db.EmployeePassports.Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .ToList();
            }
            return passports;
        }

        private bool EmployeePassportExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeePassports.Count(e => e.Id == id) > 0;
            }
        }
    }
}
