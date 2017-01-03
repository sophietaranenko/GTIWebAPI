using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientTaxInfoDTO
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public string TaxNo { get; set; }

        public string SertificatesNo { get; set; }

        public int? TaxAddressId { get; set; }

        public DateTime? DateBeg { get; set; }

        public DateTime? DateEnd { get; set; }

    }
}
