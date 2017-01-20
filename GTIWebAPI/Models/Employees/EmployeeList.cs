using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeList
    {
        public IEnumerable<OfficeDTO> Offices { get; set; }

        public IEnumerable<ProfessionDTO> Professions { get; set; }

        public IEnumerable<DepartmentDTO> Departments { get; set; }

        public IEnumerable<FoundationDocumentDTO> FoundationDocuments { get; set; }

        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<ContactTypeDTO> ContactTypes { get; set; }

        public IEnumerable<EducationStudyFormDTO> EducationStudyForms { get; set; }

        public IEnumerable<EmployeeLanguageTypeDTO> EmployeeLanguageTypes { get; set; }

        public IEnumerable<AddressLocalityDTO> AddressLocalities { get; set; }

        public IEnumerable<AddressRegionDTO> AddressRegions { get; set; }

        public IEnumerable<AddressPlaceDTO> AddressPlaces { get; set; }

        public IEnumerable<AddressVillageDTO> AddressVillages { get; set; }

        public IEnumerable<CountryDTO> Countries { get; set; }

    }
}
