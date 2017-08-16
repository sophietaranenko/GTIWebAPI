namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeDrivingLicense")]
    public partial class EmployeeDrivingLicense : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        [StringLength(25)]
        public string Number { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IssuedWhen { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [StringLength(250)]
        public string IssuedBy { get; set; }

        /// <summary>
        /// Category of license recieved 
        /// </summary>
        [StringLength(10)]
        public string Category { get; set; }

        public bool? Deleted { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeDrivingLicense";
            }
        }

        public EmployeeDrivingLicenseDTO ToDTO()
        {
            EmployeeDrivingLicenseDTO dto = new EmployeeDrivingLicenseDTO
            {
                Category = this.Category,
                EmployeeId = this.EmployeeId,
                ExpiryDate = this.ExpiryDate,
                Id = this.Id,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Number = this.Number,
                Seria = this.Seria
            };
            return dto;
        }
    }

    public class EmployeeDrivingLicenseDTO
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public string Number { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Seria { get; set; }

        public string IssuedBy { get; set; }

        public string Category { get; set; }

        public EmployeeDrivingLicense FromDTO()
        {
            return new EmployeeDrivingLicense()
            {
                Category = this.Category,
                EmployeeId = this.EmployeeId,
                ExpiryDate = this.ExpiryDate,
                Id = this.Id,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Number = this.Number,
                Seria = this.Seria
            };
        }

    }
}
