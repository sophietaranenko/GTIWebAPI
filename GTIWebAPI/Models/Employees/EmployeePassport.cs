namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Service;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class for Employee Passport from table EmployeePassport
    /// </summary>
    [Table("EmployeePassport")]
    public partial class EmployeePassport : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedWhen { get; set; }

        [StringLength(250)]
        public string IssuedBy { get; set; }

        public int? AddressId { get; set; }
        
        public virtual Address Address { get; set; }

        public virtual Employee Employee { get; set; }

        public bool? Deleted { get; set; }

        public string ShortName
        {
            get
            {
                string res = "";
                if (FirstName != null && SecondName != null && Surname != null)
                {
                    if (FirstName.Length > 3 && SecondName.Length > 3)
                    {
                        res = FirstName.Substring(0, 1) + ". " +
                            SecondName.Substring(0, 1) + ". " +
                            Surname;
                    }
                    else
                    {
                        res = "";
                    }
                }
                else
                {
                    res = "";
                }
                return res;   
            }
        }

        public EmployeePassportDTO ToDTO()
        {
            EmployeePassportDTO dto =
                new EmployeePassportDTO
                {
                    SecondName = this.SecondName,
                    Seria = this.Seria,
                    ShortName = this.ShortName,
                    Surname = this.Surname,
                    Address = this.Address == null ? null : this.Address.ToDTO(),
                    AddressId = this.AddressId,
                    EmployeeId = this.EmployeeId,
                    FirstName = this.FirstName,
                    Id = this.Id,
                    IssuedBy = this.IssuedBy,
                    IssuedWhen = this.IssuedWhen,
                    Number = this.Number
                };
            return dto;
        }

        protected override string TableName
        {
            get
            {
                return "EmployeePassport";
            }
        }
    }

    public class EmployeePassportDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string Seria { get; set; }

        public string Number { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Surname { get; set; }

        public string ShortName { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public string IssuedBy { get; set; }

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

        public EmployeePassport FromDTO()
        {
            return new EmployeePassport()
            {
                AddressId = this.AddressId,
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                FirstName = this.FirstName,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Number = this.Number,
                SecondName = this.SecondName,
                Seria = this.Seria,
                Surname = this.Surname,
                Address = this.Address == null ? null : this.Address.FromDTO()
            };
        }
    }
}
