using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Service
{
    public interface IGetNewTableId
    {
        Int32 GetNewTableId(string tableName);
    }
}
