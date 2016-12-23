using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// Class for current Employee View 
    /// </summary>
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
        /// string UserId foreign key to AspNetUsers  
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// AddressDTO of employee address of permanent residense 
        /// </summary>
        public AddressDTO Address { get; set; }

        /// <summary>
        /// Cropped profile picture
        /// </summary>
        public byte[] ProfilePicture { get; set; }

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
        public IEnumerable<EmployeeFoundationDocDTO> EmployeeFoundationDoc { get; set; }

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
}