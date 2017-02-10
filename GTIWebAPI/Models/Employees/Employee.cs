namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Class for Employee table
    /// </summary>
    [Table("Employee")]
    public partial class Employee : GTITable
    {
        /// <summary>
        /// Id of Employee
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// short value of Employee Sex (can be 1 or 2) 
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// Employee identity code
        /// </summary>
        [Column("IdentityCode")]
        [StringLength(20)]
        public string IdentityCode { get; set; }

        /// <summary>
        /// Employee's date of birth
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        public Address Address { get; set; }

        /// <summary>
        /// Id of Employee's Address of permanent residence in Address table 
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// Deleted mark
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        /// Age, counted from date of birth
        /// </summary>
        public Age Age
        {
            get { return new Age(DateOfBirth); }
        }

        protected override string TableName
        {
            get
            {
                return "Employee";
            }
        }

        public EmployeeDTO ToDTOView()
        {
            EmployeeDTO dto = new EmployeeDTO
            {
                Sex = this.Sex,
                AddressId = this.AddressId,
                DateOfBirth = this.DateOfBirth,
                Id = this.Id,
                IdentityCode = this.IdentityCode,
                Address = this.Address == null ? null : this.Address.ToDTO(),
                Age = this.Age.ToString()
            };
            return dto;
        }


        public EmployeeEditDTO ToDTOEdit()
        {
            EmployeeEditDTO dto = new EmployeeEditDTO
            {
                Sex = this.Sex,
                AddressId = this.AddressId,
                DateOfBirth = this.DateOfBirth,
                Id = this.Id,
                IdentityCode = this.IdentityCode,
                Address = this.Address == null ? null : this.Address.ToDTO()
            };
            return dto;
        }

        /// <summary>
        /// String value of Employee Sex
        /// </summary>
        public string SexString
        {
            get
            {
                if (Sex != null)
                {
                    return Enum.GetName(typeof(Sex), Sex);
                }
                return "";
            }
        } 
    }

    public class EmployeeDTO
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee Sex in int
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// Employee identity code
        /// </summary>
        public string IdentityCode { get; set; }

        /// <summary>
        /// Employee date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Employee address of permanent residense Id from Address table 
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// AddressDTO of employee address of permanent residense 
        /// </summary>
        public AddressDTO Address { get; set; }

        /// <summary>
        /// Employee age in string
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// Collection of employee Passports
        /// </summary>
        public IEnumerable<EmployeePassportDTO> EmployeePassport { get; set; }

        /// <summary>
        /// Collection of employee contacts
        /// </summary>
        public IEnumerable<EmployeeContactDTO> EmployeeContact { get; set; }

        /// <summary>
        /// Collection of employee positions
        /// </summary>
        public IEnumerable<EmployeeOfficeDTO> EmployeeOffice { get; set; }

        /// <summary>
        /// Collection of employee educations
        /// </summary>
        public IEnumerable<EmployeeEducationDTO> EmployeeEducation { get; set; }

        /// <summary>
        /// Collection of employee languages
        /// </summary>
        public IEnumerable<EmployeeLanguageDTO> EmployeeLanguage { get; set; }

        /// <summary>
        /// Collection of employee foundation documents 
        /// </summary>
        public IEnumerable<EmployeeFoundationDocumentDTO> EmployeeFoundationDoc { get; set; }

        /// <summary>
        /// Collection of employee international passports
        /// </summary>
        public IEnumerable<EmployeeInternationalPassportDTO> EmployeeInternationalPassport { get; set; }

        /// <summary>
        /// Collection of employee cars
        /// </summary>
        public IEnumerable<EmployeeCarDTO> EmployeeCar { get; set; }

        /// <summary>
        /// Collection of employee driving licenses
        /// </summary>
        public IEnumerable<EmployeeDrivingLicenseDTO> EmployeeDrivingLicense { get; set; }

        /// <summary>
        /// Collection of employee gun store permissions
        /// </summary>
        public IEnumerable<EmployeeGunDTO> EmployeeGun { get; set; }

        /// <summary>
        /// Collection of employee military cards
        /// </summary>
        public IEnumerable<EmployeeMilitaryCardDTO> EmployeeMilitaryCard { get; set; }
    }

    public class EmployeeEditDTO
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee Sex in Int
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// Employee identity code
        /// </summary>
        public string IdentityCode { get; set; }

        /// <summary>
        /// Employee date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Employee Address Id
        /// </summary>
        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

    }
}
