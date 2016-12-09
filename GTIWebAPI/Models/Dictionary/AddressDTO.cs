using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Dictionary
{
    public class AddressDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostIndex { get; set; }

        /// <summary>
        /// Область, республика в целочисленном
        /// </summary>
        public byte? RegionType { get; set; }

        /// <summary>
        /// Область, республика в строковом формате
        /// </summary>
        public string RegionTypeString { get; set; }

        /// <summary>
        /// Название области
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Тип поселения (город, село...) в целочисленном
        /// </summary>
        public byte? LocalityType { get; set; }

        /// <summary>
        /// Тип поселения (город, село...) в строковом
        /// </summary>
        public string LocalityTypeString { get; set; }

        /// <summary>
        /// Название поселения
        /// </summary>
        public string LocalityName { get; set; }

        /// <summary>
        /// Тип части поселения (поселок, микрорайон) в целочисленном
        /// </summary>
        public byte? VillageType { get; set; }

        /// <summary>
        /// Тип части поселения (поселок, микрорайон) в строковом
        /// </summary>
        public string VillageTypeString { get; set; }

        /// <summary>
        /// Название части поселения
        /// </summary>
        public string VillageName { get; set; }

        /// <summary>
        /// Тип дороги (улица, переулок, площадь) в целочисленном 
        /// </summary>
        public byte? PlaceType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlaceTypeString { get; set; }

        public string PlaceName { get; set; }

        public short? BuildingNumber { get; set; }

        public string Housing { get; set; }

        public string Apartment { get; set; }

    }
}