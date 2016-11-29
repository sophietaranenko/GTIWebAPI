using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    [Table("klient")]
    public class ClientGTI 
    {
        [Column("kod")]
        public int Id { get; set; }
        [Column("naimen_f")]
        public string FullName { get; set; }
        [Column("naimen")]
        public string ShortName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("tel")]
        public string Phone { get; set; }
        [Column("adres")]
        public string Address { get; set; }
        //[Column("UserId")]
        //public string ApplicationUserId { get; set; }
        //public ApplicationUser User { get; set; }
        [Column("office")]
        public int OfficeId { get; set; }
    }
}
