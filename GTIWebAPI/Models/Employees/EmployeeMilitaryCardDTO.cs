using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeMilitaryCardDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? Number { get; set; }

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
