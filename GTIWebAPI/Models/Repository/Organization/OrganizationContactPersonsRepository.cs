using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository.Organization
{
    public class OrganizationContactPersonsRepository : IOrganizationContactPersonsRepository
    {
        private IDbContextFactory factory;
        public OrganizationContactPersonsRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationContactPersonsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationContactPersonView Add(OrganizationContactPerson organizationContactPerson)
        {
            OrganizationContactPersonView organizationContactPersonView = new OrganizationContactPersonView();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationContactPerson.Id = organizationContactPerson.NewId(db);
                db.OrganizationContactPersons.Add(organizationContactPerson);

                if (organizationContactPerson.OrganizationContactPersonContact != null)
                {
                    if (organizationContactPerson.OrganizationContactPersonContact.Count > 0)
                    {
                        foreach (var contact in organizationContactPerson.OrganizationContactPersonContact)
                        {
                            contact.Id = contact.NewId(db);
                            contact.OrganizationContactPersonId = organizationContactPerson.Id;
                            db.OrganizationContactPersonContacts.Add(contact);
                        }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationContactPersonExists(organizationContactPerson.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }

                organizationContactPersonView =
                    db.OrganizationContactPersonViews
                    .Where(d => d.Id == organizationContactPerson.Id)
                    .FirstOrDefault();
                if (organizationContactPersonView != null)
                {
                    organizationContactPersonView.OrganizationContactPersonContacts =
                        db.OrganizationContactPersonContacts
                        .Where(d => d.Deleted != true && d.OrganizationContactPersonId == organizationContactPerson.Id)
                        .Include(d => d.ContactType)
                        .ToList();

                }


            }
            return organizationContactPersonView;
        }

        public OrganizationContactPersonView Delete(int id)
        {
            OrganizationContactPersonView organizationContactPerson = new OrganizationContactPersonView();
            using (IAppDbContext db = factory.CreateDbContext())
            {


                OrganizationContactPerson toDelete = db.OrganizationContactPersons
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
                if (toDelete == null)
                {
                    throw new NotFoundException();
                }
                toDelete.Deleted = true;
                db.MarkAsModified(toDelete);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationContactPersonExists(id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }


                organizationContactPerson =
                    db.OrganizationContactPersonViews
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
                organizationContactPerson.OrganizationContactPersonContacts = db.OrganizationContactPersonContacts
                   .Where(c => c.OrganizationContactPersonId == organizationContactPerson.Id && c.Deleted != true)
                   .Include(d => d.ContactType)
                   .ToList();              
            }
            return organizationContactPerson;
        }

        public OrganizationContactPersonView Edit(OrganizationContactPerson organizationContactPerson)
        {
            OrganizationContactPersonView view = new OrganizationContactPersonView();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(organizationContactPerson);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationContactPersonExists(organizationContactPerson.Id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
                view = db.OrganizationContactPersonViews
                    .Where(d => d.Id == organizationContactPerson.Id)
                    .FirstOrDefault();

                view.OrganizationContactPersonContacts = db.OrganizationContactPersonContacts
                   .Where(c => c.OrganizationContactPersonId == organizationContactPerson.Id && c.Deleted != true)
                   .Include(d => d.ContactType)
                   .ToList();
            }
            return view;
        }

        public OrganizationContactPersonView Get(int id)
        {
            OrganizationContactPersonView organizationContactPersonView = new OrganizationContactPersonView();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationContactPersonView =
                        db.OrganizationContactPersonViews
                        .Where(d => d.Id == id)
                        .FirstOrDefault();
                if (organizationContactPersonView != null)
                {
                    organizationContactPersonView.OrganizationContactPersonContacts =
                        db.OrganizationContactPersonContacts
                        .Where(c => c.OrganizationContactPersonId == organizationContactPersonView.Id && c.Deleted != true)
                        .Include(d => d.ContactType)
                        .ToList();
                }
            }

            return organizationContactPersonView;
        }

        public List<OrganizationContactPersonView> GetByOrganizationId(int organizationId)
        {
            List<OrganizationContactPersonView> persons = new List<OrganizationContactPersonView>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                persons =
                    db.OrganizationContactPersonViews
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId).ToList();

                if (persons != null)
                {
                    foreach (var person in persons)
                    {
                        person.OrganizationContactPersonContacts =
                            db.OrganizationContactPersonContacts
                            .Where(d => d.Deleted != true && d.OrganizationContactPersonId == person.Id)
                            .Include(d => d.ContactType)
                            .ToList();
                    }
                }

            }
            return persons;
        }

        private bool OrganizationContactPersonExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationContactPersons.Count(e => e.Id == id) > 0;
            }
        }
    }
}
