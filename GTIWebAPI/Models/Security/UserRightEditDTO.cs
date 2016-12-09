using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRightEditDTO
    {
        public Int32 OfficeId { get; set; }
        public Int32 ControllerId { get; set; }
        public Int32 ActionId { get; set; }
    }
}
