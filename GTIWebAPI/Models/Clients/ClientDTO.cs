using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string IdentityCode { get; set; }

        public int? AddressPhysicalId { get; set; }

        public AddressDTO AddressPhysical { get; set; }

        public int? AddressLegalId { get; set; }

        public AddressDTO AddressLegal { get; set; }

        public bool? Deleted { get; set; }

        public string UserId { get; set; }

        public int EmployeeId { get; set; }

        public List<ClientContactDTO> ClientContact { get; set; }
    }
}
