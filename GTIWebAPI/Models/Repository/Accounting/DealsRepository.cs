using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public class DealsRepository : IDealsRepository
    {
        private IDbContextFactory factory;

        public DealsRepository()
        {
            factory = new DbContextFactory();
        }

        public DealsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public DealFullViewDTO Get(Guid id)
        {
            DealFullViewDTO dto = new DealFullViewDTO();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                dto = db.GetDealCardInfo(id);
                if (dto == null)
                {
                    throw new ArgumentException();
                }
                dto.Containers = db.GetContainersByDeal(id);
                dto.Invoices = db.GetInvoicesByDeal(id);
            }
            return dto;
        }

        public List<DealViewDTO> GetAll(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            List<DealViewDTO> list = new List<DealViewDTO>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.GetDealsFiltered(organizationId, dateBegin, dateEnd).ToList();
            }
            return list;
        }
    }
}
