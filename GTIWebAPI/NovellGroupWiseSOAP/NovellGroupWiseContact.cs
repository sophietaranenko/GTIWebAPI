using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovellGroupWiseSOAP
{
    public class NovellGroupWiseContact
    {
        public string UUId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string TYpe { get; set; }

        public string FirstName { get; set; }

        public string LastNAme { get; set; }

    }

    public class NovellGroupWiseContactBook
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<NovellGroupWiseContact> Contacts { get; set; }
    }
}
