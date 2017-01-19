using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Clients
{
    /// <summary>
    /// Profile of organization contact info
    /// </summary>
    public class OrganizationContactUserDTO
    {
        /// <summary>
        /// Info about contact (a person)
        /// </summary>
        public OrganizationContactDTO OrganizationContact { get; set; }

        /// <summary>
        /// Info about organization person relates to
        /// </summary>
        public OrganizationEditDTO Organization { get; set; }

        /// <summary>
        /// Profile picture of contact (from AspNetUserImages, related to AspNetUser)
        /// </summary>
        public string ProfilePicture { get; set; }
         
    }
}
