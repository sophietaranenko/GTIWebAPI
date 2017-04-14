using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Organization
{
    public interface IOrganizationRepository<T> where T : class
    {
        List<T> GetByOrganizationId(int organizationId);

        T Get(int id);

        T Add(T item);

        T Edit(T item);

        T Delete(int id);
    }
}
