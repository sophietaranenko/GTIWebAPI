using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    [Table("AspNetUserRightMasks")]
    public class UserRightMask
    {
        public UserRightMask()
        {
            Rights = new HashSet<UserRightMaskRight>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int OfficeId { get; set; }

        public int CreatorId { get; set; }

        public virtual Office Office {get;set;}

        [ForeignKey("CreatorId")]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<UserRightMaskRight> Rights { get; set; }

        public UserRightMaskDTO ToDTO()
        {
            UserRightMaskDTO dto = new UserRightMaskDTO()
            {
                Id = this.Id,
                Name = this.Name,
                OfficeId = this.OfficeId,
                Office = this.Office == null ? null : this.Office.ToDTO(),
                Controllers = this.Rights.Select(d => d.Action.Controller).Distinct().Select(d => d.ToDTO())
             };
            foreach (var item in dto.Controllers)
            {

            }
            return dto;
        }


    }


    public class UserRightMaskDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OfficeId { get; set; }

        public int CreatorId { get; set; }

        public OfficeDTO Office { get; set; }

        public IEnumerable<RightControllerDTO> Controllers { get; set; }

        public UserRightMask FromDTO()
        {
            return new UserRightMask()
            {
                Id = this.Id,
                Name = this.Name,
                OfficeId = this.OfficeId,
                Office = this.Office == null ? null : this.Office.FromDTO()
            };
        }

    }
}
