namespace GTIWebAPI.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("OrganizationContactPersonView")]
    public partial class OrganizationContactPersonView
    {
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

        [StringLength(256)]
        public string UserName { get; set; }

        public bool IsRegistered { get

            { return UserName == null ? false : true; }
        }

        [NotMapped]
        public ICollection<OrganizationContactPersonContact> OrganizationContactPersonContacts { get; set; }

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
                IsRegistered = this.IsRegistered,
                UserName = this.UserName,
                OrganizationContactPersonContact = this.OrganizationContactPersonContacts == null ? null : this.OrganizationContactPersonContacts.Select(c => c.ToDTO()).ToList()
            };
            return dto;
        }
    }
}
