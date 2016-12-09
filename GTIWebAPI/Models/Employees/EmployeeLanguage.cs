namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeLanguage")]
    public partial class EmployeeLanguage : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? LanguageId { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateBegin { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        public int? Type { get; set; }

        [StringLength(250)]
        public string Definition { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
        public bool? Deleted { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Language Language { get; set; }
        protected override string TableName
        {
            get
            {
                return "EmployeeLanguage";
            }
        }
    }
}
