namespace GTIWebAPI.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("OrganizationContactPerson")]
    public partial class OrganizationContactPerson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationContactPerson()
        {
            OrganizationContactPersonContact = new HashSet<OrganizationContactPersonContact>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        public int? OrganizationId { get; set; }

        public bool? Deleted { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(250)]
        public string Position { get; set; }

        public virtual Organization Organization { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationContactPersonContact> OrganizationContactPersonContact { get; set; }

        public OrganizationContactPersonDTO ToDTO()
        {
            OrganizationContactPersonDTO dto = new OrganizationContactPersonDTO()
            {
                DateOfBirth = this.DateOfBirth,
                FirstName = this.FirstName,
                Email = this.Email,
                Id = this.Id,
                LastName = this.LastName,
                OrganizationId = this.OrganizationId,
                Position = this.Position,
                SecondName = this.SecondName,
                OrganizationContactPersonContact = this.OrganizationContactPersonContact == null ? null : this.OrganizationContactPersonContact.Select(c => c.ToDTO()).ToList()
            };
            return dto;
        }
    }
}
