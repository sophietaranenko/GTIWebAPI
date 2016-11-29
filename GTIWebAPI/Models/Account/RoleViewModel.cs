using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Account
{
    public class CreateRoleModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EditRoleModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
    }
}
