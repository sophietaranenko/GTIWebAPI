namespace GTIWebAPI.Models.Dictionary
{
    using Employees;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// To get new Id from procedure (table_id) class must inherit abstract GTITable 
    /// </summary>
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
        public byte? RegionId { get; set; }

        /// <summary>
        /// Город, село, ПГТ
        /// </summary>
        public byte? LocalityId { get; set; }

        /// <summary>
        /// Микрорайон, поселок
        /// </summary>
        public byte? VillageId { get; set; }

        
        /// <summary>
        /// Улица, переулок, площадь...
        /// </summary>
        public byte? PlaceId { get; set; }

        public virtual AddressRegion AddressRegion { get; set; }

        public virtual AddressPlace AddressPlace { get; set; }

        public virtual AddressLocality AddressLocality { get; set; }

        public virtual AddressVillage AddressVillage { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employee { get; set; }

        public AddressDTO ToDTO()
        {
            AddressDTO dto = new AddressDTO
            {
                Id = this.Id,
                Apartment = this.Apartment,
                BuildingNumber = this.BuildingNumber,
                Country = this.Country,
                Housing = this.Housing,
                LocalityName = this.LocalityName,
                Locality = this.AddressLocality == null? null : new AddressLocalityDTO
                    {
                        Id = this.AddressLocality.Id,
                        Name = this.AddressLocality.Name
                    },
                LocalityId = this.LocalityId,
                PlaceName = this.PlaceName,
                Place = this.AddressPlace == null ? null : new AddressPlaceDTO
                {
                    Id = this.AddressPlace.Id,
                    Name = this.AddressPlace.Name
                },
                PlaceId = this.PlaceId,
                PostIndex = this.PostIndex,
                RegionName = this.RegionName,
                Region = this.AddressRegion == null ? null : new AddressRegionDTO
                {
                    Id = this.AddressRegion.Id,
                    Name = this.AddressRegion.Name
                },
                RegionId = this.RegionId,
                VillageName = this.VillageName,
                Village = this.AddressVillage == null ? null : new AddressVillageDTO
                {
                    Id = this.AddressVillage.Id,
                    Name = this.AddressVillage.Name
                }
            };
            return dto;
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
