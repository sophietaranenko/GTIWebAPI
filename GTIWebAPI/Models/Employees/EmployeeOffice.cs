namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Service;
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeOffice")]
    public partial class EmployeeOffice : GTITable
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

        public virtual Employee Employee { get; set; }

        public bool? Deleted { get; set; }

        public EmployeeOfficeDTO ToDTO()
        {
            EmployeeOfficeDTO dto = new EmployeeOfficeDTO
            {
                Id = this.Id,
                Office = this.Office == null ? null : this.Office.ToDTO(),
                OfficeId = this.OfficeId,
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                Department = this.Department == null ? null : this.Department.ToDTO(),
                DepartmentId = this.DepartmentId,
                EmployeeId = this.EmployeeId,
                Profession = this.Profession == null ? null : this.Profession.ToDTO(),
                ProfessionId = this.ProfessionId,
                Remark = this.Remark
             };
            return dto;
        }
       
        protected override string TableName
        {
            get
            {
                return "EmployeeOffice";
            }
        }
    }

    public class EmployeeOfficeDTO
    {
        public int Id { get; set; }

        public OfficeDTO Office { get; set; }

        public DepartmentDTO Department { get; set; }

        public virtual ProfessionDTO Profession { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Remark { get; set; }

        public int? EmployeeId { get; set; }

        public int? OfficeId { get; set; }

        public int? DepartmentId { get; set; }

        public int? ProfessionId { get; set; }

        public EmployeeOffice FromDTO()
        {
            return new EmployeeOffice()
            {
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                DepartmentId = this.DepartmentId,
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                OfficeId = this.OfficeId,
                ProfessionId = this.ProfessionId,
                Remark = this.Remark
            };
        }

    }
}
