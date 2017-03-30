using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Novell
{
    public interface INovellManager
    {
        string GenerateLogin(string login);

        bool CredentialsCorrect(string username, string password);

        bool CreateOrganization(INovellOrganizationContactPerson person);

        string FindEmail(string username);

        string FindOffice(string username);
    }
}
