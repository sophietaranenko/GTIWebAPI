using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Organization
{
    public interface IOrganizationsRepository
    {
        List<OrganizationSearchDTO> Search(int countryId, string registrationNumber);

        List<OrganizationView> GetAll(List<int> officeIds);

        Organizations.Organization GetView(int id);

        Organizations.Organization GetEdit(int id);

        Organizations.Organization Edit(Organizations.Organization organization);

        Organizations.Organization Add(Organizations.Organization organization);

        Organizations.Organization DeleteOrganization(int id);

        OrganizationList GetTypes();
    }
}
