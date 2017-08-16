using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    [Table("EmployeeInsurance")]
    public partial class EmployeeInsurance
    {
        public EmployeeInsurance()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }

        [StringLength(30)]
        public string Number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        public EmployeeInsuranceDTO ToDTO()
        {
            return new EmployeeInsuranceDTO()
            {
                Id = this.Id,
                Number = this.Number
            };
        }
    }

    public class EmployeeInsuranceDTO
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public EmployeeInsurance FromDTO()
        {
            return new EmployeeInsurance()
            {
                Id = this.Id,
                Number = this.Number
            };
        }
    }
}
