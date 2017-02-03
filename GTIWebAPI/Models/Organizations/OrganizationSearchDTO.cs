using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationSearchDTO
    {
        public int Id { get; set; }

        public string NativeName { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public IEnumerable<OrganizationGTIShortDTO> OrganizationGTILinks { get; set; }
    }
}
