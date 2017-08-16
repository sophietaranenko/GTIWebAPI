namespace GTIWebAPI.Models.Dictionary
{
    using Employees;
    using Organizations;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// To get new Id from procedure (NewTableId) class must inherit abstract GTITable 
    /// </summary>
    [Table("Address")]
    public partial class Address : GTITable
    {
        public Address()
        {
            OrganizationAddresses = new HashSet<OrganizationAddress>();
            EmployeePassports = new HashSet<EmployeePassport>();
            Employees = new HashSet<Employee>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }


        public int? CountryId { get; set; }

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

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeePassport> EmployeePassports { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        public AddressDTO ToDTO()
        {
            AddressDTO dto = new AddressDTO
            {
                Id = this.Id,
                Apartment = this.Apartment,
                BuildingNumber = this.BuildingNumber,
                CountryId = this.CountryId,
                Country = this.Country == null ? null : this.Country.ToDTO(),
                Housing = this.Housing,
                LocalityName = this.LocalityName,
                Locality = this.AddressLocality == null? null : this.AddressLocality.ToDTO(),
                LocalityId = this.LocalityId,
                PlaceName = this.PlaceName,
                Place = this.AddressPlace == null ? null : this.AddressPlace.ToDTO(),
                PlaceId = this.PlaceId,
                PostIndex = this.PostIndex,
                RegionName = this.RegionName,
                Region = this.AddressRegion == null ? null : this.AddressRegion.ToDTO(),
                RegionId = this.RegionId,
                VillageId = this.VillageId,
                VillageName = this.VillageName,
                Village = this.AddressVillage == null ? null : this.AddressVillage.ToDTO()
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


    public class AddressDTO
    {
        public int Id { get; set; }

        public CountryDTO Country { get; set; }

        public int? CountryId { get; set; }

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

        public Address FromDTO()
        {
            return new Address()
            {
                Apartment = this.Apartment,
                BuildingNumber = this.BuildingNumber,
                CountryId = this.CountryId,
                Housing = this.Housing,
                Id = this.Id,
                LocalityId = this.LocalityId,
                LocalityName = this.LocalityName,
                PlaceId = this.PlaceId,
                PlaceName = this.PlaceName,
                PostIndex = this.PostIndex,
                RegionId = this.RegionId,
                RegionName = this.RegionName,
                VillageId = this.VillageId,
                VillageName = this.VillageName
            };
        }


    }
}
