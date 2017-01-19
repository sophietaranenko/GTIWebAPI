using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class OrganizationEditDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public string RussianName { get; set; }

        public int? OrganizationTypeId { get; set; }

        public string IdentityCode { get; set; }

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public OrganizationTypeDTO OrganizationType { get; set; }
    }
}
