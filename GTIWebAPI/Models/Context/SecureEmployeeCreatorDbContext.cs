using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    class SecureEmployeeCreatorDbContext : DbContext
    {
        public SecureEmployeeCreatorDbContext() : base("name=DbSecureEmployeeCreator")
        {
            Database.SetInitializer<MainDbContext>(null);
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

        public int UpdateEmployee(int employeeId, string userId)
        {
            int methodResult = 0;
            SqlParameter eP = new SqlParameter
            {
                DbType = System.Data.DbType.Int32,
                Value = employeeId
            };
            SqlParameter uP = new SqlParameter
            {
                DbType = System.Data.DbType.String,
                Value = userId
            };

            try
            {
                var result = Database.SqlQuery<int>("exec UpdateEmployee @EmployeeId, @UserId").FirstOrDefault();
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
