namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Service;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Data from car registration document
    /// </summary>
    [Table("EmployeeCar")]
    public partial class EmployeeCar : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
       // public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public short? MassMax { get; set; }

        public short? MassInService { get; set; }

        public short? Capacity { get; set; }

        public byte? NumberOfSeats { get; set; }

        public short? RegistrationYear { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodOfValidity { get; set; }

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

        public EmployeeCarDTO ToDTO()
        {
            EmployeeCarDTO dto = new EmployeeCarDTO
            {
                Capacity = this.Capacity,
                Colour = this.Colour,
                Description = this.Description,
                VehicleCategory = this.VehicleCategory,
                EmployeeId = this.EmployeeId,
                FuelType = this.FuelType,
                GivenName = this.GivenName,
                Id = this.Id,
                IdentificationNumber = this.IdentificationNumber,
                IssuedBy = this.IssuedBy,
                Make = this.Make,
                MassInService = this.MassInService,
                MassMax = this.MassMax,
                Number = this.Number,
                NumberOfSeats = this.NumberOfSeats,
                Owner = this.Owner,
                Ownership = this.Ownership,
                PeriodOfValidity = this.PeriodOfValidity,
                RegistrationDate = this.RegistrationDate,
                RegistrationNumber = this.RegistrationNumber,
                RegistrationYear = this.RegistrationYear,
                Seria = this.Seria,
                Type = this.Type
            };
            return dto;
        }
        protected override string TableName
        {
            get
            {
                return "EmployeeCar";
            }
        }

    }

    public class EmployeeCarDTO
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public short? MassMax { get; set; }

        public short? MassInService { get; set; }

        public short? Capacity { get; set; }

        public byte? NumberOfSeats { get; set; }

        public short? RegistrationYear { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime? PeriodOfValidity { get; set; }

        public string Number { get; set; }

        public string Seria { get; set; }

        public string Ownership { get; set; }

        public string Make { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string IdentificationNumber { get; set; }

        /// <summary>
        /// A, B, C, D ...
        /// </summary>
        public string VehicleCategory { get; set; }

        /// <summary>
        /// Бензин, D ... 
        /// </summary>
        public string FuelType { get; set; }

        public string Colour { get; set; }

        public string RegistrationNumber { get; set; }

        public string Owner { get; set; }

        public string GivenName { get; set; }

        public string IssuedBy { get; set; }

        public EmployeeCar FromDTO()
        {
            return new EmployeeCar()
            {
                Capacity = this.Capacity,
                Colour = this.Colour,
                Seria = this.Seria,
                MassInService = this.MassInService,
                NumberOfSeats = this.NumberOfSeats,
                Description = this.Description,
                EmployeeId = this.EmployeeId,
                FuelType = this.FuelType,
                GivenName = this.GivenName,
                Id = this.Id,
                IdentificationNumber = this.IdentificationNumber,
                IssuedBy = this.IssuedBy,
                Make = this.Make,
                MassMax = this.MassMax,
                Number = this.Number,
                Owner = this.Owner,
                Ownership = this.Ownership,
                PeriodOfValidity = this.PeriodOfValidity,
                RegistrationDate = this.RegistrationDate,
                RegistrationNumber = this.RegistrationNumber,
                RegistrationYear = this.RegistrationYear,
                Type = this.Type,
                VehicleCategory = this.VehicleCategory 
            };
        }
    }
}
