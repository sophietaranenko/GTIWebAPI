using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeDrivingLicenseDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string Number { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Seria { get; set; }

        public string IssuedBy { get; set; }

        public string Category { get; set; }

    }
}
