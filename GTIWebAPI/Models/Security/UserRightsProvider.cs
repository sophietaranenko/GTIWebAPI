using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    public class UserRightsProvider : IDisposable
    {
        DbRights db;

        public UserRightsProvider()
        {
            db = new DbRights();
        }

        public void GrantOrganization(string userId)
        {
            db.GrantRightsToOrganization(userId);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
