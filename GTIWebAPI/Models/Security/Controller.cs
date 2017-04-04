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
            UserRights = new List<UserRight>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public string Name { get; set; }

        public int BoxId { get; set; }

        [ForeignKey("BoxId")]
        public virtual ControllerBox ControllerBox { get; set; }

        protected override string TableName { get { return "RightController"; } }

        public virtual ICollection<UserRight> UserRights { get; set; }
    }

    public class ControllerDTO
    {
        [Key]
        public int Id { get; set; }

        public string ControllerName { get; set; }

        public List<ActionDTO> Actions { get; set; }
    }
}
