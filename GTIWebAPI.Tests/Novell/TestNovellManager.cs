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
            return true;
        }

        public bool CredentialsCorrect(string username, string password)
        {
            return true;
        }

        public string FindEmail(string username)
        {
            return "testemail@testemail.test";
        }

        public string GenerateLogin(string login)
        {
            return login.Replace(" ", "");
        }
    }
}
