using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Reports.ProductivityReport
{

    //select s.Year, s.Week, s.DateBegin, s.DateEnd, s.EmployeeId,
    //s.Sales, s.Expenses, s.Profit, s.SalesProfitPercent, s.CompanyProfitPercent, s.OfficeId,
    //c.AllContacts, c.AddedToDB, c.CommercialOffer, c.Rejection, c.Meeting, c.Agreement,
    //w.DealsCount, w.Count20, w.Count40, w.CountOther, w.CountUnit, w.PlannedProfit
    //from PR_SalesWeekResult s
    //inner join PR_ContactsWeekResult c
    //on s.Year = c.Year and s.Week = c.Week and s.EmployeeId = c.EmployeeId
    //inner join PR_WorksWeekResult w
    //on s.Year = w.Year and s.Week = w.Week and s.EmployeeId = w.EmployeeId
    //where s.EmployeeId = 214

    public class ReportMonth
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public int OfficeId { get; set; }

        public decimal Sales { get; set; }

        public decimal Expenses { get; set; }

        public decimal Profit { get; set; }

        public decimal SalesProfitPercent { get; set; }

        public decimal CompanyProfitPercent { get; set; }

        public int AllContacts { get; set; }

        public int AddedToDB { get; set; }

        public int CommercialOffer { get; set; }

        public int Rejection { get; set; }

        public int Meeting { get; set; }

        public int Agreement { get; set; }

        public int DealsCount { get; set; }

        public int Count20 { get; set; }

        public int Count40 { get; set; }

        public int CountOther { get; set; }

        public int CountUnit { get; set; }

        public decimal PlannedProfit { get; set; }

        public decimal AddedToDBKPI { get; set; }

        public decimal CommercialOfferKPI { get; set; }

        public decimal RejectionKPI { get; set; }

        public decimal MeetingKPI { get; set; }

        public decimal AgreementKPI { get; set; }

        public decimal PercentProfitSalesKPI { get; set; }

    }
}
