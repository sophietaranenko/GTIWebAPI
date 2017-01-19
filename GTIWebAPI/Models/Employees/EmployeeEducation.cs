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

        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Seria of diploma
        /// </summary>
        [StringLength(5)]
        public string Seria { get; set; }

        /// <summary>
        /// Number of diploma
        /// </summary>
        [StringLength(25)]
        public string Number { get; set; }

        /// <summary>
        /// Year of education end
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Form of study
        /// </summary>
        public int? StudyFormId { get; set; }

        public virtual EducationStudyForm EducationStudyForm { get; set; }

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

        public EmployeeEducationDTO ToDTO()
        {
            EmployeeEducationDTO dto = new EmployeeEducationDTO
            {
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                Institution = this.Institution,
                Number = this.Number,
                Qualification = this.Qualification,
                Seria = this.Seria,
                Specialty = this.Specialty,
                EducationStudyForm = this.EducationStudyForm == null ? null : new EducationStudyFormDTO
                {
                    Id = this.EducationStudyForm.Id,
                    Name = this.EducationStudyForm.Name
                },
                StudyFormId = this.StudyFormId,
                Year = this.Year
            };
            return dto;
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
