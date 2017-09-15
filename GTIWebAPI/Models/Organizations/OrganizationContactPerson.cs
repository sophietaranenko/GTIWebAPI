namespace GTIWebAPI.Models.Organizations
{
    using Quiz;
    using Sales;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("OrganizationContactPerson")]
    public partial class OrganizationContactPerson : GTITable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationContactPerson()
        {
            OrganizationContactPersonContact = new HashSet<OrganizationContactPersonContact>();
            InteractionActOrganizationMembers = new HashSet<InteractionActOrganizationMember>();
            Passings = new HashSet<QuizPassingOrganizationContactPersonLink>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        [Required]
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

        public string AspNetUserId { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual ICollection<QuizPassingOrganizationContactPersonLink> Passings { get; set; };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationContactPersonContact> OrganizationContactPersonContact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionActOrganizationMember> InteractionActOrganizationMembers { get; set; }


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
                OrganizationContactPersonContact = this.OrganizationContactPersonContact == null ? null : this.OrganizationContactPersonContact.Select(c => c.ToDTO()).ToList(),
               // AspNetUserId = this.AspNetUserId
            };
            return dto;
        }

        protected override string TableName
        {
            get
            {
                return "OrganizationContactPerson";
            }
        }

        public OrganizationContactPersonShortForm ToShortForm()
        {
            return new OrganizationContactPersonShortForm()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                SecondName = this.SecondName,
                Surname = this.LastName,
                AspNetUserId = this.AspNetUserId 
            };
        }
    }


    public class OrganizationContactPersonDTO
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? OrganizationId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Position { get; set; }

        public string UserName { get; set; }

        public bool IsRegistered { get; set; }

      //  public string AspNetUserId { get; set; }

        public virtual IEnumerable<OrganizationContactPersonContactDTO> OrganizationContactPersonContact { get; set; }

        public OrganizationContactPerson FromDTO()
        {
            //мы не должны обновлять AspNetUserId 
            return new OrganizationContactPerson()
            {
                DateOfBirth = this.DateOfBirth,
                Email = this.Email,
                FirstName = this.FirstName,
                Id = this.Id,
                LastName = this.LastName,
                OrganizationId = this.OrganizationId,
                Position = this.Position,
                SecondName = this.SecondName,
              //  AspNetUserId = this.AspNetUserId 
            };
        }
    }

    public class OrganizationContactPersonShortForm
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Surname { get; set; }

        public string AspNetUserId { get; set; }

    }
}
