namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeGun")]
    public partial class EmployeeGun : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [Column("NumberChar")]
        [StringLength(25)]
        public string Number { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

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

        protected override string TableName
        {
            get
            {
                return "EmployeeGun";
            }
        }
    }
}
