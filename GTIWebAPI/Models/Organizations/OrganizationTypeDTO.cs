using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class OrganizationTypeDTO
    {
        public int Id { get; set; }

        public string RussianName { get; set; }

        public string UkrainianName { get; set; }

        public string EnglishName { get; set; }

        public string RussianExplanation { get; set; }

        public string UkrainianExplanation { get; set; }

        public string EnglishExplanation { get; set; }

        public string Name { get; set; }
    }
}
