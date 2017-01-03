using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    public class ClientContactDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public string PhoneAdd { get; set; }
       
        public string PhoneHome { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }

        public string EmailAdd { get; set; }

        public int? ClientId { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DateOfBirth { get; set; }

    }
}
