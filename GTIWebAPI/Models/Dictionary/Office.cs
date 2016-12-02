using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    [Table("office")]
    public class Office 
    {
        [Column("kod")]
        public int Id { get; set; }
        [Column("naimen_rus")]
        public string NativeName { get; set; }
        [Column("naimen")]
        public string ShortName { get; set; }
        [Column("naimen_eng")]
        public string FullName { get; set; }
        [Column("naimen_small")]
        public string DealIndex { get; set; }
    }
}
