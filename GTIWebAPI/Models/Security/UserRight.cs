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
        //public UserRight()
        //{

        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public string AspNetUserId { get; set; }

        //[ForeignKey]
        public Int32 OfficeId { get; set; }

        public Int32 ControllerId { get; set; }

        public Int32 ActionId { get; set; }

        [ForeignKey("ControllerId")]
        public virtual Models.Security.Controller Controller { get; set; }

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

    public class UserRightDTO
    {
        [Key]
        public Int32 OfficeId { get; set; }
        public string OfficeName { get; set; }
        public List<ControllerDTO> Controllers { get; set; }
    }

    public class UserRightEditDTO
    {
        public Int32 OfficeId { get; set; }
        public Int32 ControllerId { get; set; }
        public Int32 ActionId { get; set; }
    }

}