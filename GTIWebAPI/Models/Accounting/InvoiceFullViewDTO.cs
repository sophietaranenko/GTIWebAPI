using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class InvoiceFullViewDTO
    {

        public IEnumerable<InvoiceContainerViewDTO> Containers { get; set; }
        public IEnumerable<InvoiceLineViewDTO> Lines { get; set; }
    }
}
