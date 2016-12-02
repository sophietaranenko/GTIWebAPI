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

        public byte? RegionType { get; set; }

        public string RegionTypeString { get; set; }

        public string RegionName { get; set; }

        public byte? LocalityType { get; set; }

        public string LocalityTypeString { get; set; }

        public string LocalityName { get; set; }

        public byte? VillageType { get; set; }

        public string VillageTypeString { get; set; }

        public string VillageName { get; set; }

        public byte? PlaceType { get; set; }

        public string PlaceTypeString { get; set; }

        public string PlaceName { get; set; }

        public short? BuildingNumber { get; set; }

        public string Housing { get; set; }

        public string Apartment { get; set; }

    }
}