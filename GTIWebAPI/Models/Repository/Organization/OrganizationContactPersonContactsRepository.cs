using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Context;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GTIWebAPI.Models.Repository.Organization
{
    public class OrganizationContactPersonContactsRepository : IOrganizationContactPersonContactsRepository
    {
        private IDbContextFactory factory;
        public OrganizationContactPersonContactsRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationContactPersonContactsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationContactPersonContact Add(OrganizationContactPersonContact organizationContactPersonContact)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {

                organizationContactPersonContact.Id = organizationContactPersonContact.NewId(db);
                db.OrganizationContactPersonContacts.Add(organizationContactPersonContact);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationContactPersonContactExists(organizationContactPersonContact.Id))
                    {
                        throw new ArgumentException("Conflict");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationContactPersonContact = organizationContactPersonContact = db.OrganizationContactPersonContacts
                    .Where(p => p.Deleted != true && p.Id == organizationContactPersonContact.Id)
                    .Include(d => d.ContactType)
                    .FirstOrDefault();
            }
            return organizationContactPersonContact;
        }

        public OrganizationContactPersonContact Delete(int id)
        {
            OrganizationContactPersonContact organizationContactPersonContact = new OrganizationContactPersonContact();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationContactPersonContact = db.OrganizationContactPersonContacts
                    .Where(p => p.Id == id)
                    .Include(d => d.ContactType)
                    .FirstOrDefault();

                if (organizationContactPersonContact == null)
                {
                    throw new ArgumentException("Not found");
                }
                organizationContactPersonContact.Deleted = true;
                db.MarkAsModified(organizationContactPersonContact);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationContactPersonContactExists(id))
                    {
                        throw new ArgumentException("Not found");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return organizationContactPersonContact;
        }

        public OrganizationContactPersonContact Edit(OrganizationContactPersonContact organizationContactPersonContact)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(organizationContactPersonContact);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationContactPersonContactExists(organizationContactPersonContact.Id))
                    {
                        throw new ArgumentException("Not found");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationContactPersonContact = db.OrganizationContactPersonContacts
                    .Where(p => p.Deleted != true && p.Id == organizationContactPersonContact.Id)
                    .Include(d => d.ContactType)
                    .FirstOrDefault();
            }
            return organizationContactPersonContact;
        }

        public OrganizationContactPersonContact Get(int id)
        {
            OrganizationContactPersonContact contact = new OrganizationContactPersonContact();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                contact = db.OrganizationContactPersonContacts
                    .Where(p => p.Id == id)
                    .Include(d => d.ContactType)
                    .FirstOrDefault();
            }
            return contact;
        }

        public List<OrganizationContactPersonContact> GetByOrganizationContactPersonId(int organizationContactPersonId)
        {
            List<OrganizationContactPersonContact> list = new List<OrganizationContactPersonContact>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.OrganizationContactPersonContacts
                    .Where(p => p.Deleted != true && p.OrganizationContactPersonId == organizationContactPersonId)
                    .Include(d => d.ContactType)
                    .ToList();
            }
            return list;
        }

        private bool OrganizationContactPersonContactExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationContactPersonContacts.Count(e => e.Id == id) > 0;
            }
        }
    }
}
