using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeContactDTO
    {
        public int Id { get; set; }

        public int? Type { get; set; }

        public string Value { get; set; }

        public int? EmployeeId { get; set; }

        public ContactTypeDTO ContactType { get; set; }
    }
}