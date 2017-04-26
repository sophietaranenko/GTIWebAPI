using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    public static class UserRightsManager
    {
        public static bool GrantOrganizationRights(string userId)
        { 
            using (UserRightsProvider p = new UserRightsProvider())
            {
                p.GrantOrganization(userId);
            }
            return true;
        }

        public static bool GrantStandardPersonnelRights(string userId)
        {
            using (UserRightsProvider p = new UserRightsProvider())
            {
                p.GrantStandardPersonnel(userId);
            }
            return true;
        }



        protected class UserRightsProvider : IDisposable
        {
            RightsDbContext db;

            public UserRightsProvider()
            {
                db = new RightsDbContext();
            }

            public void GrantOrganization(string userId)
            {
                db.GrantRightsToOrganization(userId);
            }

            public void GrantStandardPersonnel(string userId)
            {
                db.GrantStandardRightsToPersonnel(userId);
            }

            public void Dispose()
            {
                db.Dispose();
            }
        }
    }
}
