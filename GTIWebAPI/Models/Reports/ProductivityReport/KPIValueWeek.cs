using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    public class KPIValueWeek
    {
        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public int Week { get; set; }

        public int Year { get; set; }

        public int WeeksCount { get; set; }

        public decimal AddedToDBKPI { get; set; }

        public decimal CommercialOfferKPI { get; set; }

        public decimal RejectionKPI { get; set; }

        public decimal MeetingKPI { get; set; }

        public decimal AgreementKPI { get; set; }

        public decimal PercentProfitSalesKPI { get; set; }
    }
}
