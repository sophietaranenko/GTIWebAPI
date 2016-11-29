namespace GTIWebAPI.Models.Employees
{
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeContact")]
    public partial class EmployeeContact 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Type { get; set; }
        [StringLength(100)]
        public string Value { get; set; }
        public int? EmployeeId { get; set; }

        public bool? Deleted { get; set; }
        [NotMapped]
        public ContactType ContactType { get; set; }
    }
}
