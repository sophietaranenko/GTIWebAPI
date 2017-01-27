using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
using System.Data.Common;

namespace GTIWebAPI.Models.Context
{
    public class DbPersonnel : DbContext
    {
        public DbPersonnel()
            : base("Data Source=192.168.0.229;Initial Catalog=GTIWeb_DEV;User ID=sa;Password=12345")
        {
        }

        /// <summary>
        /// Addresses block
        /// </summary>
        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<AddressLocality> Localities { get; set; }

        public virtual DbSet<AddressPlace> Places { get; set; }

        public virtual DbSet<AddressRegion> Regions { get; set; }

        public virtual DbSet<AddressVillage> Villages { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Continent> Continents { get; set; }


        /// <summary>
        /// Employees block
        /// </summary>
        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<EmployeeCar> EmployeeCars { get; set; }

        public virtual DbSet<EmployeeDocumentScan> EmployeeDocumentScans { get; set; }

        public virtual DbSet<EmployeeDrivingLicense> EmployeeDrivingLicenses { get; set; }

        public virtual DbSet<EmployeeEducation> EmployeeEducations { get; set; }

        public virtual DbSet<EmployeeInternationalPassport> EmployeeInternationalPassports { get; set; }

        public virtual DbSet<EmployeeLanguage> EmployeeLanguages { get; set; }

        public virtual DbSet<EmployeeMilitaryCard> EmployeeMilitaryCards { get; set; }

        public virtual DbSet<EmployeePassport> EmployeePassports { get; set; }

        public virtual DbSet<EmployeePhoto> EmployeePhotos { get; set; }

        public virtual DbSet<EmployeeContact> EmployeeContacts { get; set; }

        public virtual DbSet<EmployeeOffice> EmployeeOffices { get; set; }

        public virtual DbSet<EmployeeFoundationDocument> EmployeeFoundationDocuments { get; set; }

        public virtual DbSet<EmployeeGun> EmployeeGun { get; set; }

        //later

        //public virtual DbSet<EmployeeSicklist> EmployeeSicklists { get; set; }

        //public virtual DbSet<EmployeeOrder> EmployeeOrders { get; set; }

        //public virtual DbSet<EmployeeOrderContent> EmployeeOrderContent { get; set; }

        //public virtual DbSet<EmployeePaper> EmployeePaper { get; set; }

        /// <summary>
        /// Some staff
        /// </summary>

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<Profession> Professions { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<ContactType> ContactTypes { get; set; }
        
        public virtual DbSet<FoundationDocument> FoundationDocuments { get; set; }

        public virtual DbSet<EducationStudyForm> EducationStudyForms { get; set; }
        
        public virtual DbSet<EmployeeLanguageType> EmployeeLanguageTypes { get; set; }


        public virtual DbSet<Office> Offices { get; set; }

        //public virtual DbSet<OrderGroup> OrderGroup { get; set; }
        //public virtual DbSet<OrderType> OrderType { get; set; }
        //public virtual DbSet<PaperType> PaperType { get; set; }

        /// <summary>
        /// Procedure call that returns unique number to name photo with
        /// </summary>
        /// <returns></returns>
        public virtual int FileNameUnique()
        {
            string tableName = "FileNameUnique";
            SqlParameter table = new SqlParameter("@table_name", tableName);
            int result = this.Database.SqlQuery<int>("exec table_id @table_name", table).FirstOrDefault();
            return result;
        }




        /// <summary>
        /// Stored procedure call that filters employee list 
        /// </summary>
        /// <param name="myFilter"></param>
        /// <returns></returns>
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

        public IEnumerable<EmployeeView> EmployeeByOffices(IEnumerable<int> officeIds)
        {
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("Value");

            foreach (var id in officeIds)
            {
                DataRow row = dataTable.NewRow();
                row["Value"] = id;
                dataTable.Rows.Add(row);
            }

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@OfficeIds",
                TypeName = "ut_IntList",
                Value = dataTable,
                SqlDbType = SqlDbType.Structured
            };

            List<EmployeeView> employeeList = new List<EmployeeView>();
            try
            {
                var result = Database.SqlQuery<EmployeeView>("exec EmployeeByOfficeIds @OfficeIds", parameter).ToList();
                employeeList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return employeeList;
        }

        /// <summary>
        /// Stored procedure call that filters employee list 
        /// </summary>
        /// <param name="myFilter"></param>
        /// <returns></returns>
        public IEnumerable<EmployeeDocumentScanDTO> EmployeeAllDocumentScans(int employeeId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@Employeeid",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = employeeId 
            };

            List<EmployeeDocumentScanDTO> scanList = new List<EmployeeDocumentScanDTO>();
            try
            {
                var result = Database.SqlQuery<EmployeeDocumentScanDTO>("exec EmployeeDocumentScanValid @EmployeeId", parameter).ToList();
                scanList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return scanList;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactType>()
                .HasMany(e => e.EmployeeContact)
                .WithOptional(e => e.ContactType)
                .HasForeignKey(e => e.ContactTypeId);

            modelBuilder.Entity<EducationStudyForm>()
                .HasMany(r => r.EmployeeEducations)
                .WithOptional(r => r.EducationStudyForm)
                .HasForeignKey(r => r.StudyFormId);

            modelBuilder.Entity<AddressRegion>()
                .HasMany(r => r.Addresses)
                .WithOptional(r => r.AddressRegion)
                .HasForeignKey(r => r.RegionId);

            modelBuilder.Entity<AddressPlace>()
                .HasMany(r => r.Addresses)
                .WithOptional(r => r.AddressPlace)
                .HasForeignKey(r => r.PlaceId);

            modelBuilder.Entity<AddressVillage>()
                .HasMany(r => r.Addresses)
                .WithOptional(r => r.AddressVillage)
                .HasForeignKey(r => r.VillageId);

            modelBuilder.Entity<AddressLocality>()
                .HasMany(r => r.Addresses)
                .WithOptional(r => r.AddressLocality)
                .HasForeignKey(r => r.LocalityId);
        }

    }
}