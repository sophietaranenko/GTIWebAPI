using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// DTO for EmployeePassport
    /// </summary>
    public class EmployeePassportDTO
    {
        /// <summary>
        /// EmployeePassport Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee owner Id
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Passport seria
        /// </summary>
        public string Seria { get; set; }

        /// <summary>
        /// Passport number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Employee First Name specified in Passport
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Employee second name specified in Passport
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Employee surname specified in Passport
        /// </summary>
        public string Surname { get; set; }
        
        /// <summary>
        /// Short format of employee name "С.Э. Тараненко" 
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Date when passport was issued to Employee
        /// </summary>
        public DateTime? IssuedWhen { get; set; }

        /// <summary>
        /// Government organization that issued Passport to Employee
        /// </summary>
        public string IssuedBy { get; set; }

        /// <summary>
        /// Id of registration Id specified in Passport 
        /// </summary>
        public int? AddressId { get; set; }
        
        /// <summary>
        /// AddressDTO from registration Address
        /// </summary>
        public virtual AddressDTO Address { get; set; }
    }
}
