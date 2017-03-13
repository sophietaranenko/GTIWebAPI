using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public static class AppDbContextFactory
    {
        public static IAppDbContext CreateDbContext(IPrincipal user)
        {
            string userName = user.Identity.Name;
            string originalString = "";

            if (userName == null)
            {
                userName = "UserIsNotAllowed";
                throw new ArgumentException("User is not allowed");
            }

            if (user.IsInRole("Personnel"))
            {
                originalString = ConfigurationManager.ConnectionStrings["DbPersonnel"].ConnectionString;
                originalString = originalString.Replace("somefakelogin", userName);
                return new MainDbContext(originalString);
            }
            else if (user.IsInRole("Organization"))
            {
                originalString = ConfigurationManager.ConnectionStrings["DbOrganization"].ConnectionString;
                originalString = originalString.Replace("somefakelogin", userName);
                return new MainDbContext(originalString);
            }
            else
            {
                throw new ArgumentException("Database login is invalid");
            }
        }
    }
}
