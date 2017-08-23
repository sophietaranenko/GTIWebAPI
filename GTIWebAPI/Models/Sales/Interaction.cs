namespace GTIWebAPI.Models.Sales
{
    using Employees;
    using Notifications;
    using Organizations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Interaction")]
    public partial class Interaction : INotifiable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Interaction()
        {
            InteractionActs = new HashSet<InteractionAct>();
            InteractionMembers = new HashSet<InteractionMember>();
            InteractionStatusMovements = new HashSet<InteractionStatusMovement>();
        }

        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? CreatorId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public int StatusId { get; set; }

        public virtual Organization Organization { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionAct> InteractionActs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionMember> InteractionMembers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionStatusMovement> InteractionStatusMovements { get; set; }

        public InteractionDTO ToDTO()
        {
            return new InteractionDTO()
            {
                Id = this.Id,
                CreatorId = this.CreatorId,
                Name = this.Name,
                OrganizationId = this.OrganizationId,
                InteractionActs = this.InteractionActs == null ? null : this.InteractionActs.Select(d => d.ToDTO()),
                InteractionMembers = this.InteractionMembers == null ? null : this.InteractionMembers.Select(d => d.ToDTO()),
                InteractionStatusMovements = this.InteractionStatusMovements == null ? null : this.InteractionStatusMovements.Select(d => d.ToDTO()),
                StatusId = this.StatusId
            };
        }

        public Notification NotifyAdd()
        {
            
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            Notification notification = new Notification();
            string text = "";
            //text +=
            return notification;
        }

        public Notification ToAddingNotify(INotificationAuthor author)
        {
            Notification notification = new Notification();
            notification.LinkId = this.Id;
            notification.LinkName = "Interaction";
            notification.EmployeeId = author.Id;
            notification.NotificationText = String.Format(
                "{0} {1} добавил(а) вас как участника в новое взаимодействие \"{2}\"",
                author.FirstName,
                author.Surname,
                this.Name);
            foreach (var item in this.InteractionMembers)
            {
                notification.NotificationRecipients.Add(
                    new NotificationRecipient()
                    {
                        EmployeeId = item.EmployeeId,
                        Employee = item.Employee
                    }
                    );
            }
            return notification;
        }

        public Notification ToEditingNotify(INotificationAuthor author)
        {
            Notification notification = new Notification();
            notification.LinkId = this.Id;
            notification.LinkName = "Interaction";
            notification.EmployeeId = author.Id;
            notification.NotificationText = String.Format(
                "{0} {1} изменил(а) взаимодействие \"{2}\", в котром вы учавствуете",
                author.FirstName,
                author.Surname,
                this.Name);
            foreach (var item in this.InteractionMembers)
            {
                notification.NotificationRecipients.Add(
                    new NotificationRecipient()
                    {
                        EmployeeId = item.EmployeeId,
                        Employee = item.Employee
                    }
                    );
            }
            return notification;
        }

        public Notification ToDeletingNotify(INotificationAuthor author)
        {
            Notification notification = new Notification();
            notification.LinkId = this.Id;
            notification.LinkName = "Interaction";
            notification.EmployeeId = author.Id;
            notification.NotificationText = String.Format(
                "{0} {1} удалил(а) взаимодействие \"{2}\", в котром вы учавствуете",
                author.FirstName,
                author.Surname,
                this.Name);
            foreach (var item in this.InteractionMembers)
            {
                notification.NotificationRecipients.Add(
                    new NotificationRecipient()
                    {
                        EmployeeId = item.EmployeeId,
                        Employee = item.Employee
                    }
                    );
            }            
            return notification;
        }
    }

    public class InteractionDTO
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? CreatorId { get; set; }

        public int StatusId { get; set; }

        public string Name { get; set; }

        public IEnumerable<InteractionActDTO> InteractionActs { get; set; }

        public IEnumerable<InteractionMemberDTO> InteractionMembers { get; set; }

        public IEnumerable<InteractionStatusMovementDTO> InteractionStatusMovements { get; set; }

        public Interaction FromDTO()
        {
            return new Interaction()
            {
                Id = this.Id,
                CreatorId = this.CreatorId,
                InteractionActs = this.InteractionActs == null ? null : this.InteractionActs.Select(d => d.FromDTO()).ToList(),
                InteractionMembers = this.InteractionMembers == null ? null : this.InteractionMembers.Select(d => d.FromDTO()).ToList(),
                InteractionStatusMovements = this.InteractionStatusMovements == null ? null : this.InteractionStatusMovements.Select(d => d.FromDTO()).ToList(),
                Name = this.Name,
                OrganizationId = this.OrganizationId,
                StatusId = this.StatusId
            };
        }

    }

}
