using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class InvoiceLineViewDTO
    {
        public string ServiceType { get; set; }

        public string ServiceName { get; set; }

        public decimal? ServiceSum { get; set; }

        public decimal? UsdSum { get; set; }

        public decimal? CurrencySum { get; set; }

        public int? VatId { get; set; }

        public string VatInPersent { get; set; }

        public decimal? VatSum { get; set; }

        public int? ServiceTypeId { get; set; }

        public Guid Id { get; set; }

        public int? LinePosition { get; set; }

        public int? InvoiceId { get; set; }
    }
}
