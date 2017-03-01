using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    class DbSecureEmployeeCreator : DbContext
    {

        public DbSecureEmployeeCreator() : base("name=DbSecureEmployeeCreator")
        {
        }

        public int CreateEmployee()
        {


            int methodResult = 0;

            try
            {
                var result = Database.SqlQuery<int>("exec CreateEmployee").FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

    }
}
