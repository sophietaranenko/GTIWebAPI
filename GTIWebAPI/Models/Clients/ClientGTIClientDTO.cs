using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientGTIClientDTO
    {
        public int Id { get; set; }

        public int? GTIClientId { get; set; }

        public int? ClientId { get; set; }

        public ClientGTIDTO ClientGTI { get; set; }


    }
}
