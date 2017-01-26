using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationGTILinkView
    {

        public string ClientName { get; set; }

        public int ClientId { get; set; }

        public string ClientGTIName { get; set; }

        public int ClientGTIId { get; set; }

        public int OfficeId { get; set; }

        public string OfficeName { get; set; }

    }
}
