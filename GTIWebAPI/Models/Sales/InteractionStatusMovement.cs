using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Sales
{
    [Table("InteractionStatusMovement")]
    public partial class InteractionStatusMovement
    {
        public int Id { get; set; }

        public int InteractionId { get; set; }

        public virtual Interaction Interaction { get; set; }

        public int StatusId { get; set; }

        public virtual InteractionStatus Status { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employe { get; set; }

        public DateTime MoveDate { get; set; }

        public InteractionStatusMovementDTO ToDTO()
        {
            return new InteractionStatusMovementDTO()
            {
                Id = this.Id,
                MoveDate = this.MoveDate,
                EmployeeId = this.EmployeeId,
                InteractionId = this.InteractionId,
                Status = this.Status == null ? null : this.Status.ToDTO(),
                StatusId = this.StatusId
            };
        }
    }

    public class InteractionStatusMovementDTO
    {
        public int Id { get; set; }

        public int InteractionId { get; set; }

        public int StatusId { get; set; }

        public InteractionStatusDTO Status { get; set; }

        public int EmployeeId { get; set; }

        public DateTime MoveDate { get; set; }

        public InteractionStatusMovement FromDTO()
        {
            return new InteractionStatusMovement()
            {
                Id = this.Id,
                MoveDate = this.MoveDate,
                EmployeeId = this.EmployeeId,
                InteractionId = this.InteractionId,
                StatusId = this.StatusId
            };
        }
    }


}
