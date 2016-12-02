using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeGunDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string Seria { get; set; }

        public int? Number { get; set; }

        public string Description { get; set; }

        public string IssuedBy { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
