using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeOfficeDTO
    {
        public int Id { get; set; }

        public OfficeDTO Office { get; set; }

        public DepartmentDTO Department { get; set; }

        public virtual ProfessionDTO Profession { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Remark { get; set; }

        public int? EmployeeId { get; set; }

    }
}
