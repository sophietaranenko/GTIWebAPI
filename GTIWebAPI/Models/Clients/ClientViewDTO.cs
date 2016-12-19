using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientViewDTO
    {
        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string IdentityCode { get; set; }

        public string Creator { get; set; }

        public string UserName { get; set; }

        public string ProfilePicture { get; set; }

        public Int32 Id { get; set; }
        
        public Int32 CreatorId { get; set; }
    }
}
