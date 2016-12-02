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
    public partial class Employee : IUserable
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
        [Column("IdentityCodeChar")]
        [StringLength(20)]
        public string IdentityCode { get; set; }

        /// <summary>
        /// Employee's date of birth
        /// </summary>
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Id of Employee's Address of permanent residence in Address table 
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// Deleted mark
        /// </summary>
        public bool? Deleted { get; set; }
        
        /// <summary>
        /// Connection to AspNetUsers
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Cropped profile picture of employee
        /// </summary>
        public byte[] ProfilePicture { get; set; }

        /// <summary>
        /// Age, counted from date of birth
        /// </summary>
        public Age Age
        {
            get { return new Age(DateOfBirth); }
        }

        /// <summary>
        /// TableName (for service, part of interface IUserable)
        /// </summary>
        public string TableName
        {
            get
            {
                return "Employee";
            }
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
}
