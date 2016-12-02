using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Service
{
    /// <summary>
    /// Class representing enum item
    /// </summary>
    public class EnumItem
    {
        /// <summary>
        /// Integer value of enum item
        /// </summary>
        public Int32 Value { get; set;  }

        /// <summary>
        /// Text of appropriated enum item 
        /// </summary>
        public String Text { get; set; }
    }
}
