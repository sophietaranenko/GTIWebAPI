using GTIWebAPI.NovelleDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IPrincipalProvider
    {
        string GetUserName();

        bool IsInRole(string roleName);

        string GetPostOffice();

        string GetSessionId();
    }
}
