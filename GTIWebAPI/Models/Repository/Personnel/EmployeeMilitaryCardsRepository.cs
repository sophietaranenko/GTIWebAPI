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
    public class EmployeeMilitaryCardsRepository : IRepository<EmployeeMilitaryCard>
    {
        private IDbContextFactory factory;
        public EmployeeMilitaryCardsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeMilitaryCardsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeMilitaryCard Add(EmployeeMilitaryCard employeeMilitaryCard)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeMilitaryCard.Id = employeeMilitaryCard.NewId(db);
                db.EmployeeMilitaryCards.Add(employeeMilitaryCard);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeMilitaryCardExists(employeeMilitaryCard.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeMilitaryCard =
                    db.EmployeeMilitaryCards
                    .Where(d => d.Id == employeeMilitaryCard.Id)
                    .FirstOrDefault();
            }
            return employeeMilitaryCard;

        }

        public EmployeeMilitaryCard Delete(int id)
        {
            EmployeeMilitaryCard employeeMilitaryCard = new EmployeeMilitaryCard();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeMilitaryCard = 
                    db.EmployeeMilitaryCards
                    .Where(d => d.Id == id)
                    .FirstOrDefault();

                if (employeeMilitaryCard == null)
                {
                    throw new ArgumentException();
                }
                employeeMilitaryCard.Deleted = true;
                db.MarkAsModified(employeeMilitaryCard);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeMilitaryCardExists(id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeMilitaryCard;
            }

        public EmployeeMilitaryCard Edit(EmployeeMilitaryCard employeeMilitaryCard)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeMilitaryCard);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeMilitaryCardExists(employeeMilitaryCard.Id))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeMilitaryCard;
        }

        public EmployeeMilitaryCard Get(int id)
        {
            EmployeeMilitaryCard militaryCard = new EmployeeMilitaryCard();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                militaryCard = db.EmployeeMilitaryCards.Find(id);
            }
            return militaryCard;
        }

        public List<EmployeeMilitaryCard> GetAll()
        {
            List<EmployeeMilitaryCard> list = new List<EmployeeMilitaryCard>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeMilitaryCards
                    .Where(p => p.Deleted != true)
                    .ToList();
            }
            return list;
        }

        public List<EmployeeMilitaryCard> GetByEmployeeId(int employeeId)
        {
            List<EmployeeMilitaryCard> list = new List<EmployeeMilitaryCard>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeMilitaryCards
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .ToList();
            }
            return list;
        }

        private bool EmployeeMilitaryCardExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeMilitaryCards.Count(e => e.Id == id) > 0;
            }
        }
    }
}
