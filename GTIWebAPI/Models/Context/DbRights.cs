using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Context
{
    public class DbRights : DbContext
    {

        public DbRights() : base("name=DbRights")
        {
        }

        public bool GrantRightsToOrganization(string userId)
        {
            SqlParameter pUserId = new SqlParameter
            {
                ParameterName = "@AspNetUserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = userId
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec GrantAspNetUserRightsForOrganization @AspNetUserId ",
                    pUserId
                    ).FirstOrDefault();
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