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
    public partial class Address : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        [StringLength(20)]
        public string Country { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        [StringLength(20)]
        public string PostIndex { get; set; }

        /// <summary>
        /// Имя области или региона
        /// </summary>
        [StringLength(50)]
        public string RegionName { get; set; }

        /// <summary>
        /// Название поселения
        /// </summary>
        [StringLength(50)]
        public string LocalityName { get; set; }

        /// <summary>
        /// Название района внутри поселения
        /// </summary>
        [StringLength(50)]
        public string VillageName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string PlaceName { get; set; }


        /// <summary>
        /// Номер дома
        /// </summary>
        public short? BuildingNumber { get; set; }

        /// <summary>
        /// Номер кампуса (Адрес состоит из трех домов, поэтому будет 1А, 1Б, 1С)
        /// </summary>
        [StringLength(5)]
        public string Housing { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        [StringLength(5)]
        public string Apartment { get; set; }

        /// <summary>
        /// Регион, область, или республика
        /// </summary>
        public byte? RegionType { get; set; }

        /// <summary>
        /// Город, село, ПГТ
        /// </summary>
        public byte? LocalityType { get; set; }

        /// <summary>
        /// Микрорайон, поселок
        /// </summary>
        public byte? VillageType { get; set; }

        /// <summary>
        /// Улица, переулок, площадь...
        /// </summary>
        public byte? PlaceType { get; set; }

        /// <summary>
        /// в строковом формате вместо инт
        /// </summary>
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

        /// <summary>
        /// в строковом формате вместо инт
        /// </summary>
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

        /// <summary>
        /// в строковом формате вместо инт
        /// </summary>
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

        /// <summary>
        /// в строковом формате вместо инт
        /// </summary>
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

        /// <summary>
        /// в строковом формате вместо инт
        /// </summary>
        public SelectList GetRegionType()
        {
            var regionList = Enum.GetValues(typeof(Region)).Cast<Region>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(regionList, "Value", "Text");
        }

        protected override string TableName
        {
            get
            {
                return "Address";
            }
        }
    }
}
