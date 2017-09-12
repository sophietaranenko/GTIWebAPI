using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Sales
{
    [Table("InteractionSucceed")]
    public class InteractionSucceed
    {
        public int Id { get; set; }

        public int? OfficeId { get; set; }

        public virtual Office Office { get; set; }

        public int? DealNumber { get; set; }

        public Guid? DealGTIVFPId { get; set; }

        public virtual ICollection<Interaction> Interactions { get;  set; }

        public InteractionSucceedDTO ToDTO()
        {
            return new InteractionSucceedDTO()
            {
                Id = this.Id,
                DealGTIVFPId = this.DealGTIVFPId,
                DealNumber = this.DealNumber,
                Office = this.Office == null ? null : this.Office.ToDTO(),
                OfficeId = this.OfficeId
            };
        }
    }

    public class InteractionSucceedDTO
    {
        public int Id { get; set; }

        public int? OfficeId { get; set; }

        public OfficeDTO Office { get; set; }

        public int? DealNumber { get; set; }

        public Guid? DealGTIVFPId { get; set; }

        public InteractionSucceed FromDTO()
        {
            return new InteractionSucceed()
            {
                Id = this.Id,
                DealGTIVFPId = this.DealGTIVFPId,
                DealNumber = this.DealNumber,
                Office = this.Office == null ? null : this.Office.FromDTO(),
                OfficeId = this.OfficeId
            };
        }
    }
}
