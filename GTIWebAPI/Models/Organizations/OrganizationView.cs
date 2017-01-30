using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationView
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EnglishName { get; set; }
            
        public string NativeName { get; set; }
            
        public string RussianName { get; set; }

        public string ShortName { get; set; }
            
        public string PhoneNumber { get; set; } 
            
        public string FaxNumber { get; set; } 
            
        public string Website { get; set; }
            
        public string Email { get; set; }

        public string Skype { get; set; }
            
        public bool? Deleted { get; set; }
            
        public int? CountryId { get; set; }
            
        public int? OrganizationLegalFormId { get; set; }

        public string OrganizationLegalFormName { get; set; }

        public string OrganizationLegalFormExplanation { get; set; }

        public string OrganizationRegistrationCountryName { get; set; }

        public string CreatorShortName { get; set; }
    }
}
