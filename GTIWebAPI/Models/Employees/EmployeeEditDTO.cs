using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// DTO form Employee Edit
    /// </summary>
    public class EmployeeEditDTO
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee Sex in Int
        /// </summary>
        public short? Sex { get; set; }

        /// <summary>
        /// Employee identity code
        /// </summary>
        public string IdentityCode { get; set; }

        /// <summary>
        /// Employee date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Employee Address Id
        /// </summary>
        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; }

    }
}
