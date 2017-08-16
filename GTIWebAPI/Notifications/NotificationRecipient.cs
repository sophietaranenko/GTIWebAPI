using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Notifications
{
    [Table("NotificationRecipient")]
    public partial class NotificationRecipient
    {
        public int Id { get; set; }

        public int? NotificationId { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Notification Notification { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
