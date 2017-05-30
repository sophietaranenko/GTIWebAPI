using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    public class SalesWeek
    {
        public int Year { get; set; }

        public int Week { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public int OfficeId { get; set; }

        public decimal Sales { get; set; }

        public decimal Expenses { get; set; }

        public decimal Profit { get; set; }

        public decimal SalesProfitPercent { get; set; }

        public decimal CompanyProfitPercent { get; set; }

        public decimal PercentProfitSalesKPI { get; set; }
    }
}
