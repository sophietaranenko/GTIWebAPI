using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationGTIDTO
    {
        public int Id { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string RegistrationNumber { get; set; }

        public string TaxNumber { get; set; }

        public int OfficeId { get; set; }

        public OfficeDTO Office { get; set; }
    }
}
