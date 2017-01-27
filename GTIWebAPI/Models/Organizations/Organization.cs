namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// All the organizations 
    /// </summary>
    [Table("Organization")]
    public partial class Organization : GTITable
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organization()
        {
            OrganizationAddresses = new HashSet<OrganizationAddress>();
            OrganizationContactPersons = new HashSet<OrganizationContactPerson>();
            OrganizationGTILinks = new HashSet<OrganizationGTILink>();
            OrganizationProperties = new HashSet<OrganizationProperty>();
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

        [StringLength(50)]
        public string ShortName { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }

        [StringLength(50)]
        public string Website { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Skype { get; set; }

        public bool? Deleted { get; set; }

        public int? OrganizationLegalFormId { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual OrganizationLegalForm OrganizationLegalForms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationContactPerson> OrganizationContactPersons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationGTILink> OrganizationGTILinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationProperty> OrganizationProperties { get; set; }


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
                Email = Email,
                EmployeeId = EmployeeId,
                EnglishName = EnglishName,
                FaxNumber = FaxNumber,
                Id = Id,
                NativeName = NativeName,
                RussianName = RussianName,
                PhoneNumber = PhoneNumber,
                Website = Website
            };
            return dto;
        }

        public OrganizationDTO MapToDTO()
        {
            OrganizationDTO dto = new OrganizationDTO
            {
                ShortName = this.ShortName,
                Skype = this.Skype,
                Email = this.Email,
                EmployeeId = this.EmployeeId,
                EnglishName = this.EnglishName,
                FaxNumber = this.FaxNumber,
                OrganizationLegalFormId = this.OrganizationLegalFormId,
                Id = this.Id,
                NativeName = this.NativeName,
                PhoneNumber = this.PhoneNumber,
                RussianName = this.RussianName,
                Website = this.Website
            };
            return dto;
        }

    }
}
