namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeMilitaryCard")]
    public partial class EmployeeMilitaryCard : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [Column("NumberChar")]
        [StringLength(25)]
        public string Number { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OfficeDate { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [StringLength(50)]
        public string Rank { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string TypeGroup { get; set; }

        [StringLength(50)]
        public string Corps { get; set; }

        [StringLength(250)]
        public string Specialty { get; set; }

        [StringLength(50)]
        public string SpecialtyNumber { get; set; }

        [StringLength(250)]
        public string Office { get; set; }
        public bool? Deleted { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeMilitaryCard";
            }
        }

        public EmployeeMilitaryCardDTO ToDTO()
        {
            EmployeeMilitaryCardDTO dto = new EmployeeMilitaryCardDTO
            {
                Category = this.Category,
                Corps = this.Corps,
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                Number = this.Number,
                Office = this.Office,
                OfficeDate = this.OfficeDate,
                Rank = this.Rank,
                Seria = this.Seria,
                Specialty = this.Specialty,
                SpecialtyNumber = this.SpecialtyNumber,
                TypeGroup = this.TypeGroup,
            };
            return dto;
        }
    }

    public class EmployeeMilitaryCardDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string Number { get; set; }

        public DateTime? OfficeDate { get; set; }

        public string Seria { get; set; }

        public string Rank { get; set; }

        public string Category { get; set; }

        public string TypeGroup { get; set; }

        public string Corps { get; set; }

        public string Specialty { get; set; }

        public string SpecialtyNumber { get; set; }

        public string Office { get; set; }

    }
}
