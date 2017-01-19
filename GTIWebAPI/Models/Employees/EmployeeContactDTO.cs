using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeContactDTO
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public int? ContactTypeId { get; set; }

        public ContactTypeDTO ContactType { get; set; }

        public string Value { get; set; }
        
    }
}