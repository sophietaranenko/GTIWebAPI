 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// DTO for EmployeeEducation
    /// </summary>
    public class EmployeeEducationDTO
    {
        /// <summary>
        /// Education id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee id
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Education document seria
        /// </summary>
        public string Seria { get; set; }

        /// <summary>
        /// Education document number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Year of education end
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Study form
        /// </summary>
        public int? StudyForm { get; set; }

        
        /// <summary>
        /// Institution name 
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// Specialty name
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// Qualification
        /// </summary>
        public string Qualification { get; set; }

        /// <summary>
        /// String form of StudyForm
        /// </summary>
        public string StudyFormString { get; set; }
    }
}
