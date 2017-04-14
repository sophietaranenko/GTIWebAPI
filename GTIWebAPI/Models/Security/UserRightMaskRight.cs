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

        public int MaskId { get; set; }

        public bool Value { get; set; }

        public virtual RightControllerAction Action { get; set; }

        [ForeignKey("MaskId")]
        public virtual UserRightMask Mask { get; set; }

        public UserRightMaskRightDTO ToDTO()
        {
            return new UserRightMaskRightDTO()
            {
                Id = this.Id,
                ActionId = this.ActionId,
                MaskId = this.MaskId,
                Value = this.Value
            };
        }

    }


    public class UserRightMaskRightDTO
    {
        public int Id { get; set; }

        public int ActionId { get; set; }

        public int MaskId { get; set; }

        public bool Value { get; set; }

        public RightControllerActionDTO Action { get; set; }

        public RightControllerDTO Controllers { get; set; }

        public UserRightMaskRight FromDTO()
        {
            return new UserRightMaskRight()
            {
                Id = this.Id,
                ActionId = this.ActionId,
                MaskId = this.MaskId,
                Value = this.Value
            };
        }

    }
}
