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
        public IEnumerable<FoundationDocumentDTO> FoundationDocuments { get; set; }


        public IEnumerable<ContactTypeDTO> ContactTypes { get; set; }

        public IEnumerable<EducationStudyFormDTO> EducationStudyForms { get; set; }

        public EmployeeLanguageList EmployeeLanguageList { get; set; }

        public AddressList AddressList { get; set; }

        public EmployeeOfficeList EmployeeOfficeList { get; set; }

    }
}
