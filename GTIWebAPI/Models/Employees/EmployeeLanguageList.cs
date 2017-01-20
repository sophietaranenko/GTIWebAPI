using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeLanguageList
    {
        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<EmployeeLanguageTypeDTO> EmployeeLanguageTypes { get; set; }

    }
}
