using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Dictionary;
using System.Data;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Personnel;

namespace GTIWebAPI.Models.Context
{
    /// <summary>
    /// Db set for organization users (contains info about organizations and what they need to read to show them) 
    /// Rights to add and update deal document scans, show all other information 
    /// </summary>
    public class DbOrganization : DbContext, IDbContextOrganization
    {

        public DbOrganization()
            : base("Data Source=192.168.0.229;Initial Catalog=GTIWeb_DEV;User ID=sa;Password=12345")
        {
        }

        public static DbOrganization Create()
        {
            return new DbOrganization();
        }



        //For addresses work
        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<AddressLocality> Localities { get; set; }

        public virtual DbSet<AddressPlace> Places { get; set; }

        public virtual DbSet<AddressRegion> Regions { get; set; }

        public virtual DbSet<AddressVillage> Villages { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Continent> Continents { get; set; }




        //Some Id-Name data 
        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<OrganizationGTI> GTIOrganizations { get; set; }

        public virtual DbSet<ContactType> ContactTypes { get; set; }

        public virtual DbSet<Office> Offices { get; set; }


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

        public virtual DbSet<OrganizationLanguageShortName> OrganizationLanguageShortNames { get; set; }


        //Organizations - for woth with new Organization table and OrganizationGTI - synonym of klient
        /// <summary>
        /// Stored procedure call that filters organization view by text filter (filter parameters separated by space symbols) 
        /// </summary>
        /// <param name="myFilter"></param>
        /// <returns></returns>
        public IEnumerable<OrganizationView> GetOrganizationsFilter(string myFilter)
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

        public IEnumerable<OrganizationView> GetOrganizationsByOffices(IEnumerable<int> officeIds)
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

        public IEnumerable<OrganizationGTI> SearchOrganizationGTI(IEnumerable<int> officeIds, string registrationNumber)
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

            IEnumerable<OrganizationGTI> gtis = new List<OrganizationGTI>();
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

        public IEnumerable<OrganizationSearchDTO> SearchOrganization(int countryId, string registrationNumber)
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

            IEnumerable<OrganizationSearchDTO> gtis = new List<OrganizationSearchDTO>();
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
        public IEnumerable<DealViewDTO> GetDealsFiltered(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            DateTime dateBeginParameter = dateBegin.GetValueOrDefault();
            DateTime dateEndParameter = dateEnd.GetValueOrDefault();

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
                Value = dateBeginParameter
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEndParameter
            };

            IEnumerable<DealViewDTO> dealList = new List<DealViewDTO>();
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
        public DealFullViewDTO GetDealCardInfo(Guid dealId)
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
        public IEnumerable<DealContainerViewDTO> GetContainersByDeal(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            IEnumerable<DealContainerViewDTO> dto = new List<DealContainerViewDTO>();
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
        public IEnumerable<DealInvoiceViewDTO> GetInvoicesByDeal(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            IEnumerable<DealInvoiceViewDTO> dto = new List<DealInvoiceViewDTO>();
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
        public IEnumerable<DealInvoiceViewDTO> GetInvoicesList(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            DateTime dateBeginParameter = dateBegin.GetValueOrDefault();
            DateTime dateEndParameter = dateEnd.GetValueOrDefault();

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
                Value = dateBeginParameter
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEndParameter
            };

            IEnumerable<DealInvoiceViewDTO> invoiceList = new List<DealInvoiceViewDTO>();
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

        public InvoiceFullViewDTO GetInvoiceCardInfo(int invoiceId)
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
        public IEnumerable<InvoiceLineViewDTO> GetInvoiceLinesByInvoice(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            IEnumerable<InvoiceLineViewDTO> dto = new List<InvoiceLineViewDTO>();
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

        public IEnumerable<InvoiceContainerViewDTO> GetContainersByInvoiceId(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            IEnumerable<InvoiceContainerViewDTO> dto = new List<InvoiceContainerViewDTO>();
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
        public IEnumerable<DealContainerViewDTO> GetContainersFiltered(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            DateTime dateBeginParameter = dateBegin.GetValueOrDefault();
            DateTime dateEndParameter = dateEnd.GetValueOrDefault();

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
                Value = dateBeginParameter
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEndParameter
            };

            IEnumerable<DealContainerViewDTO> containersList = new List<DealContainerViewDTO>();
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

        public DealContainerViewDTO GetContainer(Guid id)
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
        public IEnumerable<DocumentScanTypeDTO> GetDocumentScanTypes()
        {
            IEnumerable<DocumentScanTypeDTO> dtos = new List<DocumentScanTypeDTO>();

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

        public IEnumerable<DocumentScanDTO> GetDocumentScanByDeal(Guid dealId)
        {
            IEnumerable<DocumentScanDTO> dtos = new List<DocumentScanDTO>();


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

        public IEnumerable<OrganizationGTIShortDTO> GetOrganizationGTIByOrganization(int organizationId)
        {
            IEnumerable<OrganizationGTIShortDTO> dtos = new List<OrganizationGTIShortDTO>();


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

        public Guid InsertDealDocumentScan(Guid dealId, byte[] fileContent, string fileName, string email, int documentScanTypeId)
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

        public DocumentScanDTO UpdateDocumentScanType(Guid scanId, int documentScanTypeId)
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
                DbType = DbType.Int16,
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


        public bool DeleteDocumentScan(Guid scanId)
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

        public DocumentScanDTO GetDealDocumentScanById(Guid scanId)
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
        

        //Addresses foreign keys 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

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