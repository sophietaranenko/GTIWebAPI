using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    public class AddressList
    {
        public IEnumerable<AddressLocalityDTO> AddressLocalities { get; set; }

        public IEnumerable<AddressRegionDTO> AddressRegions { get; set; }

        public IEnumerable<AddressPlaceDTO> AddressPlaces { get; set; }

        public IEnumerable<AddressVillageDTO> AddressVillages { get; set; }

        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}
