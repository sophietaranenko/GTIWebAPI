﻿namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Security;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    [Table("Employee")]
    public partial class Employee : GTITable
    {
        public Employee()
        {
            Masks = new HashSet<UserRightMask>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public short? Sex { get; set; }

        [Column("IdentityCode")]
        [StringLength(20)]
        public string IdentityCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        public Address Address { get; set; }

        public int? AddressId { get; set; }

        public bool? Deleted { get; set; }

        public Age Age
        {
            get { return new Age(DateOfBirth); }
        }

        /// <summary>
        /// Does not update
        /// </summary>
        [NotMapped]
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Does not update
        /// </summary>
        [NotMapped]
        public string FullUserName { get; set; }

        public virtual IEnumerable<EmployeeOffice> EmployeeOffices { get; set; }

        public virtual IEnumerable<EmployeePassport> EmployeePassports { get; set; }

        public virtual IEnumerable<EmployeeMilitaryCard> EmployeeMilitaryCards { get; set; }

        public virtual IEnumerable<EmployeeLanguage> EmployeeLanguages { get; set; }

        public virtual IEnumerable<EmployeeInternationalPassport> EmployeeInternationalPassports { get; set; }

        public virtual IEnumerable<EmployeeGun> EmployeeGuns { get; set; }

        public virtual IEnumerable<EmployeeFoundationDocument> EmployeeFoundationDocuments { get; set; }

        public virtual IEnumerable<EmployeeEducation> EmployeeEducations { get; set; }

        public virtual IEnumerable<EmployeeDrivingLicense> EmployeeDrivingLicenses { get; set; }

        public virtual IEnumerable<EmployeeContact> EmployeeContacts { get; set; }

        public virtual IEnumerable<EmployeeCar> EmployeeCars { get; set; }

        public virtual IEnumerable<UserRightMask> Masks { get; set; }

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
                Age = this.Age.ToString(),
                EmployeeCars = this.EmployeeCars == null ? null : this.EmployeeCars.Select(d => d.ToDTO()).ToList(),
                EmployeePassports = this.EmployeePassports == null ? null : this.EmployeePassports.Select(d => d.ToDTO()).ToList(),
                EmployeeOffices = this.EmployeeOffices == null ? null : this.EmployeeOffices.Select(d => d.ToDTO()).ToList(),
                EmployeeMilitaryCards = this.EmployeeMilitaryCards == null ? null : this.EmployeeMilitaryCards.Select(d => d.ToDTO()).ToList(),
                EmployeeLanguages = this.EmployeeLanguages == null ? null : this.EmployeeLanguages.Select(d => d.ToDTO()).ToList(),
                EmployeeInternationalPassports = this.EmployeeInternationalPassports == null ? null : this.EmployeeInternationalPassports.Select(d => d.ToDTO()).ToList(),
                EmployeeGuns = this.EmployeeGuns == null ? null : this.EmployeeGuns.Select(d => d.ToDTO()).ToList(),
                EmployeeFoundationDocuments = this.EmployeeFoundationDocuments == null ? null : this.EmployeeFoundationDocuments.Select(d => d.ToDTO()).ToList(),
                EmployeeEducations = this.EmployeeEducations == null ? null : this.EmployeeEducations.Select(d => d.ToDTO()).ToList(),
                EmployeeDrivingLicenses = this.EmployeeDrivingLicenses == null ? null : this.EmployeeDrivingLicenses.Select(d => d.ToDTO()).ToList(),
                EmployeeContacts = this.EmployeeContacts == null ? null : this.EmployeeContacts.Select(d => d.ToDTO()).ToList(),
                ProfilePicture = this.ProfilePicture,
                FullUserName = this.FullUserName
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
        public int Id { get; set; }

        public short? Sex { get; set; }

        public string IdentityCode { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

        public string Age { get; set; }

        /// <summary>
        /// Does not update
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Does not update
        /// </summary>
        public string FullUserName { get; set; }

        public IEnumerable<EmployeePassportDTO> EmployeePassports { get; set; }

        public IEnumerable<EmployeeContactDTO> EmployeeContacts { get; set; }

        public IEnumerable<EmployeeOfficeDTO> EmployeeOffices { get; set; }

        public IEnumerable<EmployeeEducationDTO> EmployeeEducations { get; set; }

        public IEnumerable<EmployeeLanguageDTO> EmployeeLanguages { get; set; }

        public IEnumerable<EmployeeFoundationDocumentDTO> EmployeeFoundationDocuments { get; set; }

        public IEnumerable<EmployeeInternationalPassportDTO> EmployeeInternationalPassports { get; set; }

        public IEnumerable<EmployeeCarDTO> EmployeeCars { get; set; }

        public IEnumerable<EmployeeDrivingLicenseDTO> EmployeeDrivingLicenses { get; set; }

        public IEnumerable<EmployeeGunDTO> EmployeeGuns { get; set; }

        public IEnumerable<EmployeeMilitaryCardDTO> EmployeeMilitaryCards { get; set; }
    }

    public class EmployeeEditDTO
    {
        public int Id { get; set; }

        public short? Sex { get; set; }

        public string IdentityCode { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

    }
}
