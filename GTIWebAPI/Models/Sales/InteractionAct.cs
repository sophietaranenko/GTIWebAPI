namespace GTIWebAPI.Models.Sales
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Runtime.Serialization;

    [Table("InteractionAct")]
    public partial class InteractionAct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InteractionAct()
        {
            InteractionActMembers = new HashSet<InteractionActMember>();
            InteractionActOrganizationMembers = new HashSet<InteractionActOrganizationMember>();
        }

        public int Id { get; set; }

        public int? InteractionId { get; set; }

        public int? ActId { get; set; }

        public DateTime? Happens { get; set; }
        
        public virtual Act Act { get; set; }

        [IgnoreDataMember]
        public virtual Interaction Interaction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionActMember> InteractionActMembers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionActOrganizationMember> InteractionActOrganizationMembers { get; set; }

        public InteractionActDTO ToDTO()
        {
            return new InteractionActDTO()
            {
                Id = this.Id,
                InteractionId = this.InteractionId,
                ActId = this.ActId,
                Act = this.Act == null ? null : Act.ToDTO(),
                Happens = this.Happens,
                InteractionActMembers = this.InteractionActMembers == null ? null : this.InteractionActMembers.Select(d => d.ToDTO()),
                InteractionActOrganizationMembers = this.InteractionActOrganizationMembers == null ? null : this.InteractionActOrganizationMembers.Select(d => d.ToDTO())
            };
        }
    }

    public class InteractionActDTO
    {
        public int Id { get; set; }

        public int? InteractionId { get; set; }

        public int? ActId { get; set; }

        public DateTime? Happens { get; set; }

        public ActDTO Act { get; set; }

        public IEnumerable<InteractionActMemberDTO> InteractionActMembers { get; set; }

        public IEnumerable<InteractionActOrganizationMemberDTO> InteractionActOrganizationMembers { get; set; }

        public InteractionAct FromDTO()
        {
            return new InteractionAct()
            {
                ActId = this.ActId,
                Happens = this.Happens,
                Id = this.Id,
                InteractionId = this.InteractionId,
                InteractionActMembers = this.InteractionActMembers == null ? null : this.InteractionActMembers.Select(d => d.FromDTO()).ToList(),
                InteractionActOrganizationMembers = this.InteractionActOrganizationMembers == null ? null : this.InteractionActOrganizationMembers.Select(d => d.FromDTO()).ToList()
            };
        }
    }
}
