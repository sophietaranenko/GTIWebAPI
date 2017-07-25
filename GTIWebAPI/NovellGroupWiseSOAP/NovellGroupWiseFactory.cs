using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.NovellGroupWiseSOAP
{
    public interface INovellGroupWiseFactory
    {
        INovellGroupWise CreateNovellGroupWise();
    }

    public class NovellGroupWiseFactory : INovellGroupWiseFactory, IPrincipalProvider
    {
        public INovellGroupWise CreateNovellGroupWise()
        {
            return new NovellGroupWise(GetPostOffice(), GetSessionId());
        }

        public string GetPostOffice()
        {
            return HttpContext.Current.User.Identity.GetPostOfficeAddress();
        }

        public string GetSessionId()
        {
            return HttpContext.Current.User.Identity.GetSessionId();
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
