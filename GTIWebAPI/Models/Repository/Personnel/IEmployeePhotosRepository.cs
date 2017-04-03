using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.Models.Repository
{
    public interface IEmployeePhotosRepository
    {
        EmployeePhoto Add(EmployeePhoto photo);

        string SaveFile(HttpPostedFile postedFile);

        //List<EmployeePhoto> PutDbFilesToFilesystem();

        List<EmployeePhoto> GetByEmployeeId(int employeeId);

        EmployeePhoto Get(int id);

        EmployeePhoto Delete(int id);

        //EmployeePhoto RenameFile(int id);
    }
}
