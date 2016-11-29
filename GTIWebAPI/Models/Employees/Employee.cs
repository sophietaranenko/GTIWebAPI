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

    [Table("Employee")]
    public partial class Employee : IUserable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            //EmployeeDrivingLicense = new HashSet<EmployeeDrivingLicense>();
            //EmployeeEducation = new HashSet<EmployeeEducation>();
            //EmployeeLanguage = new HashSet<EmployeeLanguage>();
            //EmployeeMilitaryCard = new HashSet<EmployeeMilitaryCard>();
            //EmployeePassport = new HashSet<EmployeePassport>();
            //EmployeePhoto = new HashSet<EmployeePhoto>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string userName;
        public int Id { get; set; }

        public short? Sex { get; set; }

        [Column("IdentityCodeChar")]
        [StringLength(20)]
        public string IdentityCode { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        public int? AddressId { get; set; }
        public bool? Deleted { get; set; }
        public string UserId { get; set; }

        public Address Address { get; set; }

        public byte[] ProfilePicture { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeeDrivingLicense> EmployeeDrivingLicense { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeeEducation> EmployeeEducation { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeeLanguage> EmployeeLanguage { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeeMilitaryCard> EmployeeMilitaryCard { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeePassport> EmployeePassport { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeePhoto> EmployeePhoto { get; set; }

        public Age Age
        {
            get { return new Age(DateOfBirth); }
        }

        public string TableName
        {
            get
            {
                return "Employee";
            }
        }

        //public string GetUserName()
        //{
        //    EmployeePassport passport = EmployeePassport.OrderByDescending(p => p.IssuedWhen).Take(1).FirstOrDefault();
        //    return passport.FirstName.Substring(0, 1) + ". " +
        //        passport.SecondName.Substring(0, 1) + ". " +
        //        passport.Surname;
        //}

        public SelectList GetSexList()
        {
            var SexList = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(SexList, "Value", "Text");
        }
    }
}
