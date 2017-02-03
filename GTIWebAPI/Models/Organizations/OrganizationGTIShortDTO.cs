using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationGTIShortDTO
    {
        public int OrganizationGTIId { get; set; }

        public int OrganizationGTIOfficeId { get; set; }

        public string OrganizationGTIOfficeShortName { get; set; }

        public string OrganizationGTIEnglishName { get; set; }

        public string CreatorShortName { get; set; }

        public int? CreatorId { get; set; }
    }
}
