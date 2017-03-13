using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public interface IEmployeePassportsRepository
    {
        List<EmployeePassport> GetAll();

        List<EmployeePassport> GetByEmployeeId(int employeeId);

        EmployeePassport Get(int id);

        EmployeePassport Add(EmployeePassport passport);

        EmployeePassport Edit(EmployeePassport passport);

        EmployeePassport Delete(int id);
    }
}
