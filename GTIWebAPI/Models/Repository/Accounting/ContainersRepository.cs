using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public class ContainersRepository : IContainersRepository
    {
        private IDbContextFactory factory;

        public ContainersRepository()
        {
            factory = new DbContextFactory();
        }

        public ContainersRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public DealContainerViewDTO Get(Guid id)
        {
            DealContainerViewDTO container = new DealContainerViewDTO();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                container = db.GetContainer(id);
            }
            if (container == null)
            {
                throw new NotFoundException();
            }
            return container;
        }

        public List<DealContainerViewDTO> GetAll(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            List<DealContainerViewDTO> list = new List<DealContainerViewDTO>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.GetContainersFiltered(organizationId, dateBegin, dateEnd).ToList();
            }
            if (list == null)
            {
                throw new NotFoundException();
            }
            return list;
        }
    }
}
