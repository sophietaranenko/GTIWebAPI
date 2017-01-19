using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// class for results of procedure EmployeeDocumentScanValid 
    /// </summary>
    public class EmployeeDocumentScanDTO
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public string TableName { get; set; }

        public string ScanName { get; set; }
    }
}
