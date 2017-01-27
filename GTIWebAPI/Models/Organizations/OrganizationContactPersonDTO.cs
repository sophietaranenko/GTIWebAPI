using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationContactPersonDTO
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? OrganizationId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Position { get; set; }

        public virtual ICollection<OrganizationContactPersonContactDTO> OrganizationContactPersonContact { get; set; }
    }
}
