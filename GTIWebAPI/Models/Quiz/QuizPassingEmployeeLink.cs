namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using GTIWebAPI.Models.Employees;


    [Table("QuizPassingEmployeeLink")]
    public class QuizPassingEmployeeLink
    {
        public int Id { get; set; }

        public int? PassingId { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual QuizPassing Passing { get; set; }
    }
}