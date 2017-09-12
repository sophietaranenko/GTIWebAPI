using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Sales
{
    [Table("interactionBrokenReason")]
    public class InteractionBrokenReason
    {
        public InteractionBrokenReason()
        {
            InteractionBroken = new HashSet<InteractionBroken>();
        }

        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual ICollection<InteractionBroken> InteractionBroken { get; set; }

        public InteractionBrokenReasonDTO ToDTO()
        {
            return new InteractionBrokenReasonDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }

    public class InteractionBrokenReasonDTO
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public InteractionBrokenReason FromDTO()
        {
            return new InteractionBrokenReason()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }
}
