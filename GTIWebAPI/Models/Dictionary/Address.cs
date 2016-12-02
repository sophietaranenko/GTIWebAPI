namespace GTIWebAPI.Models.Dictionary
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    [Table("Address")]
    public partial class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(20)]
        public string PostIndex { get; set; }

        [StringLength(50)]
        public string RegionName { get; set; }

        [StringLength(50)]
        public string LocalityName { get; set; }

        [StringLength(50)]
        public string VillageName { get; set; }

        [StringLength(50)]
        public string PlaceName { get; set; }

        public short? BuildingNumber { get; set; }

        [StringLength(5)]
        public string Housing { get; set; }

        [StringLength(5)]
        public string Apartment { get; set; }

        public byte? RegionType { get; set; }

        public byte? LocalityType { get; set; }

        public byte? VillageType { get; set; }

        public byte? PlaceType { get; set; }

        public string RegionTypeString
        {
            get
            {
                if (RegionType != null)
                {
                    return Enum.GetName(typeof(Region), RegionType);
                }
                return "";
            }
        }
        public string LocalityTypeString
        {
            get
            {
                if (LocalityType != null )
                { 
                return Enum.GetName(typeof(Locality), LocalityType);
                }
                return "";
            }
        }
        public string VillageTypeString
        {
            get
            {
                if (VillageType != null)
                {
                    return Enum.GetName(typeof(Village), VillageType);
                }
                return "";

            }
        }
        public string PlaceTypeString
        {
            get
            {
                if (PlaceType != null)
                {
                    return Enum.GetName(typeof(Place), PlaceType);
                }
                return "";
            }
        }


        public SelectList GetRegionType()
        {
            var regionList = Enum.GetValues(typeof(Region)).Cast<Region>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(regionList, "Value", "Text");
        }

        public SelectList GetLocalityType()
        {
            var localityList = Enum.GetValues(typeof(Locality)).Cast<Locality>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(localityList, "Value", "Text");
        }

        public SelectList GetVillageType()
        {
            var villageList = Enum.GetValues(typeof(Village)).Cast<Village>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(villageList, "Value", "Text");
        }

        public SelectList GetPlaceType()
        {
            var placeList = Enum.GetValues(typeof(Place)).Cast<Place>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(placeList, "Value", "Text");
        }
    }
}
