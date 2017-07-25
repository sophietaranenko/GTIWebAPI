using GTIWebAPI.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovelleDirectory
{
    public interface INovelleDirectory
    { 
        NovellUser Connect(string login, string password);

        INovellOrganizationContactPerson CreateOrganization(INovellOrganizationContactPerson person);
    }
}
