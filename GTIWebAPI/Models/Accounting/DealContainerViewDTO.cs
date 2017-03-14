using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class DealContainerViewDTO
    {
        public string Container { get; set; }

        public string Type { get; set; } 

        public DateTime? ReadinessDate { get; set; }

        public DateTime? ETSPOL { get; set; } 

        public DateTime? ETAPOT { get; set; }

        public string Terminal { get; set; }
        
        public DateTime? ETAPOD { get; set; }
        
        public string DocumentNo { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime? TerminalHandlingDate { get; set; }

        public DateTime? ClearanceDate { get; set; }

        public DateTime? GateOutFullDate { get; set; }

        public string PlatformTruck { get; set; }

        public string PolicyNo { get; set; }

        public DateTime? PolicyDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? GateInEmptyDate { get; set; }

        public string Seal { get; set; }

        public string MRN { get; set; }

        public decimal Weight { get; set; }

        public Guid Id { get; set; }

        public Guid DealId { get; set; }

        public string DealCargo { get; set; }

    }
}
