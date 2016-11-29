namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeMilitaryCard")]
    public partial class EmployeeMilitaryCard 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? Number { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OfficeDate { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        [StringLength(20)]
        public string Rank { get; set; }

        [StringLength(20)]
        public string Category { get; set; }

        [StringLength(20)]
        public string TypeGroup { get; set; }

        [StringLength(20)]
        public string Corps { get; set; }

        [StringLength(50)]
        public string Specialty { get; set; }

        [StringLength(10)]
        public string SpecialtyNumber { get; set; }

        [StringLength(50)]
        public string Office { get; set; }
        public bool? Deleted { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
