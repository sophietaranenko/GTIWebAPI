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
    }
}
