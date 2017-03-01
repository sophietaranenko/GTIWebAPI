namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
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

        public int? EmployeeLanguageTypeId { get; set; }

        [StringLength(250)]
        public string Definition { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public bool? Deleted { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Language Language { get; set; }

        public virtual EmployeeLanguageType EmployeeLanguageType { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeLanguage";
            }
        }

        public EmployeeLanguageDTO ToDTO()
        {
            EmployeeLanguageDTO dto = new EmployeeLanguageDTO()
            {
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                Definition = this.Definition,
                EmployeeId = this.EmployeeId,
                EmployeeLanguageType = this.EmployeeLanguageType == null ? null : this.EmployeeLanguageType.ToDTO(),
                EmployeeLanguageTypeId = this.EmployeeLanguageTypeId,
                Id = this.Id,
                Language = this.Language == null ? null : this.Language.ToDTO(),
                LanguageId = this.LanguageId,
                Remark = this.Remark
            };
            return dto;
        }
    }

    public class EmployeeLanguageDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? LanguageId { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public int? EmployeeLanguageTypeId { get; set; }

        public string Definition { get; set; }

        public string Remark { get; set; }

        public LanguageDTO Language { get; set; }

        public EmployeeLanguageTypeDTO EmployeeLanguageType { get; set; }
    }
}
