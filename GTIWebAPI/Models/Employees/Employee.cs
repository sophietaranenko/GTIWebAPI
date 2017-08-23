namespace GTIWebAPI.Models.Employees
{
    using Dictionary;
    using Tasks;
    using Notifications;
    using Sales;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            EmployeePassports = new HashSet<EmployeePassport>();
            EmployeeOffices = new HashSet<EmployeeOffice>();
            EmployeeMilitaryCards = new HashSet<EmployeeMilitaryCard>();
            EmployeeLanguages = new HashSet<EmployeeLanguage>();
            EmployeeInternationalPassports = new HashSet<EmployeeInternationalPassport>();
            EmployeeGuns = new HashSet<EmployeeGun>();
            EmployeeFoundationDocuments = new HashSet<EmployeeFoundationDocument>();
            EmployeeEducations = new HashSet<EmployeeEducation>();
            EmployeeDrivingLicenses = new HashSet<EmployeeDrivingLicense>();
            EmployeeContacts = new HashSet<EmployeeContact>();
            EmployeeCars = new HashSet<EmployeeCar>();
            InteractionActMembers = new HashSet<InteractionActMember>();
            InteractionMembers = new HashSet<InteractionMember>();
            Notifications = new HashSet<Notification>();
            NotificationRecipients = new HashSet<NotificationRecipient>();
            CreatorTasks = new HashSet<Task>();
            DoerTasks = new HashSet<Task>();
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
            get
            {
                return new Age(DateOfBirth);
            }
        }

        [NotMapped]
        public string ProfilePicture { get; set; }

        [NotMapped]
        public string FullUserName { get; set; }

        public string AspNetUserId { get; set; }
 
        public virtual ICollection<EmployeeOffice> EmployeeOffices { get; set; }

        public virtual ICollection<EmployeePassport> EmployeePassports { get; set; }

        public virtual ICollection<EmployeeMilitaryCard> EmployeeMilitaryCards { get; set; }

        public virtual ICollection<EmployeeLanguage> EmployeeLanguages { get; set; }

        public virtual ICollection<EmployeeInternationalPassport> EmployeeInternationalPassports { get; set; }

        public virtual ICollection<EmployeeGun> EmployeeGuns { get; set; }

        public virtual ICollection<EmployeeFoundationDocument> EmployeeFoundationDocuments { get; set; }

        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }

        public virtual ICollection<EmployeeDrivingLicense> EmployeeDrivingLicenses { get; set; }

        public virtual ICollection<EmployeeContact> EmployeeContacts { get; set; }

        public virtual ICollection<EmployeeCar> EmployeeCars { get; set; }

        public virtual ICollection<InteractionActMember> InteractionActMembers { get; set; }

        public virtual ICollection<InteractionMember> InteractionMembers { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> CreatorTasks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> DoerTasks { get; set; }

        public int? EmployeeInsuranceId { get; set; }

        public virtual EmployeeInsurance EmployeeInsurance { get; set; }

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
                EmployeeInsurance = this.EmployeeInsurance == null ? null : this.EmployeeInsurance.ToDTO(),
                EmployeeInsuranceId = this.EmployeeInsuranceId,
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
                Address = this.Address == null ? null : this.Address.ToDTO(),
                EmployeeInsurance = this.EmployeeInsurance == null ? null : this.EmployeeInsurance.ToDTO(),
                EmployeeInsuranceId = this.EmployeeInsuranceId
            };
            return dto;
        }

        public EmployeeShortForm ToShortForm()
        {
            EmployeePassport passport = this.EmployeePassports.OrderByDescending(d => d.IssuedWhen).FirstOrDefault();
            EmployeeShortForm form = new EmployeeShortForm() { Id = this.Id, AspNetUserId = this.AspNetUserId};
            if (passport != null)
            {
                form.FirstName = passport.FirstName;
                form.SecondName = passport.SecondName;
                form.Surname = passport.Surname;
            }
            return form;
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

        public string ProfilePicture { get; set; }

        public string FullUserName { get; set; }

        public int? EmployeeInsuranceId { get; set; }

        public EmployeeInsuranceDTO EmployeeInsurance { get; set; }

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

        public int? EmployeeInsuranceId { get; set; }

        public EmployeeInsuranceDTO EmployeeInsurance { get; set; }

        public Employee FromDTO()
        {
            return new Employee()
            {
                Address = this.Address.FromDTO(),
                AddressId = this.AddressId,
                DateOfBirth = this.DateOfBirth,
                Id = this.Id,
                IdentityCode = this.IdentityCode,
                Sex = this.Sex,
                EmployeeInsurance = this.EmployeeInsurance == null ? null : this.EmployeeInsurance.FromDTO(),
                EmployeeInsuranceId = this.EmployeeInsuranceId
            };
        }

    }


    public interface INotificationAuthor
    {
        int Id { get; set; }

        string FirstName { get; set; }

        string SecondName { get; set; }

        string Surname { get; set; }

        string AspNetUserId { get; set; }

    }


    public class EmployeeShortForm : INotificationAuthor 
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Surname { get; set; }

        public string AspNetUserId { get; set; }

    }
}
