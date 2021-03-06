namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeSicklist")]
    public partial class EmployeeSicklist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        public int? Number { get; set; }

        [StringLength(250)]
        public string IssuedBy { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedWhen { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
        public bool? Deleted { get; set; }
    }
}
