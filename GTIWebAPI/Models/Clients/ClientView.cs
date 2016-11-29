using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Clients
{
    [Table("ClientView")]
    public class ClientView
    {
        public Int32 Id { get; set; }
        public String FullName { get; set; }
        public String UserName { get; set; }
        public String ShortName { get; set; }
        public Int32? EmployeeId { get; set; }
        public Int32? DealCount { get; set; }
        public Int32? CntrCount { get; set; }
        public Decimal? SumUsd { get; set; }

    }
}