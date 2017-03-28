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
        /// <summary>
        /// Passport Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Employee owner Id
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Passport Seria
        /// </summary>
        [StringLength(5)]
        public string Seria { get; set; }

        /// <summary>
        /// Number of passport
        /// </summary>
        [StringLength(25)]
        public string Number { get; set; }

        /// <summary>
        /// Employee first name specified in passport
        /// </summary>
        [StringLength(20)]
        public string FirstName { get; set; }

        /// <summary>
        /// Employee second name specified in passport
        /// </summary>
        [StringLength(20)]
        public string SecondName { get; set; }

        /// <summary>
        /// Employee surname specified in passport
        /// </summary>
        [StringLength(20)]
        public string Surname { get; set; }


        /// <summary>
        /// Date when passport was issued to employee 
        /// </summary>
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssuedWhen { get; set; }

        /// <summary>
        /// Government place where passport was issued to employee
        /// </summary>
        [StringLength(100)]
        public string IssuedBy { get; set; }

        /// <summary>
        /// Address of employee registration in passport
        /// </summary>
        public int? AddressId { get; set; }
        
        /// <summary>
        /// Address object
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Delete mark
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        /// Short name construcred from passport (С.Э. Тараненко) 
        /// </summary>
        public string ShortName
        {
            get
            {
                string res = "";
                if (FirstName != null && SecondName != null && Surname != null)
                {
                    res = FirstName.Substring(0, 1) + ". " +
                        SecondName.Substring(0, 1) + ". " +
                        Surname;
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
        /// <summary>
        /// EmployeePassport Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee owner Id
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Passport seria
        /// </summary>
        public string Seria { get; set; }

        /// <summary>
        /// Passport number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Employee First Name specified in Passport
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Employee second name specified in Passport
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Employee surname specified in Passport
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Short format of employee name "С.Э. Тараненко" 
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Date when passport was issued to Employee
        /// </summary>
        public DateTime? IssuedWhen { get; set; }

        /// <summary>
        /// Government organization that issued Passport to Employee
        /// </summary>
        public string IssuedBy { get; set; }

        /// <summary>
        /// Id of registration Id specified in Passport 
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// AddressDTO from registration Address
        /// </summary>
        public AddressDTO Address { get; set; }
    }
}
