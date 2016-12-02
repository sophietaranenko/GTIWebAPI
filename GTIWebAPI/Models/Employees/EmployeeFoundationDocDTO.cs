using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeFoundationDocDTO
    {
        public int Id { get; set; }

        public string Seria { get; set; }

        public int? Number { get; set; }

        public string Description { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public string IssuedBy { get; set; }

        public FoundationDocumentDTO FoundationDocument { get; set; }
    }
}
