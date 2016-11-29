using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;

namespace GTIWebAPI.Models.Context
{
    public class DbPersonnel : DbContext
    {
        public DbPersonnel()
            : base("Data Source=192.168.0.229;Initial Catalog=Crew;User ID=sa;Password=12345")
        {
        }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<EmployeeCar> EmployeeCar { get; set; }
        public virtual DbSet<EmployeeDocumentScan> EmployeeDocumentScan { get; set; }
        public virtual DbSet<EmployeeDrivingLicense> EmployeeDrivingLicense { get; set; }
        public virtual DbSet<EmployeeEducation> EmployeeEducation { get; set; }
        public virtual DbSet<EmployeeInternationalPassport> EmployeeInternationalPassport { get; set; }
        public virtual DbSet<EmployeeLanguage> EmployeeLanguage { get; set; }
        public virtual DbSet<EmployeeMilitaryCard> EmployeeMilitaryCard { get; set; }
        public virtual DbSet<EmployeePassport> EmployeePassport { get; set; }
        public virtual DbSet<EmployeePhoto> EmployeePhoto { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Profession> Profession { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<EmployeeContact> EmployeeContact { get; set; }
        public virtual DbSet<EmployeeSicklist> EmployeeSicklist { get; set; }
        public virtual DbSet<EmployeeOffice> EmployeeOffice { get; set; }
        public virtual DbSet<OrderGroup> OrderGroup { get; set; }
        public virtual DbSet<OrderType> OrderType { get; set; }
        public virtual DbSet<EmployeeOrder> EmployeeOrder { get; set; }
        public virtual DbSet<EmployeeOrderContent> EmployeeOrderContent { get; set; }
        public virtual DbSet<EmployeeFoundationDoc> EmployeeFoundationDoc { get; set; }
        public virtual DbSet<FoundationDocument> FoundationDocument { get; set; }
        public virtual DbSet<EmployeeGun> EmployeeGun { get; set; }
        public virtual DbSet<EmployeePaper> EmployeePaper { get; set; }
        public virtual DbSet<PaperType> PaperType { get; set; }
        public virtual DbSet<Office> Offices { get; set; }

        public virtual DbSet<EmployeeView> EmployeeView { get; set; }

        public virtual int NewId(string tableName)
        {
            SqlParameter table = new SqlParameter("@table_name", tableName);
            int result = this.Database.SqlQuery<int>("exec table_id @table_name", table).FirstOrDefault();
            return result;
        }





        public IEnumerable<EmployeeView> EmployeeFilter(string myFilter)
        {
            if (myFilter == null)
                myFilter = "";

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@filter",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Size = 1000,
                Value = myFilter
            };

            List<EmployeeView> employeeList = new List<EmployeeView>();
            try
            {
                var result = Database.SqlQuery<EmployeeView>("exec EmployeeFilter @filter", parameter).ToList();
                employeeList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
             
            return employeeList;
        }

     

    }
}