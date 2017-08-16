using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Sales
{
    [Table("InteractionMember")]
    public partial class InteractionMember
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InteractionMember()
        {
        }

        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? InteractionId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Interaction Interaction { get; set; }

        public InteractionMemberDTO ToDTO()
        {
            return new InteractionMemberDTO()
            {
                Id = this.Id,
                EmployeeId = this.EmployeeId,
                InteractionId = this.InteractionId,
                Employee = this.Employee == null ? null : this.Employee.ToShortForm()
            };
        }
    }

    public class InteractionMemberDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? InteractionId { get; set; }

        public EmployeeShortForm Employee { get; set; }

        public InteractionMember FromDTO()
        {
            return new InteractionMember()
            {
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                InteractionId = this.InteractionId
            };
        }

    }
}
