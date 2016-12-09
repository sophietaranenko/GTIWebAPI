using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeLanguageDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }
        public int? LanguageId { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public int? Type { get; set; }

        public string Definition { get; set; }

        public string Remark { get; set; }

        public LanguageDTO Language { get; set; }
    }
}