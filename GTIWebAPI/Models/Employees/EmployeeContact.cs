namespace GTIWebAPI.Models.Employees
{
    using Service;
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeContact")]
    public partial class EmployeeContact : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Type { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        public int? EmployeeId { get; set; }

        public bool? Deleted { get; set; }

        public virtual ContactType ContactType { get; set; }
        protected override string TableName
        {
            get
            {
                return "EmployeeContact";
            }
        }
    }
}
