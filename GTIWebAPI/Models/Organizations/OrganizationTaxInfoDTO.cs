using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class OrganizationTaxInfoDTO
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public string TaxNo { get; set; }

        public string SertificatesNo { get; set; }

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

        public DateTime? DateBeg { get; set; }

        public DateTime? DateEnd { get; set; }

    }
}
