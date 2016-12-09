namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Table for scans of documents 
    /// </summary>
    [Table("EmployeeDocumentScan")]
    public partial class EmployeeDocumentScan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id in table named like stored in prop TableName
        /// </summary>
        public int? TableId { get; set; }

        /// <summary>
        /// Byte array image, should be replaced 
        /// </summary>
        [Column(TypeName = "image")]
        public byte[] Scan { get; set; }

        /// <summary>
        /// Scan document table name
        /// </summary>
        [StringLength(50)]
        public string TableName { get; set; }

        /// <summary>
        /// Name of file
        /// </summary>
        [StringLength(50)]
        public string ScanName { get; set; }

        /// <summary>
        /// Deleted mark
        /// </summary>
        public bool? Deleted { get; set; }
    }
}
