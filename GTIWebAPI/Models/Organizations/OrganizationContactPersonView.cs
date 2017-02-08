using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    [Table("OrganizationContactPersonView")]
    public partial class OrganizationContactPersonView
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? OrganizationId { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Position { get; set; }

        public string UserName { get; set; }

        public bool IsRegistered
        {
            get
            {
                return UserName == null ? false : true; 
            }
        }

    }
}
