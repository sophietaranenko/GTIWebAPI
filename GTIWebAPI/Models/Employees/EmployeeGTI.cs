namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sotrud")]
    public partial class EmployeeGTI
    {
       /// <summary>
       /// Id of  GTI employee
       /// </summary>
        [Column("kod")]
        public int Id { get; set; }

        /// <summary>
        /// naimen of gti employee
        /// </summary>
        [Required]
        [StringLength(30)]
        [Column("naimen")]
        public string Name { get; set; }

        /// <summary>
        /// fio in russian
        /// </summary>
        [Required]
        [StringLength(60)]
        [Column("fio")]
        public string NativeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("office")]
        public int OfficeId { get; set; }

        /// <summary>
        /// integer code inside gti 
        /// </summary>
        [Column("prava")]
        public int GTIId { get; set; }

        /// <summary>
        /// key column in sotrud gti
        /// </summary>
        [Key]
        public Guid rc { get; set; }

        /// <summary>
        /// email part after @ (@mail.ru)
        /// </summary>
        [Required]
        [StringLength(50)]
        [Column("email")]
        public string EmailEnd { get; set; }

       /// <summary>
       /// email first part (before @)
       /// </summary>
        [Required]
        [StringLength(30)]
        [Column("naimen_e")]
        public string EmailBegin { get; set; }



        [NotMapped]
        public string Email
        {
            get
            {
                string result = "";
                result += EmailBegin != null ? EmailBegin.Trim() : "";
                result += EmailEnd != null ? EmailEnd.Trim() : "";
                return result;
            }
        }

    }
}
