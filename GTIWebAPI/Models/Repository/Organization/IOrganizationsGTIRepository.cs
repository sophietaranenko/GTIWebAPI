using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Organization
{
    public interface IOrganizationsGTIRepository
    {
        List<OrganizationGTI> Search(List<int> officeIds, string registrationNumber);
    }
}
