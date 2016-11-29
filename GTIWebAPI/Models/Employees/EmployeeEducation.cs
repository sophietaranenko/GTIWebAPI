namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeEducation")]
    public partial class EmployeeEducation 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? Number { get; set; }

        public int? Year { get; set; }

        public int? StudyForm { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        [StringLength(150)]
        public string Institution { get; set; }

        [StringLength(50)]
        public string Specialty { get; set; }

        [StringLength(50)]
        public string Qualification { get; set; }
        public bool? Deleted { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
