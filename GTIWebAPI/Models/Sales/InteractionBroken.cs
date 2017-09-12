using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Sales
{
    [Table("InteractionBroken")]
    public class InteractionBroken
    {
        public InteractionBroken()
        {
            Interactions = new HashSet<Interaction>();
        }

        public int Id { get; set; }

        [Column("ReasonId")]
        public int? InteractionBrokenReasonId { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public virtual InteractionBrokenReason InteractionBrokenReason { get; set; }

        public virtual ICollection<Interaction> Interactions { get; set; }

        public InteractionBrokenDTO ToDTO()
        {
            return new InteractionBrokenDTO()
            {
                Id = this.Id,
                InteractionBrokenReason = this.InteractionBrokenReason == null ? null : this.InteractionBrokenReason.ToDTO(),
                InteractionBrokenReasonId = this.InteractionBrokenReasonId,
                Remark = this.Remark
            };
        }

    }

    public class InteractionBrokenDTO
    {
        public int Id { get; set; }

        public int? InteractionBrokenReasonId { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public InteractionBrokenReasonDTO InteractionBrokenReason { get; set; }

        public InteractionBroken FromDTO()
        {
            return new InteractionBroken()
            {
                Id = this.Id,
                InteractionBrokenReason = this.InteractionBrokenReason == null ? null : this.InteractionBrokenReason.FromDTO(),
                InteractionBrokenReasonId = this.InteractionBrokenReasonId,
                Remark = this.Remark
            };
        }

    }
}
