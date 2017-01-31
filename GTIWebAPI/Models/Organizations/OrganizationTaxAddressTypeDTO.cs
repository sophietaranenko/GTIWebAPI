using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationTaxAddressTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CountryId { get; set; }

        public CountryDTO Country { get; set; }

    }
}
