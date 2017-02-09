using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationGTICreateLinkDTO
    {
        public int OrganizationId { get; set; }

        public IEnumerable<int> OrganizationGTIIds { get; set; }
    }
}
