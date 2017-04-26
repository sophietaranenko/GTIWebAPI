using AutoMapper;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    public class AddressList
    {
        public AddressList() 
        {
        }

        public static AddressList CreateAddressList(IDbContextAddress db)
        {
            AddressList AddressList = new AddressList();

            List<AddressLocality> localities = db.Localities.ToList();
            AddressList.AddressLocalities = localities.Select(l => l.ToDTO()).ToList();

            List<AddressPlace> places = db.Places.ToList();
            AddressList.AddressPlaces = places.Select(s => s.ToDTO()).ToList();

            List<AddressRegion> regions = db.Regions.ToList();
            AddressList.AddressRegions = regions.Select(d => d.ToDTO()).ToList();

            List<AddressVillage> villages = db.Villages.ToList();
            AddressList.AddressVillages = villages.Select(c => c.ToDTO()).ToList();

            List<Country> countries = db.Countries.ToList();
            AddressList.Countries = countries.Select(c => c.ToDTO()).ToList();

            return AddressList;
        }

        public IEnumerable<AddressLocalityDTO> AddressLocalities { get; set; }

        public IEnumerable<AddressRegionDTO> AddressRegions { get; set; }

        public IEnumerable<AddressPlaceDTO> AddressPlaces { get; set; }

        public IEnumerable<AddressVillageDTO> AddressVillages { get; set; }

        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}
