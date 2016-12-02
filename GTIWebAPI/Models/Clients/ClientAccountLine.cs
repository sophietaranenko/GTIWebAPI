using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Clients
{
    [Table("acc_sp")]
    public class ClientAccountLine
    {
        [Column("naimen")]
        public string Name { get; set; }

        [Column("total")]
        public decimal Amount { get; set; } 
    }
}