using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public interface IInvoicesRepository
    {
        List<DealInvoiceViewDTO> GetAll(int organizationId, DateTime dateBegin, DateTime dateEnd);

        InvoiceFullViewDTO Get(int id);

    }
}
