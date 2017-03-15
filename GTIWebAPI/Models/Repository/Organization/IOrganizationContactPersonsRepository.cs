using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Organization
{
    public interface IOrganizationContactPersonsRepository
    {
        OrganizationContactPersonView Add(OrganizationContactPerson organizationContactPerson);

        OrganizationContactPersonView Delete(int id);

        OrganizationContactPersonView Edit(OrganizationContactPerson organizationContactPerson);

        OrganizationContactPersonView Get(int id);

        List<OrganizationContactPersonView> GetByOrganizationId(int organizationId);
        
    }
}
