using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class DealViewDTO
    {
        public string Number { get; set; }
        public DateTime CreateDate { get; set; }

        public string Direction { get; set; }

        public string Cargo { get; set; }

        public string Line { get; set; }

        public string POO { get; set; }

        public string POL { get; set; }

        public string POD { get; set; }

        public string FD { get; set; }

        public int? OfficeId { get; set; }

        public Guid Id { get; set; }

        public int? DirectionId { get; set; }
    }
}
