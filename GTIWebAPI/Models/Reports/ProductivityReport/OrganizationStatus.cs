using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    public class OrganizationStatus
    {
        public int Count { get; set; }
        
        public int Potential { get; set; }
        
        public int QuarantineNoDeals { get; set; }
        
        public int QuarantineWithDeals { get; set; }
        
        public int Lost { get; set; }
        
        public int Active { get; set; }
        
        public int EmptyStatus { get; set; }
        
        public int Unattended { get; set; }
         
    }
}
