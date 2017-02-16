using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public class DbSession : DbContext
    {

        public DbSession()
           // : base("Data Source=192.168.0.229;Initial Catalog=GTIWeb_DEV;User ID=sa;Password=12345")
            : base("name=DbSession") 
        {
        }

        public bool CreateSession(string UserId)
        {
            bool result = false;

            SqlParameter pUserId = new SqlParameter
            {
                ParameterName = "@UserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = UserId
            };

            try
            {
                var res = Database.SqlQuery<bool>(
                    @"Create table #session
(
    UserId nvarchar(128)
)
insert into #session (UserId)
       values(@UserId)
       select cast(1 as bit) ",
                  pUserId
                 ).FirstOrDefault();

                //var res = Database.SqlQuery<bool>("exec CreateSession @UserId",
                //  pUserId
                // ).FirstOrDefault();
                result = res;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return result; 
        }


        public int GetCurrentUser()
        {
            int result = 0;


            try
            {
                var res = Database.SqlQuery<Int32>(@"if object_id('tempdb..#session') IS NOT NULL

       select top 1 u.TableId 
           from #session s
		   inner join AspNetUsers u
		   on s.UserId = u.Id

		   else 

		   select cast(1 as int) ").FirstOrDefault();
                //var res = Database.SqlQuery<Int32>("exec GetCurrentUser").FirstOrDefault();
                result = res;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return result;
        }

    }
}
