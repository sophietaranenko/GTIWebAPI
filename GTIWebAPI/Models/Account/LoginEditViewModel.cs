using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Account 
{
    public class LoginEditViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<string> Roles { get; set; }
    }
}
