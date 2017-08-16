using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Notifications
{
    [Table("Notification")]
    public partial class Notification
    {
        public Notification()
        {
            NotificationRecipients = new HashSet<NotificationRecipient>();
        }

        public int Id { get; set; }

        public string NotificationText { get; set; }

        public string LinkName { get; set; }

        public int LinkId { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime? NotificationDate { get; set; }

        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }

    }
}
