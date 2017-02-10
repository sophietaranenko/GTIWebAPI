using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IDbContextOrganization : IDbContextAddress
    {
        DbSet<ContactType> ContactTypes { get; set; }

        DbSet<OrganizationAddressType> OrganizationAddressTypes { get; set; }

        DbSet<OrganizationTaxAddressType> OrganizationTaxAddressTypes { get; set; }

        DbSet<OrganizationPropertyType> OrganizationPropertyTypes { get; set; }

        DbSet<OrganizationLegalForm> OrganizationLegalForms { get; set; }

        DbSet<OrganizationPropertyTypeAlias> OrganizationPropertyTypeAliases { get; set; }

        DbSet<Address> Addresses { get; set; }

        DbSet<AddressLocality> Localities { get; set; }

        DbSet<AddressPlace> Places { get; set; }

        DbSet<AddressRegion> Regions { get; set; }

        DbSet<AddressVillage> Villages { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<Continent> Continents { get; set; }

        DbSet<Language> Languages { get; set; }
    }
}
