using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    [Table("EducationStudyForm")]
    public partial class EducationStudyForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EducationStudyForm()
        {
            EmployeeEducations = new HashSet<EmployeeEducation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }

        public EducationStudyFormDTO ToDTO()
        {
            EducationStudyFormDTO dto = new EducationStudyFormDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }

    public class EducationStudyFormDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
