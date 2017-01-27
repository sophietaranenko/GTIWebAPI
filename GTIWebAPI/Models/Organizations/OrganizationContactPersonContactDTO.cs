using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationContactPersonContactDTO
    {
        public int Id { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public int? ContactTypeId { get; set; }

        public ContactTypeDTO ContactType { get; set; }

        public string Value { get; set; }
    }
}
