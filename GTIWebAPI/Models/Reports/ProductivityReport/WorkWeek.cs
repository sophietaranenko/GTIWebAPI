using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    public class WorkWeek
    {
        public int Year { get; set; }

        public int Week { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public int OfficeId { get; set; }

        public int DealsCount { get; set; }

        public int Count20 { get; set; }

        public int Count40 { get; set; }

        public int CountOther { get; set; }

        public int CountUnit { get; set; }

        public decimal PlannedProfit { get; set; }
    }
}
