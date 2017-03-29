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

namespace GTIWebAPI.Models.Context
{
    /// <summary>
    /// Context for working with Employees 
    /// </summary>
    internal class MainDbContext : DbContext, IAppDbContext //, IPrincipalProvider
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




        public virtual bool IsEmployeeInformationFilled(int employeeId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@EmployeeId",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Size = 1000,
                Value = employeeId
            };
            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec IsEmployeeInformationFilled @EmployeeId", parameter).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return methodResult;
        }

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


        /// <summary>
        /// Stored procedure call that filters employee list 
        /// </summary>
        /// <param name="myFilter"></param>
        /// <returns></returns>
        public virtual List<EmployeeView> EmployeeFilter(string myFilter)
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

        public virtual List<EmployeeView> EmployeeByOffices(List<int> officeIds)
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
        public virtual List<EmployeeDocumentScan> EmployeeAllDocumentScans(int employeeId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@Employeeid",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = employeeId
            };

            List<EmployeeDocumentScan> scanList = new List<EmployeeDocumentScan>();
            try
            {
                var result = Database.SqlQuery<EmployeeDocumentScan>("exec EmployeeDocumentScanValid @EmployeeId", parameter).ToList();
                scanList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return scanList;
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

        public virtual List<OrganizationGTI> SearchOrganizationGTI(List<int> officeIds, string registrationNumber)
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

            SqlParameter pOffices = new SqlParameter
            {
                ParameterName = "@OfficeIds",
                TypeName = "ut_IntList",
                Value = dataTable,
                SqlDbType = SqlDbType.Structured
            };

            SqlParameter pRegistrationNumber = new SqlParameter
            {
                ParameterName = "@RegistrationNumber",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Size = 1000,
                Value = registrationNumber
            };

            List<OrganizationGTI> gtis = new List<OrganizationGTI>();
            try
            {
                var result = Database.SqlQuery<OrganizationGTI>("exec SearchOrganizationGTI @OfficeIds, @RegistrationNumber", pOffices, pRegistrationNumber).ToList();
                gtis = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return gtis;
        }

        public virtual List<OrganizationSearchDTO> SearchOrganization(int countryId, string registrationNumber)
        {
            SqlParameter pCountryId = new SqlParameter
            {
                ParameterName = "@CountryId",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = countryId
            };
            SqlParameter pRegistrationNumber = new SqlParameter
            {
                ParameterName = "@RegistrationNumber",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Size = 50,
                Value = registrationNumber
            };

            List<OrganizationSearchDTO> gtis = new List<OrganizationSearchDTO>();
            try
            {
                var result = Database.SqlQuery<OrganizationSearchDTO>("exec SearchOrganization @CountryId, @RegistrationNumber", pCountryId, pRegistrationNumber).ToList();
                gtis = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return gtis;
        }



        //Deals - for work with DealGTI - synonym of booking
        public virtual List<DealViewDTO> GetDealsFiltered(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            SqlParameter parClient = new SqlParameter
            {
                ParameterName = "@OrganizationId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            SqlParameter parBegin = new SqlParameter
            {
                ParameterName = "@DateBegin",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateBegin
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEnd
            };

            List<DealViewDTO> dealList = new List<DealViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealViewDTO>("exec DealsFilter @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd).ToList();
                dealList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dealList;

        }

        /// <summary>
        /// Stored procedure call to return current deal data
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns>Deal info</returns>
        public virtual DealFullViewDTO GetDealCardInfo(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            DealFullViewDTO dto = new DealFullViewDTO();
            try
            {
                var result = Database.SqlQuery<DealFullViewDTO>("exec DealInfo @DealId", parameter).FirstOrDefault();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        /// <summary>
        /// Selects containers by current deal 
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns>List of containers</returns>
        public virtual List<DealContainerViewDTO> GetContainersByDeal(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            List<DealContainerViewDTO> dto = new List<DealContainerViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealContainerViewDTO>("exec DealContainersList @DealId", parameter).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }


        /// <summary>
        /// Selects invoices by current deal using 
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns>list of invoices</returns>
        public virtual List<DealInvoiceViewDTO> GetInvoicesByDeal(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            List<DealInvoiceViewDTO> dto = new List<DealInvoiceViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealInvoiceViewDTO>(
                @"exec DealInvoicesList @DealId", parameter).OrderBy(d => d.InvoiceDate).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }



        //Invoices - for wirk with InvoicesGTI - synonym of account
        /// <summary>
        /// Stored procedure call to return invoice list selected by organization and dates 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns>List of invoices</returns>
        public virtual List<DealInvoiceViewDTO> GetInvoicesList(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            SqlParameter parClient = new SqlParameter
            {
                ParameterName = "@OrganizationId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            SqlParameter parBegin = new SqlParameter
            {
                ParameterName = "@DateBegin",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateBegin
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEnd
            };

            List<DealInvoiceViewDTO> invoiceList = new List<DealInvoiceViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealInvoiceViewDTO>("exec InvoicesList @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd).ToList();
                invoiceList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return invoiceList;
        }

        public virtual InvoiceFullViewDTO GetInvoiceCardInfo(int invoiceId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };

            InvoiceFullViewDTO dto = new InvoiceFullViewDTO();
            try
            {
                var result = Database.SqlQuery<InvoiceFullViewDTO>("exec InvoiceInfo @InvoiceId", parameter).FirstOrDefault();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        /// <summary>
        /// Select view InvoiceLines by invoiceId
        /// </summary>
        /// <param name="invoiceId">Invoice Id</param>
        /// <returns>List of invoice lines</returns>
        public virtual List<InvoiceLineViewDTO> GetInvoiceLinesByInvoice(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            List<InvoiceLineViewDTO> dto = new List<InvoiceLineViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceLineViewDTO>(
                @"Exec InvoiceLineList @InvoiceId", parameter).OrderBy(l => l.LinePosition).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        public virtual List<InvoiceContainerViewDTO> GetContainersByInvoiceId(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            List<InvoiceContainerViewDTO> dto = new List<InvoiceContainerViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceContainerViewDTO>(
                @"Exec InvoiceContainerList @InvoiceId", parameter).OrderBy(l => l.Name).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }




        //Containers - for work with ContainerGTI - synonym of book_cntr
        public virtual List<DealContainerViewDTO> GetContainersFiltered(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            SqlParameter parClient = new SqlParameter
            {
                ParameterName = "@OrganizationId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            SqlParameter parBegin = new SqlParameter
            {
                ParameterName = "@DateBegin",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateBegin
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEnd
            };

            List<DealContainerViewDTO> containersList = new List<DealContainerViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealContainerViewDTO>("exec ContainersList @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd).ToList();
                containersList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return containersList;
        }

        public virtual DealContainerViewDTO GetContainer(Guid id)
        {
            SqlParameter parId = new SqlParameter
            {
                ParameterName = "@ContainerId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = id
            };

            DealContainerViewDTO container = new DealContainerViewDTO();
            try
            {
                var result = Database.SqlQuery<DealContainerViewDTO>("exec ContainerFullView @ContainerId", parId).FirstOrDefault();
                container = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return container;
        }




        //DealDocumentScans - for work with DocumentScanGTI - synonym of doc table
        public virtual List<DocumentScanTypeDTO> GetDocumentScanTypes()
        {
            List<DocumentScanTypeDTO> dtos = new List<DocumentScanTypeDTO>();

            try
            {
                var result = Database.SqlQuery<DocumentScanTypeDTO>("exec GetDocumentScanTypes").ToList();
                dtos = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return dtos;
        }

        public virtual List<DocumentScanDTO> GetDocumentScanByDeal(Guid dealId)
        {
            List<DocumentScanDTO> dtos = new List<DocumentScanDTO>();


            SqlParameter parId = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            try
            {
                var result = Database.SqlQuery<DocumentScanDTO>("exec GetDocumentScanByDeal @DealId", parId).ToList();
                dtos = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return dtos;
        }

        public virtual List<OrganizationGTIShortDTO> GetOrganizationGTIByOrganization(int organizationId)
        {
            List<OrganizationGTIShortDTO> dtos = new List<OrganizationGTIShortDTO>();


            SqlParameter parId = new SqlParameter
            {
                ParameterName = "@OrganizationId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            try
            {
                var result = Database.SqlQuery<OrganizationGTIShortDTO>("exec OrganizationGTILinkForSearchByOrganization @OrganizationId", parId).ToList();
                dtos = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return dtos;
        }

        public virtual Guid InsertDealDocumentScan(Guid dealId, byte[] fileContent, string fileName, string email, int documentScanTypeId)
        {


            SqlParameter pDealId = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                //Direction = ParameterDirection.Output,
                DbType = DbType.Guid,
                Value = dealId
            };

            SqlParameter pFileContent = new SqlParameter
            {
                ParameterName = "@FileContent",
                IsNullable = false,
                // Direction = ParameterDirection.Output,
                DbType = DbType.Binary,
                Value = fileContent
            };

            SqlParameter pFileName = new SqlParameter
            {
                ParameterName = "@FileName",
                IsNullable = false,
                //Direction = ParameterDirection,
                DbType = DbType.AnsiStringFixedLength,
                Size = 100,
                Value = fileName
            };

            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@Email",
                IsNullable = false,
                // Direction = ParameterDirection.Output,
                DbType = DbType.AnsiStringFixedLength,
                Size = 25,
                Value = email
            };

            SqlParameter pTypeId = new SqlParameter
            {
                ParameterName = "@DocumentScanTypeId",
                IsNullable = false,
                // Direction = ParameterDirection.Output,
                DbType = DbType.Int32,
                Value = documentScanTypeId
            };

            Guid scanId = Guid.NewGuid();
            try
            {
                var result = Database.SqlQuery<Guid>("exec InsertDocumentScanByDeal @DealId, @FileContent, @FileName, @Email, @DocumentScanTypeId",
                    pDealId,
                    pFileContent,
                    pFileName,
                    pEmail,
                    pTypeId
                    ).FirstOrDefault();

                scanId = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return scanId;
        }

        public virtual DocumentScanDTO UpdateDocumentScanType(Guid scanId, int documentScanTypeId)
        {
            SqlParameter pScanId = new SqlParameter
            {
                ParameterName = "@ScanId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = scanId
            };

            SqlParameter pDocumentScanTypeId = new SqlParameter
            {
                ParameterName = "@DocumentScanTypeId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = documentScanTypeId
            };

            DocumentScanDTO dto = new DocumentScanDTO();
            try
            {
                var result = Database.SqlQuery<DocumentScanDTO>("exec UpdateDocumentScanType @ScanId, @DocumentScanTypeId ",
                    pScanId, pDocumentScanTypeId
                    ).FirstOrDefault();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }


        public virtual bool DeleteDocumentScan(Guid scanId)
        {
            SqlParameter pScanId = new SqlParameter
            {
                ParameterName = "@ScanId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = scanId
            };
            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec DeleteDocumentScan @ScanId",
                    pScanId
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

        public virtual DocumentScanDTO GetDealDocumentScanById(Guid scanId)
        {
            SqlParameter pScanId = new SqlParameter
            {
                ParameterName = "@ScanId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = scanId
            };
            DocumentScanDTO dto = new DocumentScanDTO();
            try
            {
                var result = Database.SqlQuery<DocumentScanDTO>("exec GetDocumentScanById @ScanId",
                    pScanId
                    ).FirstOrDefault();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
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

    }
}