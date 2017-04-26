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

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int? OfficeId { get; set; }

        public int? CreatorId { get; set; }

        public bool? Deleted { get; set; }

        public virtual Office Office {get;set;}

        [ForeignKey("CreatorId")]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<UserRightMaskRight> Rights { get; set; }

    }

    public class UserRightMaskDTO
    {
        public int? CreatorId { get; set; }

        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int? OfficeId { get; set; }

        public string OfficeShortName { get; set; }

        public int? BoxId { get; set; }

        public string BoxName { get; set; }

        public string BoxLongName { get; set; }

        public int? ActionId { get; set; }

        public string ActionName { get; set; }

        public string ActionLongName { get; set; }

        public int? ControllerId { get; set; }

        public string ControllerName { get; set; }

        public string ControllerLongName { get; set; }

        public bool Value { get; set; }

        public RightControllerBoxDTO Box {get;set;}

        public RightControllerDTO Controller { get; set; }

        public RightControllerActionDTO Action { get; set; }

        public OfficeDTO Office { get; set; }

    }

    public class UserRightMaskTreeView : IEquatable<UserRightMaskTreeView>
    {
        public int? CreatorId { get; set; }

        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int? OfficeId { get; set; }

        public OfficeDTO Office { get; set; }

        public IEnumerable<RightControllerBoxDTO> Boxes { get; set; }

        public bool Equals(UserRightMaskTreeView other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
