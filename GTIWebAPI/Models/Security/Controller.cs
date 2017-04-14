using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    [Table("RightController")]
    public class Controller: GTITable
    {
        public Controller()
        {
            Actions = new HashSet<Action>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public string Name { get; set; }

        public int BoxId { get; set; }

        [ForeignKey("BoxId")]
        public virtual ControllerBox ControllerBox { get; set; }

        public virtual ICollection<Action> Actions { get; set; }

        protected override string TableName { get { return "RightController"; } }
    }

    [Table("RightController")]
    public class RightController : GTITable
    {
        public RightController()
        {
            Actions = new HashSet<RightControllerAction>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public int BoxId { get; set; }

        public virtual ICollection<RightControllerAction> Actions { get; set; }

        protected override string TableName { get { return "RightController"; } }

        public RightControllerDTO ToDTO()
        {
            return new RightControllerDTO()
            {
                Id = this.Id,
                Name = this.Name,
                LongName = this.LongName,
                Actions = this.Actions.Select(d => d.ToDTO())
            };
        }


    }


    public class RightControllerDTO
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public IEnumerable<RightControllerActionDTO> Actions { get; set; }
    }

    public class RightControllerMask


    public class ControllerDTO
    {
        [Key]
        public int Id { get; set; }

        public string ControllerName { get; set; }

        public List<ActionDTO> Actions { get; set; }
    }
}
