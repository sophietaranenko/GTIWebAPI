using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IServiceDbContext
    {
        int NewTableId(string tableName);
    }
}
