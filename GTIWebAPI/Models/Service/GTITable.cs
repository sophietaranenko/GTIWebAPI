using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Service
{
    public abstract class GTITable
    {
        protected virtual string TableName { get; }

        public int NewId(IServiceDbContext context)
        {
            return context.NewTableId(TableName);
        }
    }
}
