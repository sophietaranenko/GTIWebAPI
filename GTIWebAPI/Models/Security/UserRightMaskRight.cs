using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    [Table("AspNetUserRightMaskRights")]
    public class UserRightMaskRight
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ActionId { get; set; }

        public Guid MaskId { get; set; }

        public virtual RightControllerAction Action { get; set; }

        [ForeignKey("MaskId")]
        public virtual UserRightMask Mask { get; set; }

    }
}
