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

        public int NewId(DbContext context)
        {
            SqlParameter table = new SqlParameter("@table_name", TableName);
            int result = context.Database.SqlQuery<int>("exec table_id @table_name", table).FirstOrDefault();
            return result;
        }
    }
}
