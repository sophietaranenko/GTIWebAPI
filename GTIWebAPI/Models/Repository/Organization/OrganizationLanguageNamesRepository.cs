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
    public class OrganizationLanguageNamesRepository : IOrganizationRepository<OrganizationLanguageName>
    {
        private IDbContextFactory factory;
        public OrganizationLanguageNamesRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationLanguageNamesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationLanguageName Add(OrganizationLanguageName organizationLanguageName)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationLanguageName.Id = organizationLanguageName.NewId(db);
                db.OrganizationLanguageNames.Add(organizationLanguageName);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationLanguageNameExists(organizationLanguageName.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationLanguageName = db.OrganizationLanguageNames
                    .Where(p => p.Id == organizationLanguageName.Id)
                    .Include(d => d.Language)
                    .FirstOrDefault();

            }
            return organizationLanguageName;
        }

        public OrganizationLanguageName Delete(int id)
        {
            OrganizationLanguageName organizationLanguageName = new OrganizationLanguageName();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationLanguageName = db.OrganizationLanguageNames
                    .Where(p => p.Id == id)
                    .Include(d => d.Language)
                    .FirstOrDefault();
                if (organizationLanguageName == null)
                {
                    throw new NotFoundException();
                }
                organizationLanguageName.Deleted = true;
                db.MarkAsModified(organizationLanguageName);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationLanguageNameExists(id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return organizationLanguageName;
        }

        public OrganizationLanguageName Edit(OrganizationLanguageName organizationLanguageName)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(organizationLanguageName);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationLanguageNameExists(organizationLanguageName.Id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationLanguageName = db.OrganizationLanguageNames
                    .Where(p => p.Id == organizationLanguageName.Id)
                    .Include(d => d.Language)
                    .FirstOrDefault();
            }
            return organizationLanguageName;
        }

        public OrganizationLanguageName Get(int id)
        {
            OrganizationLanguageName organizationLanguageName = new OrganizationLanguageName();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationLanguageName = db.OrganizationLanguageNames
                    .Where(p => p.Id == id)
                    .Include(d => d.Language)
                    .FirstOrDefault();
            }
            return organizationLanguageName;
        }

        public List<OrganizationLanguageName> GetByOrganizationId(int organizationId)
        {
            List<OrganizationLanguageName> list = new List<OrganizationLanguageName>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.OrganizationLanguageNames
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                    .Include(d => d.Language)
                    .ToList();
            }
            return list;
        }

        private bool OrganizationLanguageNameExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationLanguageNames.Count(e => e.Id == id) > 0;
            }
        }
    }
}
