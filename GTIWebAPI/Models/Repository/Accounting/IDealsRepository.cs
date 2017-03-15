using GTIWebAPI.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public interface IDealsRepository
    {
        List<DealViewDTO> GetAll(int organizationId, DateTime dateBegin, DateTime dateEnd);

        DealFullViewDTO Get(Guid id);
    }
}
