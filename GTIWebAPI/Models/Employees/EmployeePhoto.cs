namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class for employee photo 
    /// </summary>
    [Table("EmployeePhoto")]
    public partial class EmployeePhoto : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        //[Column(TypeName = "image")]
        //public byte[] Photo { get; set; }

        [StringLength(500)]
        public string PhotoName { get; set; }

        public bool? Deleted { get; set; }

        public bool? ProfilePicture { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeePhoto";
            }
        }
    }
}
