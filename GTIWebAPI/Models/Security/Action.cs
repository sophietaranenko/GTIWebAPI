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
            UserRights = new HashSet<UserRight>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public Int32 ControllerId { get; set; }

        public virtual Controller Controller { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        protected override string TableName { get { return "RightControllerAction"; } }

        public virtual ICollection<UserRight> UserRights { get; set; }
    }


    //Identity Account columns shoul not cross Entities 

    [Table("RightControllerAction")]
    public class RightControllerAction : GTITable
    {
        public RightControllerAction()
        {
            UserRightMaskRights = new HashSet<UserRightMaskRight>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        public Int32 ControllerId { get; set; }

        public virtual RightController Controller { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public virtual ICollection<UserRightMaskRight> UserRightMaskRights { get; set; }

        protected override string TableName { get { return "RightControllerAction"; } }

        public RightControllerActionDTO ToDTO()
        {
            return new RightControllerActionDTO
            {
                Id = this.Id,
                Name = this.Name,
                LongName = this.LongName
            };
        }


    }
    public class RightControllerActionDTO
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public RightControllerAction FromDTO()
        {
            return new RightControllerAction
            {
                Id = this.Id,
                Name = this.Name,
                LongName = this.LongName
            };
        }
        
    }

    public class ActionDTO
    {
        [Key]
        public int Id { get; set; }

        public string ActionName { get; set; }

        public string ActionLongName { get; set; }
    }
}
