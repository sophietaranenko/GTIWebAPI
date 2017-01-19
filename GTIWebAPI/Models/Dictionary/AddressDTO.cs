using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Dictionary
{
    public class AddressDTO
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public string PostIndex { get; set; }

        public byte? RegionId { get; set; }

        public AddressRegionDTO Region { get; set; }

        public string RegionName { get; set; }

        public byte? LocalityId { get; set; }

        public AddressLocalityDTO Locality { get; set; }

        public string LocalityName { get; set; }

        public byte? VillageId { get; set; }

        public AddressVillageDTO Village { get; set; }

        public string VillageName { get; set; }

        public byte? PlaceId { get; set; }

        public AddressPlaceDTO Place { get; set; }

        public string PlaceName { get; set; }

        public short? BuildingNumber { get; set; }

        public string Housing { get; set; }

        public string Apartment { get; set; }

    }
}