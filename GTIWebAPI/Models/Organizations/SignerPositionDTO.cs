using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class SignerPositionDTO
    {
        public int Id { get; set; }

        public string RusNomCase { get; set; }

        public string RusGenCase { get; set; }

        public string UkrNomCase { get; set; }

        public string UkrGenCase { get; set; }

        public string EngNomCase { get; set; }

        public int? OrderByColumn { get; set; }
    }
}
