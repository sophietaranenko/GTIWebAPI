using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    public class ControllerDTO
    {
        [Key]
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public List<ActionDTO> Actions { get; set; }
    }
}
