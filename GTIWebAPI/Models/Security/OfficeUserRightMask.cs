using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{

    public class OfficeUserRightMask
    {
        public Office Office { get; set; }

        public List<UserRightMask> Masks { get; set; }
    }
}
