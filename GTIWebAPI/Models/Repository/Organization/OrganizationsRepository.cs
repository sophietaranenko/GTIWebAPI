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
    public class OrganizationsRepository : IOrganizationsRepository 
    {
        private IDbContextFactory factory;
        public OrganizationsRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public List<OrganizationSearchDTO> Search(int countryId, string registrationNumber)
        {
            List<OrganizationSearchDTO> organizationList = new List<OrganizationSearchDTO>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationList = db.SearchOrganization(countryId, registrationNumber).ToList();
                foreach (var item in organizationList)
                {
                    item.OrganizationGTILinks =
                        db.GetOrganizationGTIByOrganization(item.Id);
                }
            }
            if (organizationList == null)
            {
                throw new NotFoundException();
            }
            return organizationList;
        }
        public List<OrganizationView> GetAll(List<int> officeIds)
        {
            List<OrganizationView> organizationList = new List<OrganizationView>();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationList = db.GetOrganizationsByOffices(officeIds).ToList();
            }
            if (organizationList == null)
            {
                throw new NotFoundException();
            }
            return organizationList;
        }


        public Organizations.Organization GetView(int id)
        {
            Organizations.Organization organization = new Organizations.Organization();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                organization = db.Organizations
                   .Where(d => d.Id == id)
                   .FirstOrDefault();


                organization.OrganizationAddresses =
                       db.OrganizationAddresses
                       .Where(a => a.Deleted != true && a.OrganizationId == id)
                       .Include(d => d.Address)
                       .Include(d => d.Address.AddressLocality)
                       .Include(d => d.Address.AddressPlace)
                       .Include(d => d.Address.AddressRegion)
                       .Include(d => d.Address.AddressVillage)
                       .Include(d => d.Address.Country)
                       .Include(d => d.OrganizationAddressType)
                       .ToList();


                organization.OrganizationContactPersonViews =
                     db.OrganizationContactPersonViews
                     .Where(p => p.Deleted != true && p.OrganizationId == id)
                     .ToList();

                if (organization.OrganizationContactPersonViews != null)
                {
                    foreach (var person in organization.OrganizationContactPersonViews)
                    {
                        person.OrganizationContactPersonContacts = db.OrganizationContactPersonContacts
                            .Where(d => d.OrganizationContactPersonId == person.Id)
                            .Include(d => d.ContactType)
                            .ToList();
                    }
                }

                organization.OrganizationGTILinks = db.OrganizationGTILinks
                    .Where(d => d.Deleted != true && d.OrganizationId == id)
                    .ToList();

                foreach (var link in organization.OrganizationGTILinks)
                {
                    if (link.GTIId != null)
                    {
                        link.OrganizationGTI = db.GTIOrganizations
                        .Where(d => d.Id == link.GTIId)
                        .FirstOrDefault();
                        if (link.OrganizationGTI != null)
                        {
                            link.OrganizationGTI.Office =
                            db.Offices
                            .Where(d => d.Id == link.OrganizationGTI.OfficeId)
                            .FirstOrDefault();
                        }
                    }
                }

                organization.OrganizationProperties =
                    db.OrganizationProperties
                    .Where(o => o.Deleted != true && o.OrganizationId == id)
                    .Include(d => d.OrganizationPropertyType)
                    .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                    .ToList();


                organization.OrganizationTaxAddresses =
                    db.OrganizationTaxAddresses
                    .Where(o => o.Deleted != true && o.OrganizationId == id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressVillage)
                    .Include(d => d.Address.Country)
                    .ToList();

                organization.OrganizationLanguageNames =
                    db.OrganizationLanguageNames
                    .Where(o => o.Deleted != true && o.OrganizationId == id)
                    .Include(d => d.Language)
                    .ToList();
            }
            if (organization == null)
            {
                throw new NotFoundException();
            }
            return organization;
        }


        public Organizations.Organization GetEdit(int id)
        {
            Organizations.Organization organization = new Organizations.Organization();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organization = db.Organizations
                    .Where(d => d.Id == id)
                    .Include(d => d.Country)
                    .Include(d => d.OrganizationLegalForm)
                    .FirstOrDefault();
            }
            if (organization == null)
            {
                throw new NotFoundException();
            }
            return organization;
        }



        public Organizations.Organization Edit(Organizations.Organization organization)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(organization);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
                organization = db.Organizations
                   .Where(d => d.Id == organization.Id)
                   .Include(d => d.Country)
                   .Include(d => d.OrganizationLegalForm)
                   .FirstOrDefault();
            }
            return organization;
        }


        public Organizations.Organization Add(Organizations.Organization organization)
        {

            using (IAppDbContext db = factory.CreateDbContext())
            {
                organization.Id = organization.NewId(db);
                db.Organizations.Add(organization);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationExists(organization.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }
                organization = db.Organizations
                   .Where(d => d.Id == organization.Id)
                   .Include(d => d.Country)
                   .Include(d => d.OrganizationLegalForm)
                   .FirstOrDefault();
            }
            return organization;
        }

        public Organizations.Organization DeleteOrganization(int id)
        {
            Organizations.Organization organization = new Organizations.Organization();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organization = db.Organizations
              .Where(d => d.Id == id)
              .Include(d => d.Country)
              .Include(d => d.OrganizationLegalForm)
              .FirstOrDefault();
                if (organization == null)
                {
                    throw new NotFoundException();
                }
                if (organization != null)
                {
                    organization.Deleted = true;
                    db.MarkAsModified(organization);
                    db.SaveChanges();
                }
            }
            return organization;
        }




        public OrganizationList GetTypes()
        {
            OrganizationList list = new OrganizationList();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = OrganizationList.CreateOrganizationList(db);
            }
            if (list == null)
            {
                throw new NotFoundException();
            }
            return list;
        }

        private bool OrganizationExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.Organizations.Count(e => e.Id == id) > 0;
            }
        }
    }
}
