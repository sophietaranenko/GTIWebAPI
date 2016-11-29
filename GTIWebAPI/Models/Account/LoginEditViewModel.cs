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
       // [Display(Name="Имя пользователя")]
        public string Name { get; set; }
       // [Display(Name="Роли пользователя")]
        public IList<string> Roles { get; set; }
    }
}
