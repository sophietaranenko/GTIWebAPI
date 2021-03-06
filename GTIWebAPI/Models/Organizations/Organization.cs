namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

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
            OrganizationContactPersonViews = new HashSet<OrganizationContactPersonView>();
            OrganizationGTILinks = new HashSet<OrganizationGTILink>();
            OrganizationProperties = new HashSet<OrganizationProperty>();
            OrganizationTaxAddresses = new HashSet<OrganizationTaxAddress>();
            OrganizationLanguageNames = new HashSet<OrganizationLanguageName>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //now this information is stored in OrganizationOwner 
       // public int? EmployeeId { get; set; }

        [StringLength(500)]
        public string NativeName { get; set; }

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

        public int? ParentOrganizationId { get; set; }
        
        public virtual Organization ParentOrganization { get; set; }

        public virtual Country Country { get; set; }

        public virtual OrganizationLegalForm OrganizationLegalForm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationContactPersonView> OrganizationContactPersonViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationGTILink> OrganizationGTILinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationProperty> OrganizationProperties { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationTaxAddress> OrganizationTaxAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationLanguageName> OrganizationLanguageNames { get; set; }


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
                ShortName = this.ShortName,
                Skype = this.Skype,
                Email = this.Email,
                //now this information is stored in OrganizationOwner 
                //  EmployeeId = this.EmployeeId,
                FaxNumber = this.FaxNumber,
                OrganizationLegalFormId = this.OrganizationLegalFormId,
                Id = this.Id,
                NativeName = this.NativeName,
                PhoneNumber = this.PhoneNumber,
                Website = this.Website,
                ParentOrganizationId = this.ParentOrganizationId,
                CountryId = this.CountryId,
                Country = this.Country == null ? null : this.Country.ToDTO(),
                OrganizationLegalForm = this.OrganizationLegalForm == null ? null : this.OrganizationLegalForm.ToDTO()
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
                //now this information is stored in OrganizationOwner 
                // EmployeeId = this.EmployeeId,
                FaxNumber = this.FaxNumber,
                OrganizationLegalFormId = this.OrganizationLegalFormId,
                Id = this.Id,
                NativeName = this.NativeName,
                PhoneNumber = this.PhoneNumber,
                Website = this.Website,
                ParentOrganizationId = this.ParentOrganizationId,
                CountryId = this.CountryId,
                Country = this.Country == null? null : this.Country.ToDTO(),
                OrganizationLegalForm = this.OrganizationLegalForm == null? null : this.OrganizationLegalForm.ToDTO(),
                OrganizationAddresses = this.OrganizationAddresses == null? null : this.OrganizationAddresses.Select(d => d.ToDTO()).ToList(),
                OrganizationContactPersons = this.OrganizationContactPersonViews == null? null : this.OrganizationContactPersonViews.Select(d => d.ToDTO()),
                OrganizationGTILinks = this.OrganizationGTILinks == null? null : this.OrganizationGTILinks.Select(D => D.ToDTO()).ToList(),
                OrganizationLanguageNames = this.OrganizationLanguageNames == null ? null : this.OrganizationLanguageNames.Select(D => D.ToDTO()).ToList(),
                OrganizationTaxAddresses = this.OrganizationTaxAddresses == null ? null : this.OrganizationTaxAddresses.Select(d => d.ToDTO()).ToList()
            };
            return dto;
        }

    }

    //view
    public class OrganizationDTO
    {
        public int Id { get; set; }

        //now this information is stored in OrganizationOwner 
       // public int? EmployeeId { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public int? OrganizationLegalFormId { get; set; }

        public int? ParentOrganizationId { get; set; }

        public int? CountryId { get; set; }

        public CountryDTO Country { get; set; }

        public OrganizationLegalFormDTO OrganizationLegalForm { get; set; }

        public IEnumerable<OrganizationAddressDTO> OrganizationAddresses { get; set; }

        public IEnumerable<OrganizationContactPersonDTO> OrganizationContactPersons { get; set; }

        public IEnumerable<OrganizationGTILinkDTO> OrganizationGTILinks { get; set; }

        //public IEnumerable<OrganizationPropertyDTO> OrganizationProperties { get; set; }
        public IEnumerable<OrganizationPropertyTreeView> OrganizationProperties { get; set; }

        public IEnumerable<OrganizationTaxAddressDTO> OrganizationTaxAddresses { get; set; }

        public IEnumerable<OrganizationLanguageNameDTO> OrganizationLanguageNames { get; set; }


    }


    public class OrganizationEditDTO
    {
        public int Id { get; set; }

        ////now this information is stored in OrganizationOwner 
     //   public int? EmployeeId { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public int? OrganizationLegalFormId { get; set; }

        public int? ParentOrganizationId { get; set; }

        public int? CountryId { get; set; }

        public CountryDTO Country { get; set; }

        public OrganizationLegalFormDTO OrganizationLegalForm { get; set; }

        public Organization FromDTO()
        {
            return new Organization()
            {
                CountryId = this.CountryId,
                Email = this.Email,
                FaxNumber = this.FaxNumber,
                Id = this.Id,
                NativeName = this.NativeName,
                ParentOrganizationId = this.ParentOrganizationId,
                PhoneNumber = this.PhoneNumber,
                ShortName = this.ShortName,
                Skype = this.Skype,
                Website = this.Website
            };

        }


    }



    //classes for results of stored procedure work

    public class OrganizationSearchDTO
    {
        public int Id { get; set; }

        public string NativeName { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public IEnumerable<OrganizationGTIShortDTO> OrganizationGTILinks { get; set; }
    }

    public class OrganizationView
    {
        public int Id { get; set; }

        //owner is taken from OrganizationOwner (look into procedures) 
        public int? EmployeeId { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public bool? Deleted { get; set; }

        public int? CountryId { get; set; }

        public int? OrganizationLegalFormId { get; set; }

        public string OrganizationLegalFormName { get; set; }

        public string OrganizationLegalFormExplanation { get; set; }

        public string OrganizationRegistrationCountryName { get; set; }

        public string CreatorShortName { get; set; }

        public int DealsCount { get; set;}
    }


}
