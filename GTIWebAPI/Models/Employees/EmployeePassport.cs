namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeePassport")]
    public partial class EmployeePassport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? Number { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedWhen { get; set; }

        public int? AddressId { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string SecondName { get; set; }

        [StringLength(20)]
        public string Surname { get; set; }

        [StringLength(100)]
        public string IssuedBy { get; set; }
        public bool? Deleted { get; set; }

        public virtual Address Address { get; set; }
        public virtual Employee Employee { get; set; }

        public string ShortName
        {
            get
            {
                return
                    FirstName.Substring(1, 1) + ". " +
                        SecondName.Substring(1, 1) + ". " +
                        Surname;
            }
        }
    }
}
