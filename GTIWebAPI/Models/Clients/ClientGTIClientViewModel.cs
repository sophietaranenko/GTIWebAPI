using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientGTIClientViewModel
    {
        public int Id { get; set; }
        
        public Client Client { get; set; }
        public ClientGTI ClientGTI { get; set; }
    }
}
