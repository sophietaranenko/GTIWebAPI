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
    [Table("RightControllerAction")]
    public class Action: GTITable
    {
        public Action()
        {
            UserRights = new List<UserRight>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public Int32 ControllerId { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        protected override string TableName { get { return "RightControllerAction"; } }

        public virtual ICollection<UserRight> UserRights { get; set; }
    }

    public class ActionDTO
    {
        [Key]
        public int Id { get; set; }

        public string ActionName { get; set; }

        public string ActionLongName { get; set; }
    }
}
