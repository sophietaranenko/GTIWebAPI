namespace GTIWebAPI.Models.Clients
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            ClientBank = new HashSet<ClientBank>();
            ClientAgreement = new HashSet<ClientAgreement>();
            ClientContact = new HashSet<ClientContact>();
            ClientGTIClient = new HashSet<ClientGTIClient>();
            ClientSigner = new HashSet<ClientSigner>();
            ClientTaxInfo = new HashSet<ClientTaxInfo>();
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

        public bool? Deleted { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }

        [StringLength(50)]
        public string Website { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public virtual OrganizationType OrganizationType { get; set; }

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
        public virtual ICollection<ClientTaxInfo> ClientTaxInfo { get; set; }
    }
}
