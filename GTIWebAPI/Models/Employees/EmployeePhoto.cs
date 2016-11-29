namespace GTIWebAPI.Models.Employees
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    [Table("EmployeePhoto")]
    public partial class EmployeePhoto 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [StringLength(50)]
        public string PhotoName { get; set; }
        public bool? Deleted { get; set; }
        public bool? ProfilePicture { get; set; }

        public virtual Employee Employee { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}
