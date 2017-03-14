using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.Models.Repository
{
    public interface IEmployeeDocumentScansRepository
    {
        EmployeeDocumentScan Add(EmployeeDocumentScan scan);

        string SaveFile(HttpPostedFile file);

        EmployeeDocumentScan Delete(int id);

        EmployeeDocumentScan Get(int id);

        List<EmployeeDocumentScan> GetByDocumentId(string tableName, int tableId);

        List<EmployeeDocumentScan> GetByEmployeeId(int employeeId);

        List<EmployeeDocumentScan> FromByteArrayToString();


    }
}
