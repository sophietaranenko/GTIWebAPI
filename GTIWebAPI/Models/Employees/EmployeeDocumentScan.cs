namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Table for scans of documents 
    /// </summary>
    [Table("EmployeeDocumentScan")]
    public partial class EmployeeDocumentScan : GTITable 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id in table named like stored in prop TableName
        /// </summary>
        [Column("TableId")]
        public int? ScanTableId { get; set; }

        /// <summary>
        /// Byte array image, should be replaced 
        /// </summary>
        [Column(TypeName = "image")]
        public byte[] Scan { get; set; }

        /// <summary>
        /// Scan document table name
        /// </summary>
        [StringLength(50)]
        [Column("TableName")]
        public string ScanTableName { get; set; }

        /// <summary>
        /// Name of file
        /// </summary>
        [StringLength(500)]
        public string ScanName { get; set; }

        /// <summary>
        /// Deleted mark
        /// </summary>
        public bool? Deleted { get; set; }
        protected override string TableName
        {
            get
            {
                return "EmployeeDocumentScan";
            }
        }
    }
}
