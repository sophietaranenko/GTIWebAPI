using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IDbContextAddress
    {
        DbSet<Address> Addresses { get; set; }

        DbSet<AddressLocality> Localities { get; set; }

        DbSet<AddressPlace> Places { get; set; }

        DbSet<AddressRegion> Regions { get; set; }

        DbSet<AddressVillage> Villages { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<Continent> Continents { get; set; }
    }
}
