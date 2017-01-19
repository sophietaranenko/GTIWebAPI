using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeLanguageTypeDTO
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string Name { get; set; }
    }
}
