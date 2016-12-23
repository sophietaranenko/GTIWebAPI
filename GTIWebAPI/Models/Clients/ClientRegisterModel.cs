using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientRegisterModel
    {
        public string ResetPasswordToken { get; set; }
        public string ConfirmEmailToken { get; set; }
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
