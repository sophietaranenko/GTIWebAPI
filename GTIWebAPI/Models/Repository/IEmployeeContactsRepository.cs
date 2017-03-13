using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public interface IEmployeeContactsRepository
    {
        List<EmployeeContact> GetAll();

        List<EmployeeContact> GetByEmployeeId(int employeeId);

        EmployeeContact Get(int id);

        EmployeeContact Add(EmployeeContact contact);

        EmployeeContact Edit(EmployeeContact contact);

        EmployeeContact Delete(int id);
    }
}
