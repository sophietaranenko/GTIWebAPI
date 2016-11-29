namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeCar")]
    public partial class EmployeeCar 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public short? MassMax { get; set; }

        public short? MassInService { get; set; }

        public short? Capacity { get; set; }

        public byte? NumberOfSeats { get; set; }

        public short? RegistrationYear { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        public int? AddressId { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodOfValidity { get; set; }

        public int? Number { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        [StringLength(20)]
        public string Ownership { get; set; }

        [StringLength(10)]
        public string Make { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        [StringLength(30)]
        public string Description { get; set; }

        [StringLength(20)]
        public string IdentificationNumber { get; set; }

        [StringLength(2)]
        public string VehicleCategory { get; set; }

        [StringLength(2)]
        public string FuelType { get; set; }

        [StringLength(20)]
        public string Colour { get; set; }

        [StringLength(10)]
        public string RegistrationNumber { get; set; }

        [StringLength(20)]
        public string Owner { get; set; }

        [StringLength(30)]
        public string GivenName { get; set; }
        [StringLength(100)]
        public string IssuedBy { get; set; }
        public bool? Deleted { get; set; }

        public virtual Address Address { get; set; }
    }
}
