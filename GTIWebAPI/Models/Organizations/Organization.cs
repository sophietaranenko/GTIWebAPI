namespace GTIWebAPI.Models.Clients
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// All the organizations company contacts to
    /// </summary>
    [Table("Organization")]
    public partial class Organization : GTITable
    {
        /// <summary>
        /// Constructor fills all the lists inside class - tables connected by foreign key
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organization()
        {
            OrganizationBanks = new HashSet<OrganizationBank>();
            OrganizationAgreements = new HashSet<OrganizationAgreement>();
            OrganizationContacts = new HashSet<OrganizationContact>();
            OrganizationGTILink = new HashSet<OrganizationGTILink>();
            OrganizationSigners = new HashSet<OrganizationSigner>();
            OrganizationTaxInfos = new HashSet<OrganizationTaxInfoDTO>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Employee - record creator 
        /// </summary>
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

        /// <summary>
        /// Type of organization owing (LLC, LTD, OOO etc.)
        /// </summary>
        public virtual OrganizationTypeDTO OrganizationType { get; set; }

        /// <summary>
        /// Bank info about organization (its bank accounts)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationBank> OrganizationBanks { get; set; }

        /// <summary>
        /// Agreements with our company, registered in program
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationAgreement> OrganizationAgreements { get; set; }

        /// <summary>
        /// Paople managers contact to, they also can be Users of our program 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationContact> OrganizationContacts { get; set; }

        /// <summary>
        /// Link from WebAPI Organization to GTI VFP Organization (One WebAPI Organization can be linked to many GTI VFP Organizations)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationGTILink> OrganizationGTILink { get; set; }

        /// <summary>
        /// Signers registered with period of validity (person that signs organization's documents at currect period) 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationSigner> OrganizationSigners { get; set; }

        /// <summary>
        /// Legal address with Tax Codes 
        /// </summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationTaxInfoDTO> OrganizationTaxInfos { get; set; }

        protected override string TableName
        {
            get
            {
                return "Organization";
            }
        }

        /// <summary>
        /// Method for mapping (Automapper doesn't work:( ) 
        /// </summary>
        /// <returns></returns>
        public OrganizationEditDTO MapToEdit()
        {
            OrganizationEditDTO dto = new OrganizationEditDTO
            {
                Address = Address.ToDTO(),
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
