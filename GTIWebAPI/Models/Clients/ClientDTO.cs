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

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

        public int? OrganizationTypeId { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public bool? Deleted { get; set; }

        public string UserId { get; set; }

        public int EmployeeId { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public List<ClientContactDTO> ClientContact { get; set; }
        public List<ClientGTIClientDTO> ClientGTIClient { get; set; }

    }
}
