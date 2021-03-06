namespace GTIWebAPI.Models.Personnel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Department")]
    public partial class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public bool? Deleted { get; set; }

        public DepartmentDTO ToDTO()
        {
            DepartmentDTO dto = new DepartmentDTO
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }

    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
