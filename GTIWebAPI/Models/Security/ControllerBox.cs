using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    [Table("RightControllerBox")]
    public class ControllerBox
    {
        public ControllerBox()
        {
            Controllers = new HashSet<Controller>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Controller> Controllers { get; set; }

    }


    public class ControllerBoxDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ControllerDTO> Controllers { get; set; }
    }

    public class RightControllerBoxDTO : IEquatable<RightControllerBoxDTO>
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public IEnumerable<RightControllerDTO> Controllers { get; set; }

        public bool Equals(RightControllerBoxDTO other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
