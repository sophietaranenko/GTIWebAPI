namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeOrderContent")]
    public partial class EmployeeOrderContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? EmployeeOrderId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
        public int? EmployeeFoundationDocId { get; set; }
        public bool? Deleted { get; set; }
        public virtual EmployeeFoundationDoc EmployeeFoundationDoc { get; set; }
        public virtual EmployeeOrder EmployeeOrder { get; set; }
    }
}
