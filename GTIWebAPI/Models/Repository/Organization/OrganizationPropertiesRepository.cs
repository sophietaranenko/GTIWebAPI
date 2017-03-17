using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Organization
{
    public class OrganizationPropertiesRepository : IOrganizationRepository<OrganizationProperty>
    {
        private IDbContextFactory factory;
        public OrganizationPropertiesRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationPropertiesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationProperty Add(OrganizationProperty organizationProperty)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                int? propertyCountryId = db.OrganizationPropertyTypes
                    .Where(d => d.Id == organizationProperty.OrganizationPropertyTypeId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();

                int? organizationCountryId = db.Organizations
                    .Where(d => d.Id == organizationProperty.OrganizationId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();

                if (propertyCountryId != organizationCountryId)
                {
                    throw new ArgumentException("Country that property belogs to, doesn't match the Organization registration country");
                }

                organizationProperty.Id = organizationProperty.NewId(db);
                db.OrganizationProperties.Add(organizationProperty);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationPropertyExists(organizationProperty.Id))
                    {
                        throw new ArgumentException("Conflict");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationProperty =
                    db.OrganizationProperties
                    .Where(d => d.Id == organizationProperty.Id)
                    .Include(d => d.OrganizationPropertyType)
                    .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                    .FirstOrDefault();
            }
            return organizationProperty;
        }

        public OrganizationProperty Delete(int id)
        {
            OrganizationProperty organizationProperty = new OrganizationProperty();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationProperty =
                    db.OrganizationProperties
                    .Where(d => d.Id == id)
                    .Include(d => d.OrganizationPropertyType)
                    .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                    .FirstOrDefault();

                if (organizationProperty == null)
                {
                    throw new ArgumentException("Not found");
                }

                organizationProperty.Deleted = true;
                db.MarkAsModified(organizationProperty);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationPropertyExists(id))
                    {
                        throw new ArgumentException("Not found");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return organizationProperty;
        }

        public OrganizationProperty Edit(OrganizationProperty organizationProperty)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {

                int? propertyCountryId = db.OrganizationPropertyTypes
                    .Where(d => d.Id == organizationProperty.OrganizationPropertyTypeId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();

                int? organizationCountryId = db.Organizations
                    .Where(d => d.Id == organizationProperty.OrganizationId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();

                if (propertyCountryId != organizationCountryId)
                {
                    throw new ArgumentException("Country that property belogs to, doesn't match the Organization registration country");
                }


                db.MarkAsModified(organizationProperty);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationPropertyExists(organizationProperty.Id))
                    {
                        throw new ArgumentException("Not found");
                    }
                    else
                    {
                        throw;
                    }
                }

                organizationProperty =
                   db.OrganizationProperties
                   .Where(d => d.Id == organizationProperty.Id)
                   .Include(d => d.OrganizationPropertyType)
                   .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                   .FirstOrDefault();
            }
            return organizationProperty;
        }

        public OrganizationProperty Get(int id)
        {
            OrganizationProperty organizationProperty = new OrganizationProperty();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationProperty =
                    db.OrganizationProperties
                    .Where(d => d.Id == id)
                    .Include(d => d.OrganizationPropertyType)
                    .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                    .FirstOrDefault();
            }
            return organizationProperty;
        }

        public List<OrganizationProperty> GetByOrganizationId(int organizationId)
        {
            List<OrganizationProperty> organizationProperties = new List<OrganizationProperty>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationProperties =
                    db.OrganizationProperties
                    .Where(d => d.Deleted != true && d.OrganizationId == organizationId)
                    .Include(d => d.OrganizationPropertyType)
                    .Include(d => d.OrganizationPropertyType.OrganizationPropertyTypeAlias)
                    .ToList();
            }
            return organizationProperties;
        }

        private bool OrganizationPropertyExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationProperties.Count(e => e.Id == id) > 0;
            }
        }
    }
}
