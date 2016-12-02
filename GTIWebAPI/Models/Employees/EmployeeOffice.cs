namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeOffice")]
    public partial class EmployeeOffice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OfficeId { get; set; }
        public virtual Office Office { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int? ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateBegin { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int? EmployeeId { get; set; }

        public bool? Deleted { get; set; }
    }
}
