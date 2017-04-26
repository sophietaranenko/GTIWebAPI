using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    [Table("AspNetUserRights")]
    public class UserRight : GTITable
    {
        [Key]
        public Guid Id { get; set; }

        public string AspNetUserId { get; set; }

        public Int32 OfficeId { get; set; }

        public Int32 ActionId { get; set; }

        [ForeignKey("ActionId")]
        public virtual Models.Security.Action Action { get; set; }

        [ForeignKey("OfficeId")]
        public virtual OfficeSecurity OfficeSecurity { get; set; }

        [ForeignKey("AspNetUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        protected override string TableName
        {
            get
            {
                return "AspNetUserRights";
            }
        }
    }

    public class UserRightOfficeDTO
    {
        public string UserId { get; set; }

        public int? OfficeId { get; set; }

        public string OfficeShortName { get; set; }

        public int? ActionId { get; set; }

        public string ActionName { get; set; }

        public string ActionLongName { get; set; }

        public int? ControllerId { get; set; }

        public string ControllerName { get; set; }

        public string ControllerLongName { get; set; }

        public int? BoxId { get; set; }

        public string BoxName { get; set; }

        public string BoxLongName { get; set; }

        public bool? Value { get; set; }
    }

    public class UserRightTreeView : IEquatable<UserRightTreeView>
    { 

        public string UserId { get; set; }

        public int? OfficeId { get; set; }

        public OfficeDTO Office { get; set; }

        public IEnumerable<RightControllerBoxDTO> Boxes { get; set; }

        public bool Equals(UserRightTreeView other)
        {
            return this.UserId == other.UserId;
        }

        public override int GetHashCode()
        {
            return this.UserId.GetHashCode();
        }
    }


public class UserRightDTO
    {
        [Key]
        public Int32 OfficeId { get; set; }

        public string OfficeName { get; set; }

        public List<ControllerBoxDTO> Boxes { get; set; }
    }

    public class UserRightEditDTO
    {
        public Int32 OfficeId { get; set; }

        public Int32 ControllerId { get; set; }

        public Int32 ActionId { get; set; }
    }

}