using GTIWebAPI.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public interface IContainersRepository
    {
        List<DealContainerViewDTO> GetAll(int organziationId, DateTime dateBegin, DateTime dateEnd);

        DealContainerViewDTO Get(Guid id);
    }
}
