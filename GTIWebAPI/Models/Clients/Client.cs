namespace GTIWebAPI.Models.Clients
{
    using Dictionary;
    using Employees;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client : IUserable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            ClientBank = new HashSet<ClientBank>();
            ClientAgreement = new HashSet<ClientAgreement>();
            ClientContact = new HashSet<ClientContact>();
            ClientGTIClient = new HashSet<ClientGTIClient>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(500)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string ShortName { get; set; }

        public int? TypeOrganization { get; set; }

        [StringLength(30)]
        public string IdentityCode { get; set; }

        public int? AddressPhysicalId { get; set; }
        public Address AddressPhysical { get; set; }

        public int? AddressLegalId { get; set; }
        public Address AddressLegal { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(128)]
        [Column("AspNetUserId")]
        public string UserId { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [NotMapped]
        public ICollection<ClientGTI> ClientGTI { get; set; }
        [NotMapped]
        public int ClientGTIId { get; set; }
        [NotMapped]
        public string ClientList { get; set; }
        [NotMapped]
        public string Creator { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string Login { get; set; }
        [NotMapped]
        public string Email { get; set; }

        public byte[] ProfilePicture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientBank> ClientBank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAgreement> ClientAgreement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientContact> ClientContact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientGTIClient> ClientGTIClient { get; set; }

        public string TableName
        {
            get
            {
                return "Clients";
            }
        }
    }
}
