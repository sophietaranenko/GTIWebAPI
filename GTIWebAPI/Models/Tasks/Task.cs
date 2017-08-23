namespace GTIWebAPI.Models.Tasks
{
    using Employees;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task
    {
        public Guid Id { get; set; }

        public int? TableId { get; set; }

        [StringLength(100)]
        public string TableName { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? TaskDate { get; set; }

        [StringLength(500)]
        public string TaskText { get; set; }

        public int? CreatorId { get; set; }

        public int? DoerId { get; set; }

        public bool? Done { get; set; }

        public DateTime? DoneDate { get; set; }

        public virtual Employee Creator { get; set; }

        public virtual Employee Doer { get; set; }

        public TaskDTO ToDTO()
        {
            return new TaskDTO()
            {
                Creator = this.Creator == null ? null : this.Creator.ToShortForm(),
                CreatorId = this.CreatorId,
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                Doer = this.Doer == null ? null : this.Doer.ToShortForm(),
                DoerId = this.DoerId,
                Done = this.Done,
                TaskDate = this.TaskDate,
                Id = this.Id,
                TableId = this.TableId,
                TableName = this.TableName,
                TaskText = this.TaskText,
                DoneDate = this.DoneDate
            };
        }

    }

    public class TaskDTO
    {
        public Guid Id { get; set; }

        public int? TableId { get; set; }

        public string TableName { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? TaskDate { get; set; }

        public string TaskText { get; set; }

        public int? CreatorId { get; set; }

        public int? DoerId { get; set; }

        public bool? Done { get; set; }

        public DateTime? DoneDate { get; set; }

        public EmployeeShortForm Creator { get; set; }

        public EmployeeShortForm Doer { get; set; }

        public Task FromDTO()
        {
            return new Task()
            {
                CreatorId = this.CreatorId,
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                DoerId = this.DoerId,
                Done = this.Done,
                TaskDate = this.TaskDate,
                Id = this.Id,
                TableId = this.TableId,
                TableName = this.TableName,
                TaskText = this.TaskText,
                DoneDate = this.DoneDate
            };
        }
    }
}
