using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Sales
{
    [Table("InteractionStatus")]
    public partial class InteractionStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<InteractionStatusMovement> InteractionStatusMovements { get; set; }

        public InteractionStatusDTO ToDTO()
        {
            return new InteractionStatusDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }

    public class InteractionStatusDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
