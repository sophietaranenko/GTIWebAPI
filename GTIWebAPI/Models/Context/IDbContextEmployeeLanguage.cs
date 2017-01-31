using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IDbContextEmployeeLanguage
    {
        DbSet<Language> Languages { get; set; }
        DbSet<EmployeeLanguageType> EmployeeLanguageTypes { get; set; }
    }
}
