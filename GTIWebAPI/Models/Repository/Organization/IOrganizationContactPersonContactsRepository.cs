using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Organization
{
    public interface IOrganizationContactPersonContactsRepository
    {
        List<OrganizationContactPersonContact> GetByOrganizationContactPersonId(int organizationContactPersonId);

        OrganizationContactPersonContact Get(int id);

        OrganizationContactPersonContact Edit(OrganizationContactPersonContact organizationContactPersonContact);

        OrganizationContactPersonContact Add(OrganizationContactPersonContact organizationContactPersonContact);

        OrganizationContactPersonContact Delete(int id);
        
    }
}
