using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeList
    {
        public IEnumerable<OfficeDTO> Offices { get; set; }

        public IEnumerable<ProfessionDTO> Professions { get; set; }

        public IEnumerable<DepartmentDTO> Departments { get; set; }

        public IEnumerable<FoundationDocumentDTO> DocumentTypes { get; set; }

        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<ContactTypeDTO> ContactTypes { get; set; }
    }
}
