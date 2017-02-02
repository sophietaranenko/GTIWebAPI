using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Service
{
    [Table("Office")]
    public class OfficeSecurity
    {

        public OfficeSecurity()
        {
            UserRights = new HashSet<UserRight>();
        }

        public virtual ICollection<UserRight> UserRights { get; set; }

        public int Id { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string EnglishName { get; set; }

        public string DealIndex { get; set; }
    }
}
