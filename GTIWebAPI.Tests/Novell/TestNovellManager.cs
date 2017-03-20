using GTIWebAPI.Novell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Tests.Novell
{
    public class TestNovellManager : INovellManager
    {
        public bool CreateOrganization(INovellOrganizationContactPerson person)
        {
            throw new NotImplementedException();
        }

        public bool CredentialsCorrect(string username, string password)
        {
            throw new NotImplementedException();
        }

        public string FindEmail(string username)
        {
            throw new NotImplementedException();
        }

        public string GenerateLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
