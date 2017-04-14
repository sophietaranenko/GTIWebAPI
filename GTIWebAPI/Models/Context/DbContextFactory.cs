using GTIWebAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.Models.Context
{
    public class DbContextFactory : IDbContextFactory, IPrincipalProvider
    {
        public IAppDbContext CreateDbContext()
        {
            string userName = GetUserName();
            string originalString = "";

            if (userName == null)
            {
                userName = "UserIsNotAllowed";
                throw new WrongDatabaseLoginException("User is not allowed");
            }

            if (IsInRole("Personnel"))
            {
                originalString = ConfigurationManager.ConnectionStrings["DbPersonnel"].ConnectionString;
                originalString = originalString.Replace("somefakelogin", userName);
                return new MainDbContext(originalString);
            }
            else if (IsInRole("Organization"))
            {
                originalString = ConfigurationManager.ConnectionStrings["DbOrganization"].ConnectionString;
                originalString = originalString.Replace("somefakelogin", userName);
                return new MainDbContext(originalString);
            }
            else
            {
                throw new WrongDatabaseLoginException("User is not in any permitted role");
            }
        }

        public string GetUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public bool IsInRole(string roleName)
        {
            return HttpContext.Current.User.IsInRole(roleName);
        }
    }
}
