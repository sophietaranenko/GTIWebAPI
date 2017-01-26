using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    /// <summary>
    /// Class for containers that invoice contains 
    /// </summary>
    public class InvoiceContainerViewDTO
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string BL { get; set; }

        public double BruttoWeight { get; set; }

        public int? BLId { get; set; }

        public Guid Id { get; set; }

        public int? InvoiceId { get; set; }
    }
}
