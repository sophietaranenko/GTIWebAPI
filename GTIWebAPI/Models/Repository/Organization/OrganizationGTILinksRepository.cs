using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository.Organization
{
    public class OrganizationGTILinksRepository : IOrganizationRepository<OrganizationGTILink>
    {
        private IDbContextFactory factory;
        public OrganizationGTILinksRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationGTILinksRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationGTILink Add(OrganizationGTILink organizationGTILink)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                OrganizationGTILink existingLint = db.OrganizationGTILinks
                    .Where(d => d.Deleted != true && d.GTIId == organizationGTILink.GTIId)
                    .FirstOrDefault();
                if (existingLint != null)
                {
                    throw new ArgumentException("Link to this GTI Organization already exist");
                }
                organizationGTILink.Id = organizationGTILink.NewId(db);
                db.OrganizationGTILinks.Add(organizationGTILink);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationGTILinkExists(organizationGTILink.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (organizationGTILink.GTIId != null)
                {
                    organizationGTILink.OrganizationGTI =
                        db.GTIOrganizations.
                        Where(d => d.Id == organizationGTILink.GTIId)
                        .FirstOrDefault();
                    if (organizationGTILink.OrganizationGTI != null)
                    {
                        organizationGTILink.OrganizationGTI.Office =
                            db.Offices.
                            Where(d => d.Id == organizationGTILink.OrganizationGTI.OfficeId)
                            .FirstOrDefault();
                    }
                }
            }
            return organizationGTILink;
        }

        public OrganizationGTILink Delete(int id)
        {
            OrganizationGTILink organizationGTILink = new OrganizationGTILink();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationGTILink =
                    db.OrganizationGTILinks
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
                if (organizationGTILink.GTIId != null)
                {
                    organizationGTILink.OrganizationGTI =
                        db.GTIOrganizations.
                        Where(d => d.Id == organizationGTILink.GTIId)
                        .FirstOrDefault();
                    if (organizationGTILink.OrganizationGTI != null)
                    {
                        organizationGTILink.OrganizationGTI.Office =
                            db.Offices.
                            Where(d => d.Id == organizationGTILink.OrganizationGTI.OfficeId)
                            .FirstOrDefault();
                    }
                }
                if (organizationGTILink == null)
                {
                    throw new NotFoundException();
                }
                organizationGTILink.Deleted = true;
                db.MarkAsModified(organizationGTILink);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationGTILinkExists(id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return organizationGTILink;
        }

        public OrganizationGTILink Edit(OrganizationGTILink item)
        {
            throw new NotImplementedException();
        }

        public OrganizationGTILink Get(int id)
        {
            OrganizationGTILink organizationGTILink = new OrganizationGTILink();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationGTILink =
                    db.OrganizationGTILinks
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
                if (organizationGTILink.GTIId != null)
                {
                    organizationGTILink.OrganizationGTI =
                        db.GTIOrganizations.
                        Where(d => d.Id == organizationGTILink.GTIId)
                        .FirstOrDefault();
                    if (organizationGTILink.OrganizationGTI != null)
                    {
                        organizationGTILink.OrganizationGTI.Office =
                            db.Offices.
                            Where(d => d.Id == organizationGTILink.OrganizationGTI.OfficeId)
                            .FirstOrDefault();
                    }
                }
            }
            return organizationGTILink;
        }

        public List<OrganizationGTILink> GetByOrganizationId(int organizationId)
        {
            List<OrganizationGTILink> links = new List<OrganizationGTILink>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                links = db.OrganizationGTILinks
                .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                .ToList();

                foreach (var link in links)
                {
                    if (link.GTIId != null)
                    {
                        link.OrganizationGTI =
                            db.GTIOrganizations.
                            Where(d => d.Id == link.GTIId)
                            .FirstOrDefault();
                        if (link.OrganizationGTI != null)
                        {
                            link.OrganizationGTI.Office =
                                db.Offices.
                                Where(d => d.Id == link.OrganizationGTI.OfficeId)
                                .FirstOrDefault();
                        }
                    }
                }
            }
            return links;
        }

        private bool OrganizationGTILinkExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationGTILinks.Count(e => e.Id == id) > 0;
            }
        }
    }
}
