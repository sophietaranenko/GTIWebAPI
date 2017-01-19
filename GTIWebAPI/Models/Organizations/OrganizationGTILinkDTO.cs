using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class OrganizationGTILinkDTO
    {
        public int Id { get; set; }

        public int? OrganizationGTIId { get; set; }

        public int? OrganizationId { get; set; }

        public OrganizationGTIDTO OrganizationGTI { get; set; }


    }
}
