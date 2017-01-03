namespace GTIWebAPI.Models.Clients
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client : GTITable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            ClientBank = new HashSet<ClientBank>();
            ClientAgreement = new HashSet<ClientAgreement>();
            ClientContact = new HashSet<ClientContact>();
            ClientGTIClient = new HashSet<ClientGTIClient>();
            ClientSigner = new HashSet<ClientSigner>();
            ClientTaxInfo = new HashSet<ClientTaxInfoDTO>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [StringLength(500)]
        public string EnglishName { get; set; }

        [StringLength(500)]
        public string NativeName { get; set; }

        [StringLength(500)]
        public string RussianName { get; set; }

        public int? OrganizationTypeId { get; set; }

        [StringLength(30)]
        public string IdentityCode { get; set; }

        public int? AddressId { get; set; }

        public virtual Address Address { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }

        [StringLength(50)]
        public string Website { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public virtual OrganizationTypeDTO OrganizationType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientBank> ClientBank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAgreement> ClientAgreement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientContact> ClientContact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientGTIClient> ClientGTIClient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientSigner> ClientSigner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientTaxInfoDTO> ClientTaxInfo { get; set; }

        protected override string TableName
        {
            get
            {
                return "Client";
            }
        }

        public ClientEditDTO MapToEdit()
        {
            ClientEditDTO dto = new ClientEditDTO
            {
                Address =
                new AddressDTO
                {
                    Apartment = Address.Apartment,
                    BuildingNumber = Address.BuildingNumber,
                    Country = Address.Country,
                    Housing = Address.Housing,
                    Id = Address.Id,
                    LocalityName = Address.LocalityName,
                    LocalityType = Address.LocalityType,
                    LocalityTypeString = Address.LocalityTypeString,
                    PlaceName = Address.PlaceName,
                    PlaceType = Address.PlaceType,
                    PlaceTypeString = Address.PlaceTypeString,
                    PostIndex = Address.PostIndex,
                    RegionName = Address.RegionName,
                    RegionType = Address.RegionType,
                    RegionTypeString = Address.RegionTypeString,
                    VillageName = Address.VillageName,
                    VillageType = Address.VillageType,
                    VillageTypeString = Address.VillageTypeString
                },
                AddressId = AddressId,
                Email = Email,
                EmployeeId = EmployeeId,
                EnglishName = EnglishName,
                FaxNumber = FaxNumber,
                Id = Id,
                IdentityCode = IdentityCode,
                NativeName = NativeName,
                OrganizationType = OrganizationType == null ? null : new OrganizationTypeDTO
                {
                    EnglishExplanation = OrganizationType.EnglishExplanation,
                    RussianExplanation = OrganizationType.RussianExplanation,
                    RussianName = OrganizationType.RussianName,
                    EnglishName = OrganizationType.EnglishName,
                    Id = OrganizationType.Id,
                    Name = OrganizationType.Name,
                    UkrainianExplanation = OrganizationType.UkrainianExplanation,
                    UkrainianName = OrganizationType.UkrainianName
                },
                RussianName = RussianName,
                OrganizationTypeId = OrganizationTypeId,
                PhoneNumber = PhoneNumber,
                Website = Website
            };
            return dto;
        }
    }
}
