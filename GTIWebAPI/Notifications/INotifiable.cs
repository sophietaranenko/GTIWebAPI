using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Notifications
{
    public interface INotifiable
    {
        Notification ToAddingNotify(INotificationAuthor author);

        Notification ToEditingNotify(INotificationAuthor author);

        Notification ToDeletingNotify(INotificationAuthor author);
    }
}
