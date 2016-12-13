using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{

    public class DbService : DbContext
    {

        
        public DbService()
            : base("Data Source=192.168.0.229;Initial Catalog=Crew;User ID=sa;Password=12345")
        {
        }

        public static DbService Create()
        {
            return new DbService();
        }

       // public DbSet<Controller> Controllers { get; set; }
       //  public DbSet<Security.Action> Actions { get; set; }

        // for searching user
        public DbSet<Employees.Employee> Employees { get; set; }
        public DbSet<Clients.Client> Clients { get; set; }
        //public DbSet<UserRight> UserRights { get; set; }

        //public virtual int NewId(string tableName)
        //{
        //    SqlParameter table = new SqlParameter("@table_name", tableName);
        //    int result = this.Database.SqlQuery<int>("exec table_id @table_name", table).FirstOrDefault();
        //    return result;
        //}

    }
}
