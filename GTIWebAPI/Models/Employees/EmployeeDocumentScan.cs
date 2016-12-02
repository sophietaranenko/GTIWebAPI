namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeDocumentScan")]
    public partial class EmployeeDocumentScan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? TableId { get; set; }

        [Column(TypeName = "image")]
        public byte[] Scan { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(50)]
        public string ScanName { get; set; }
        public bool? Deleted { get; set; }
    }
}
