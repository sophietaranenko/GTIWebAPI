using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public class InvoicesRepository : IInvoicesRepository
    {
        private IDbContextFactory factory;

        public InvoicesRepository()
        {
            factory = new DbContextFactory();
        }

        public InvoicesRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public InvoiceFullViewDTO Get(int id)
        {
            InvoiceFullViewDTO dto = new InvoiceFullViewDTO();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                dto = db.GetInvoiceCardInfo(id);
                if (dto != null)
                {
                    dto.Containers = db.GetContainersByInvoiceId(id);
                    dto.Lines = db.GetInvoiceLinesByInvoice(id);
                }
            }
            return dto;
        }

        public List<DealInvoiceViewDTO> GetAll(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            List<DealInvoiceViewDTO> list = new List<DealInvoiceViewDTO>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.GetInvoicesList(organizationId, dateBegin, dateEnd).ToList();
            }
            return list;
        }
    }
}
