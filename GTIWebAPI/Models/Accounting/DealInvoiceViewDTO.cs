using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class DealInvoiceViewDTO
    {
        public Guid? DealId { get; set; }

        public string DealNumber { get; set; }

        public string CurrencyName { get; set; }

        public int? CurrencyId { get; set; }
        
        public string PaymentMode { get; set; }

        public int? PaymentModeId { get; set; }
        
        public string Number { get; set; }

        public string OfficialNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string PayerName { get; set; }

        public int? ClientGTIId { get; set; }

        public int? ClientId { get; set; }

        public string RE { get; set; }

        public string Voyage { get; set; }
        
        public decimal? USD { get; set; }
        
        public decimal? CurrencySum { get; set; }

        public decimal? USDVAT { get; set; }

        public decimal? CurrencyVAT { get; set; }

        public string Status { get; set; }

        public int StatusInt { get; set; }

        public decimal? DebtUSD { get; set; }

        public decimal? DebtCurrency { get; set; }

        public decimal? DebtVat { get; set; }

        public decimal? TotalUsdIncome { get; set; }
        
        public decimal? DebtVatUsd { get; set; }
        
        public Guid? InvoiceId { get; set; }
        
        public int? Id { get; set; }

        public int? Cancelled { get; set; }

        public int? Paid { get; set; }

        public DateTime? PaidDate { get; set; }

    }
}
