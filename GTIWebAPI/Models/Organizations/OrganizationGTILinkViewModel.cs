using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class OrganizationGTILinkViewModel
    {
        public int Id { get; set; }
        
        public Organization Client { get; set; }
        public OrganizationGTI ClientGTI { get; set; }
    }
}
