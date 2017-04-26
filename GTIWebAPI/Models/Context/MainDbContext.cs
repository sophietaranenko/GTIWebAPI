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
using System.Configuration;
using System.Security.Principal;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Accounting;
using System.Security.Claims;
using System.Web;
using GTIWebAPI.Models.Account;
using System.Data.Entity.Infrastructure;

namespace GTIWebAPI.Models.Context
{

    internal class MainDbContext: DbContext, IAppDbContext
    {
        public MainDbContext() : base("name=DbPersonnel")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public MainDbContext(string connectionString) : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public void MarkAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string query, params object[] parameters)
        {
            return this.Database.SqlQuery<T>(query, parameters).ToList();
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

        public virtual IDbSet<EmployeeCar> EmployeeCars { get; set; }

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

        public virtual DbSet<EmployeeGun> EmployeeGuns { get; set; }


        /// <summary>
        /// Tables with Id-Name data
        /// </summary>
        ///         //Some Id-Name data 
        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<OrganizationGTI> GTIOrganizations { get; set; }

        public virtual DbSet<ContactType> ContactTypes { get; set; }

        public virtual DbSet<Office> Offices { get; set; }

        public virtual DbSet<Profession> Professions { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<FoundationDocument> FoundationDocuments { get; set; }

        public virtual DbSet<EducationStudyForm> EducationStudyForms { get; set; }

        public virtual DbSet<EmployeeLanguageType> EmployeeLanguageTypes { get; set; }








        //Organizations
        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        public virtual DbSet<OrganizationAddressType> OrganizationAddressTypes { get; set; }

        public virtual DbSet<OrganizationContactPerson> OrganizationContactPersons { get; set; }

        public virtual DbSet<OrganizationContactPersonView> OrganizationContactPersonViews { get; set; }

        public virtual DbSet<OrganizationContactPersonContact> OrganizationContactPersonContacts { get; set; }

        public virtual DbSet<OrganizationGTILink> OrganizationGTILinks { get; set; }

        public virtual DbSet<OrganizationProperty> OrganizationProperties { get; set; }

        public virtual DbSet<OrganizationPropertyType> OrganizationPropertyTypes { get; set; }

        public virtual DbSet<OrganizationPropertyTypeAlias> OrganizationPropertyTypeAliases { get; set; }

        public virtual DbSet<OrganizationLegalForm> OrganizationLegalForms { get; set; }

        public virtual DbSet<OrganizationTaxAddress> OrganizationTaxAddresses { get; set; }

        public virtual DbSet<OrganizationTaxAddressType> OrganizationTaxAddressTypes { get; set; }

        public virtual DbSet<OrganizationLanguageName> OrganizationLanguageNames { get; set; }

        public virtual DbSet<UserImage> UserImages { get; set; }



        /// <summary>
        /// Procedure call that returns unique number to name photo with
        /// </summary>
        /// <returns></returns>
        public virtual int FileNameUnique()
        {
            string tableName = "FileNameUnique";
            SqlParameter table = new SqlParameter("@TableName", tableName);
            int result = this.Database.SqlQuery<int>("exec NewTableId @TableName", table).FirstOrDefault();
            return result;
        }



        public virtual int NewTableId(string tableName)
        {
            int result = 0;
            SqlParameter table = new SqlParameter("@TableName", tableName);
            try
            {
                result = Database.SqlQuery<int>("exec NewTableId @TableName", table).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }
        //Organizations - for woth with new Organization table and OrganizationGTI - synonym of klient
        /// <summary>
        /// Stored procedure call that filters organization view by text filter (filter parameters separated by space symbols) 
        /// </summary>
        /// <param name="myFilter"></param>
        /// <returns></returns>
        public virtual List<OrganizationView> GetOrganizationsFilter(string myFilter)
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
            List<OrganizationView> organizationList = new List<OrganizationView>();
            try
            {
                var result = Database.SqlQuery<OrganizationView>("exec OrganizationsFilter @filter", parameter).ToList();
                organizationList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return organizationList;
        }

        public virtual List<OrganizationView> GetOrganizationsByOffices(List<int> officeIds)
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

            List<OrganizationView> organizationList = new List<OrganizationView>();
            try
            {
                var result = Database.SqlQuery<OrganizationView>("exec OrganizationByOfficeIds @OfficeIds", parameter).ToList();
                organizationList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return organizationList;
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

        public bool CreateOrganization(string email, string password)
        {
            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@Username",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = email
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec CreateDatabaseExternalUser @Username ",
                    pEmail
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

        public bool CreateHoldingUser(string email, string password)
        {
            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@Username",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = email
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec CreateDatabaseHoldingUser @Username ",
                    pEmail
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

        public string GetFullUserNameByEmployeeId(int employeeId)
        {
            SqlParameter pEmployeeId = new SqlParameter
            {
                ParameterName = "@EmployeeId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = employeeId
            };

            string methodResult = "";

            try
            {
                var result = Database.SqlQuery<string>("exec GetFullAspNetUserNameByEmployeeId @EmployeeId ",
                    pEmployeeId
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