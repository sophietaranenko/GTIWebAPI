using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeInternationalPassportDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? Number { get; set; }

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
