using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Dictionary;

namespace GTIWebAPI.Models
{
    public class DbEnter : IdentityDbContext<ApplicationUser>
    {
        public DbEnter()
            : base("Data Source=192.168.0.229;Initial Catalog=Crew;User ID=sa;Password=12345")
        {
        }

        public static DbEnter Create()
        {
            return new DbEnter();
        }
        public virtual DbSet<EditRoleModel> EditRoleModels { get; set; }
        public virtual DbSet<ApplicationRole> IdentityRoles { get; set; }
        public virtual DbSet<LoginEditViewModel> LoginDetailViewModels { get; set; }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }

        public virtual int NewId(string tableName)
        {
            SqlParameter table = new SqlParameter("@table_name", tableName);
            int result = this.Database.SqlQuery<int>("exec table_id @table_name", table).FirstOrDefault();
            return result;
        }
    }
}
