namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Service;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Данные из тех паспорта автомобля
    /// </summary>
    [Table("EmployeeCar")]
    public partial class EmployeeCar : GTITable
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

        [Column("NumberChar")]
        [StringLength(25)]
        public string Number { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [StringLength(50)]
        public string Ownership { get; set; }

        [StringLength(50)]
        public string Make { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(30)]
        public string IdentificationNumber { get; set; }

        [StringLength(10)]
        public string VehicleCategory { get; set; }

        [StringLength(20)]
        public string FuelType { get; set; }

        [StringLength(30)]
        public string Colour { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [StringLength(50)]
        public string GivenName { get; set; }
        [StringLength(250)]
        public string IssuedBy { get; set; }
        public bool? Deleted { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeCar";
            }
        }

    }
}
