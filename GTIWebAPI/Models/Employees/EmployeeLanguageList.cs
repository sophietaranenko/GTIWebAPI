using AutoMapper;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Employees
{
    public class EmployeeLanguageList
    {
        public IEnumerable<LanguageDTO> Languages { get; set; }

        public IEnumerable<EmployeeLanguageTypeDTO> EmployeeLanguageTypes { get; set; }

        public EmployeeLanguageList()
        {
        }

        public static EmployeeLanguageList CreateEmployeeLanguageList(IDbContextEmployeeLanguage db)
        {
            EmployeeLanguageList EmployeeLanguageList = new EmployeeLanguageList();

            List<EmployeeLanguageType> types = db.EmployeeLanguageTypes.ToList();
            EmployeeLanguageList.EmployeeLanguageTypes = types.Select(c => c.ToDTO()).ToList();

            List<Language> languages = db.Languages.ToList();
            EmployeeLanguageList.Languages = languages.Select(d => d.ToDTO()).ToList();

            return EmployeeLanguageList;
        }

    }
}
