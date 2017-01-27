using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationView
    {
        public string NativeName { get; set; }

        public string OrganizationLegalFormName { get; set; }

        public string EmployeeShortName { get; set; }

        public int EmployeeId { get; set; }

        public string RussianName { get; set; }

        public string EnglishName { get; set; }

        public string ShortName { get; set; }
    }
}
