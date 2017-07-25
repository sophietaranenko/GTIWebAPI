using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovelleDirectory
{
    public interface INovellOrganizationContactPerson
    {
        string Login { get; set; }

        string Password { get; set; }

        string FirstName { get; set; }

        string Surname { get; set; }

        string Email { get; set; }
    }
}
