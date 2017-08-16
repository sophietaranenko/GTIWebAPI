namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeGun")]
    public partial class EmployeeGun : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [StringLength(25)]
        public string Number { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string IssuedBy { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedWhen { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeGun";
            }
        }

        public EmployeeGunDTO ToDTO()
        {
            EmployeeGunDTO dto = new EmployeeGunDTO
            {
                Id = this.Id,
                DateEnd = this.DateEnd,
                Description = this.Description,
                EmployeeId = this.EmployeeId,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Number = this.Number,
                Seria = this.Seria
            };
            return dto;
        }
    }

    public class EmployeeGunDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string Seria { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public string IssuedBy { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public DateTime? DateEnd { get; set; }

        public EmployeeGun FromDTO()
        {
            return new EmployeeGun()
            {
                DateEnd = this.DateEnd,
                Description = this.Description,
                EmployeeId = this.EmployeeId,
                Seria = this.Seria,
                Id = this.Id,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Number = this.Number
            };
        }
    }
}
