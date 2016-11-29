using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    [Table("account")]
    public class ClientAccount
    {
        [Column("kod")]
        public int Id { get; set; }

        [Column("total_usd")]
        public decimal Amount { get; set; }

        [Column("total_grn")]
        public decimal AmountNative { get; set; }

        [Column("total_nds")]
        public decimal AmountVat { get; set; }

        [Column("data")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Column("plat")]
        public int? ClientGTIId { get; set; }

        [Column("nomer")]
        public int Number { get; set; }

        [Column("office")]
        public int? OfficeId { get; set; }

        [Column("booking_rc")]
        public Guid? DealId { get; set; }

        [Column("data_opl")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DatePaid { get; set; }

    }
}
