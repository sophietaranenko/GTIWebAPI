using GTIWebAPI.NovelleDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Tests.Novell
{
    public class TestNovellManager : INovelleDirectory
    {
        INovellOrganizationContactPerson INovelleDirectory.CreateOrganization(INovellOrganizationContactPerson person)
        {
            throw new NotImplementedException();
        }

        public NovellUser Connect(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
