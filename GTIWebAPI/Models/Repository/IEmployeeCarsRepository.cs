using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public interface IEmployeeCarsRepository
    {
        List<EmployeeCar> GetAll();

        List<EmployeeCar> GetByEmployeeId(int employeeId);

        EmployeeCar Get(int id);

        EmployeeCar Add(EmployeeCar car);

        EmployeeCar Edit(EmployeeCar car);

        EmployeeCar Delete(int carId);
    }
}
