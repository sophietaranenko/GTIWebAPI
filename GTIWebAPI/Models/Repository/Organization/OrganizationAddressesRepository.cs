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
    public class OrganizationAddressesRepository : IOrganizationRepository<OrganizationAddress>
    {
        private IDbContextFactory factory;
        public OrganizationAddressesRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationAddressesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public OrganizationAddress Add(OrganizationAddress organizationAddress)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationAddress.Id = organizationAddress.NewId(db);
                organizationAddress.Address.Id = organizationAddress.Address.NewId(db);
                organizationAddress.AddressId = organizationAddress.Address.Id;
                db.Addresses.Add(organizationAddress.Address);
                db.OrganizationAddresses.Add(organizationAddress);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (OrganizationAddressExists(organizationAddress.Id))
                    {
                        throw new ArgumentException("Conflict");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationAddress = db.OrganizationAddresses
                    .Where(p => p.Id == organizationAddress.Id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.OrganizationAddressType)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();

            }
            return organizationAddress;
        }

        public OrganizationAddress Delete(int id)
        {
            OrganizationAddress organizationAddress = new OrganizationAddress();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationAddress = db.OrganizationAddresses
                    .Where(p => p.Id == organizationAddress.Id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.OrganizationAddressType)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
                if (organizationAddress == null)
                {
                    throw new ArgumentException("Not found");
                }
                organizationAddress.Deleted = true;
                db.MarkAsModified(organizationAddress);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationAddressExists(id))
                    {
                        throw new ArgumentException("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return organizationAddress;
        }

        public OrganizationAddress Edit(OrganizationAddress organizationAddress)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(organizationAddress.Address);
                db.MarkAsModified(organizationAddress);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationAddressExists(organizationAddress.Id))
                    {
                        throw new ArgumentException("Not found");
                    }
                    else
                    {
                        throw;
                    }
                }
                organizationAddress = db.OrganizationAddresses
                    .Where(p => p.Id == organizationAddress.Id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.OrganizationAddressType)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
            }
            return organizationAddress;
        }

        public OrganizationAddress Get(int id)
        {
            OrganizationAddress organizationAddress = new OrganizationAddress();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationAddress = db.OrganizationAddresses
                    .Where(p => p.Id == id)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.OrganizationAddressType)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .FirstOrDefault();
            }
            return organizationAddress;
        }

        public List<OrganizationAddress> GetByOrganizationId(int organizationId)
        {
            List<OrganizationAddress> list = new List<OrganizationAddress>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.OrganizationAddresses
                    .Where(p => p.Deleted != true && p.OrganizationId == organizationId)
                    .Include(d => d.Address)
                    .Include(d => d.Address.Country)
                    .Include(d => d.OrganizationAddressType)
                    .Include(d => d.Address.AddressLocality)
                    .Include(d => d.Address.AddressPlace)
                    .Include(d => d.Address.AddressRegion)
                    .Include(d => d.Address.AddressVillage)
                    .ToList();
            }
            return list;
        }

        private bool OrganizationAddressExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.OrganizationAddresses.Count(e => e.Id == id) > 0;
            }
        }
    }
}
