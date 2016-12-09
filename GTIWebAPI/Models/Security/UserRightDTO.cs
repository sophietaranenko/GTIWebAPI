using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    public class UserRightDTO
    {
        [Key]
        public Int32 OfficeId { get; set; }
        public string OfficeName { get; set; }
        public List<ControllerDTO> Controllers {get; set;}
    }
}
