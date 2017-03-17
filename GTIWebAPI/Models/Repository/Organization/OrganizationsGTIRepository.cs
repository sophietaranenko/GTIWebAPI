using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Context;

namespace GTIWebAPI.Models.Repository.Organization
{
    public class OrganizationsGTIRepository : IOrganizationsGTIRepository
    {
        private IDbContextFactory factory;
        public OrganizationsGTIRepository()
        {
            factory = new DbContextFactory();
        }

        public OrganizationsGTIRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public List<OrganizationGTI> Search(List<int> officeIds, string registrationNumber)
        {
            List<OrganizationGTI> orgs = new List<OrganizationGTI>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                orgs = db.SearchOrganizationGTI(officeIds, registrationNumber).ToList();
                foreach (var item in orgs)
                {
                    item.Office = db.Offices
                        .Where(d => d.Id == item.OfficeId)
                        .FirstOrDefault();
                }
            }
            return orgs;
        }
    }
}
