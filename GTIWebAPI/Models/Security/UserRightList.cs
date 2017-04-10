using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    public class UserRightList
    {
        public List<ControllerDTO> Controllers { get; set; }

        public List<ActionDTO> Actions { get; set; }
    }
}
