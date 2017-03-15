using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public interface IEmployeesRepository 
    {
        List<EmployeeView> GetAll(List<int> officeIds);

        Employee GetView(int id);

        Employee GetEdit(int id);

        Employee Edit(Employee employee);

        Employee Add(Employee employee);

        Employee Delete(int id);

        EmployeeList GetList();
    }
}
