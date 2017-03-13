using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Context;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeContactsRepository : IEmployeeContactsRepository
    {
        private IDbContextFactory factory;
        public EmployeeContactsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeContactsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public List<EmployeeContact> GetAll()
        {
            List<EmployeeContact> contacts = new List<EmployeeContact>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                contacts = 
                    db.EmployeeContacts
                    .Where(p => p.Deleted != true)
                    .Include(d => d.ContactType)
                    .ToList();
            }
            return contacts;
        }

        public List<EmployeeContact> GetByEmployeeId(int employeeId)
        {
            List<EmployeeContact> contacts = new List<EmployeeContact>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                contacts = 
                    db.EmployeeContacts
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .Include(d => d.ContactType)
                    .ToList();
            }
            return contacts;
        }

        public EmployeeContact Get(int id)
        {
            EmployeeContact contact = new EmployeeContact();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                contact = 
                    db.EmployeeContacts
                    .Where(d => d.Id == id)
                    .Include(d => d.ContactType)
                    .FirstOrDefault();
            }
            if (contact == null)
            {
                throw new ArgumentException("Given Id not found.", "id");
            }
            return contact;
        }

        public EmployeeContact Add(EmployeeContact contact)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                contact.Id = contact.NewId(db);
                db.EmployeeContacts.Add(contact);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeContactExists(contact.Id))
                    {
                        throw new DbUpdateException("Entry with the same Id already exists");
                    }
                    else
                    {
                        throw;
                    }
                }
                contact = Get(contact.Id);
            }
            return contact;
        }

        public EmployeeContact Edit(EmployeeContact contact)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(contact);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeContactExists(contact.Id))
                    {
                        throw new DbUpdateException("Entry with current Id doesn't exist");
                    }
                    else
                    {
                        throw;
                    }
                }
                contact = Get(contact.Id);
            }
            return contact;
        }

        public EmployeeContact Delete(int contactId)
        {
            EmployeeContact employeeContact = new EmployeeContact();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeContact = Get(contactId);
                if (employeeContact == null)
                {
                    throw new DbUpdateException("Entry with current id doesn't exist");
                }
                employeeContact.Deleted = true;
                db.MarkAsModified(employeeContact);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeContactExists(contactId))
                    {
                        throw new DbUpdateException("Entry with current id doesn't exist");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeContact;
        }



        public bool EmployeeContactExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeContacts.Count(e => e.Id == id) > 0;
            }
        }
    }
}
