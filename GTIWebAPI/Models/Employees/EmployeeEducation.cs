namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Education of employee
    /// </summary>
    [Table("EmployeeEducation")]
    public partial class EmployeeEducation : GTITable
    {
        /// <summary>
        /// Education Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Employee id
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Seria of diploma
        /// </summary>
        [StringLength(5)]
        public string Seria { get; set; }

        /// <summary>
        /// Number of diploma
        /// </summary>
        [Column("NumberChar")]
        [StringLength(25)]
        public string Number { get; set; }

        /// <summary>
        /// Year of education end
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Form of study
        /// </summary>
        public int? StudyForm { get; set; }

        /// <summary>
        /// Education institution name 
        /// </summary>
        [StringLength(250)]
        public string Institution { get; set; }

        /// <summary>
        /// Specialty name
        /// </summary>
        [StringLength(250)]
        public string Specialty { get; set; }

        /// <summary>
        /// Qualification name
        /// </summary>
        [StringLength(150)]
        public string Qualification { get; set; }

        /// <summary>
        /// Delete mark 
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        /// String value for StudyForm
        /// </summary>
        public string StudyFormString
        {
            get
            {
                string result = "";
                if (StudyForm != null)
                {
                    result = Enum.GetName(typeof(FormStudy), StudyForm);
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        protected override string TableName
        {
            get
            {
                return "EmployeeEducation";
            }
        }
    }
}
