using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// DTO для EmployeeCar
    /// </summary>
    public class EmployeeCarDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

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
    }
}
