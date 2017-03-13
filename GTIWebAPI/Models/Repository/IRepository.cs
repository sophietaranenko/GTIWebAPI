using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();

        List<T> GetByEmployeeId(int employeeId);

        T Get(int id);

        T Add(T contact);

        T Edit(T contact);

        T Delete(int id);
    }
}
