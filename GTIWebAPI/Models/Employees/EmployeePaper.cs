namespace GTIWebAPI.Models.Employees
{
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeePaper")]
    public partial class EmployeePaper
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? PaperTypeId { get; set; }

        public bool? Deleted { get; set; }

        public virtual PaperType PaperType { get; set; }
        public DateTime? PaperDate { get; set; }
    }
}
