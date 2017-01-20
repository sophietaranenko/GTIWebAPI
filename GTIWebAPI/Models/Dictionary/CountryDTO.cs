using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    public class CountryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string InternationalName { get; set; }

        public string FullName { get; set; }
    }
}
