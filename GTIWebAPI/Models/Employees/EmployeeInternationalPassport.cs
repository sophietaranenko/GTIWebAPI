namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeInternationalPassport")]
    public partial class EmployeeInternationalPassport : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [Column("NumberChar")]
        [StringLength(25)]
        public string Number { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }
        public bool? Deleted { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [StringLength(10)]
        public string CountryCode { get; set; }

        [StringLength(50)]
        public string Nationality { get; set; }

        [StringLength(100)]
        public string GivenNames { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(20)]
        public string PersonalNo { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedWhen { get; set; }

        [StringLength(100)]
        public string IssuedBy { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfExpiry { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeInternationalPassport";
            }
        }

        public EmployeeInternationalPassportDTO ToDTO()
        {
            EmployeeInternationalPassportDTO dto = new EmployeeInternationalPassportDTO
            {
                Id = this.Id,
                CountryCode = this.CountryCode,
                DateOfExpiry = this.DateOfExpiry,
                EmployeeId = this.EmployeeId,
                GivenNames = this.GivenNames,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Nationality = this.Nationality,
                Number = this.Number,
                PersonalNo = this.PersonalNo,
                Seria = this.Seria,
                Surname = this.Surname,
                Type = this.Type
            };
            return dto;
        }
    }

    public class EmployeeInternationalPassportDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string Number { get; set; }

        public string Seria { get; set; }

        public string Type { get; set; }

        public string CountryCode { get; set; }

        public string Nationality { get; set; }

        public string GivenNames { get; set; }

        public string Surname { get; set; }

        public string PersonalNo { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public string IssuedBy { get; set; }

        public DateTime? DateOfExpiry { get; set; }
    }
}
