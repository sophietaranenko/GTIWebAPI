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
    public class OrganizationTaxAddressesRepository : IOrganizationRepository<OrganizationTaxAddress>
    {
        private IDbContextFactory factory;
        public OrganizationTaxAddressesRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationTaxAddressesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationTaxAddress Add(OrganizationTaxAddress organizationTaxAddress)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationTaxAddress.Id = organizationTaxAddress.NewId(db);
                organizationTaxAddress.Address.Id = organizationTaxAddress.Address.NewId(db);
                organizationTaxAddress.AddressId = organizationTaxAddress.Address.Id;
                db.Addresses.Add(organizationTaxAddress.Address);
                db.OrganizationTaxAddresses.Add(organizationTaxAddress);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationTaxAddressExists(organizationTaxAddress.Id))
                    {
                        throw new ArgumentException("Conflict");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationTaxAddress = db.OrganizationTaxAddresses
                    .Where(p => p.Id == organizationTaxAddress.Id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();

            }
            return organizationTaxAddress;
        }

        public OrganizationTaxAddress Delete(int id)
        {
            OrganizationTaxAddress organizationTaxAddress = new OrganizationTaxAddress();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationTaxAddress = db.OrganizationTaxAddresses
                    .Where(p => p.Id == id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
                if (organizationTaxAddress == null)
                {
                    throw new ArgumentException("Not found");
                }
                organizationTaxAddress.Deleted = true;
                db.MarkAsModified(organizationTaxAddress);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationTaxAddressExists(id))
                    {
                        throw new ArgumentException("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return organizationTaxAddress;
        }

        public OrganizationTaxAddress Edit(OrganizationTaxAddress organizationTaxAddress)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(organizationTaxAddress.Address);
                db.MarkAsModified(organizationTaxAddress);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationTaxAddressExists(organizationTaxAddress.Id))
                    {
                        throw new ArgumentException("Not found");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationTaxAddress = db.OrganizationTaxAddresses
                    .Where(p => p.Id == organizationTaxAddress.Id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
            }
            return organizationTaxAddress;
        }

        public OrganizationTaxAddress Get(int id)
        {
            OrganizationTaxAddress organizationTaxAddress = new OrganizationTaxAddress();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationTaxAddress = db.OrganizationTaxAddresses
                    .Where(p => p.Id == id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
            }
            return organizationTaxAddress;
        }

        public List<OrganizationTaxAddress> GetByOrganizationId(int organizationId)
        {
            List<OrganizationTaxAddress> list = new List<OrganizationTaxAddress>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.OrganizationTaxAddresses
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .ToList();
            }
            return list;
        }

        private bool OrganizationTaxAddressExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationTaxAddresses.Count(e => e.Id == id) > 0;
            }
        }
    }
}
