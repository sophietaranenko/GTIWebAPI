using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public string RussianName { get; set; }

        public string ShortName { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public int? OrganizationLegalFormId { get; set; }

        public OrganizationLegalFormDTO OrganizationLegalForm { get; set; }

        public IEnumerable<OrganizationAddressDTO> OrganizationAddresses { get; set; }

        public IEnumerable<OrganizationContactPersonDTO> OrganizationContactPersons { get; set; }

        public IEnumerable<OrganizationGTILinkDTO> OrganizationGTILinks { get; set; }

        public IEnumerable<OrganizationPropertyDTO> OrganizationProperties { get; set; }

        public IEnumerable<OrganizationTaxAddressDTO> OrganizationTaxAddresses { get; set; } 

    }
}
