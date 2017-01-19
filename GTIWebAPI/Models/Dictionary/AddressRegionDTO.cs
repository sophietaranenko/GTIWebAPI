using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    public class AddressRegionDTO
    {
        public byte Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
    }
}
