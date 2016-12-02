namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeInternationalPassport")]
    public partial class EmployeeInternationalPassport 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? Number { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }
        public bool? Deleted { get; set; }

        [StringLength(5)]
        public string Type { get; set; }

        [StringLength(5)]
        public string CountryCode { get; set; }

        [StringLength(50)]
        public string Nationality { get; set; }

        [StringLength(50)]
        public string GivenNames { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(20)]
        public string PersonalNo { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime?  IssuedWhen { get; set; }

        [StringLength(10)]
        public string IssuedBy { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfExpiry { get; set; }
    }
}
