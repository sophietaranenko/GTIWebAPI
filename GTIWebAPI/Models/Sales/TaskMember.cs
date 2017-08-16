namespace GTIWebAPI.Models.Sales
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaskMember")]
    public partial class TaskMember
    {
        public Guid Id { get; set; }

        public Guid? TaskId { get; set; }

        public int? EmployeeId { get; set; }

        public int? TaskMemberRoleId { get; set; }

        public virtual Task Task { get; set; }

        public virtual TaskMemberRole TaskMemberRole { get; set; }
    }
}
