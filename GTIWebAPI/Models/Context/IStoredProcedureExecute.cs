using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    interface IStoredProcedureExecute<T> 
    {
        IEnumerable<T> ExecuteStoredProcedures(string query, params object[] parameters);
    }
}
