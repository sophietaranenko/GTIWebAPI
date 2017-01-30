using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    public class ActionDTO
    {
        [Key]
        public int Id { get; set; }

        public string ActionName { get; set; }

        public string ActionLongName { get; set; }
    }
}
