﻿using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Personnel;
using System.Data.Entity;

namespace GTIWebAPI.Tests.TestContext
{
    public class TestDbContext : IAppDbContext
    {
        public TestDbContext()
        {
            this.Addresses = new TestDbSet<Address>();
            this.Employees = new TestDbSet<Employee>();
            this.EmployeePassports = new TestDbSet<EmployeePassport>();
            this.EmployeeCars = new TestDbSet<EmployeeCar>();
            this.EmployeeContacts = new TestDbSet<EmployeeContact>();
            this.ContactTypes = new TestDbSet<ContactType>();
            this.Continents = new TestDbSet<Continent>();
            this.Countries = new TestDbSet<Country>();
            this.Departments = new TestDbSet<Department>();
            this.EducationStudyForms = new TestDbSet<EducationStudyForm>();
            this.EmployeeDocumentScans = new TestDbSet<EmployeeDocumentScan>();
            this.EmployeeDrivingLicenses = new TestDbSet<EmployeeDrivingLicense>();
            this.EmployeeEducations = new TestDbSet<EmployeeEducation>();
            this.EmployeeFoundationDocuments = new TestDbSet<EmployeeFoundationDocument>();
            this.EmployeeGun = new TestDbSet<EmployeeGun>();
            this.EmployeeInternationalPassports = new TestDbSet<EmployeeInternationalPassport>();
            this.EmployeeLanguages = new TestDbSet<EmployeeLanguage>();
            this.EmployeeLanguageTypes = new TestDbSet<EmployeeLanguageType>();
            this.EmployeeMilitaryCards = new TestDbSet<EmployeeMilitaryCard>();
            this.EmployeeOffices = new TestDbSet<EmployeeOffice>();
            this.EmployeePhotos = new TestDbSet<EmployeePhoto>();
            this.Employees = new TestDbSet<Employee>();
            this.FoundationDocuments = new TestDbSet<FoundationDocument>();
            this.GTIOrganizations = new TestDbSet<OrganizationGTI>();
            this.Languages = new TestDbSet<Language>();
            this.Localities = new TestDbSet<AddressLocality>();
            this.Offices = new TestDbSet<Office>();
            this.OrganizationAddresses = new TestDbSet<OrganizationAddress>();
            this.OrganizationAddressTypes = new TestDbSet<OrganizationAddressType>();
            this.OrganizationContactPersonContacts = new TestDbSet<OrganizationContactPersonContact>();
            this.OrganizationContactPersons = new TestDbSet<OrganizationContactPerson>();
            this.OrganizationContactPersonViews = new TestDbSet<OrganizationContactPersonView>();
            this.OrganizationGTILinks = new TestDbSet<OrganizationGTILink>();
            this.OrganizationLanguageNames = new TestDbSet<OrganizationLanguageName>();
            this.OrganizationLegalForms = new TestDbSet<OrganizationLegalForm>();
            this.OrganizationProperties = new TestDbSet<OrganizationProperty>();
            this.OrganizationPropertyTypeAliases = new TestDbSet<OrganizationPropertyTypeAlias>();
            this.OrganizationPropertyTypes = new TestDbSet<OrganizationPropertyType>();
            this.Organizations = new TestDbSet<Organization>();
            this.OrganizationTaxAddresses = new TestDbSet<OrganizationTaxAddress>();
            this.OrganizationTaxAddressTypes = new TestDbSet<OrganizationTaxAddressType>();
            this.Places = new TestDbSet<AddressPlace>();
            this.Professions = new TestDbSet<Profession>();
            this.Regions = new TestDbSet<AddressRegion>();
            this.Villages = new TestDbSet<AddressVillage>();
        }

        public void MarkAsModified(object entity)
        {
        }

        private Dictionary<string, int> TableIds = new Dictionary<string, int>();

        public int NewTableId(string tableName)
        {
            int value = 1;
            try
            {
                TableIds[tableName] = TableIds[tableName] + 1;
                value = TableIds[tableName];
            }
            catch (Exception)
            {
                TableIds[tableName] = 1;
            }
            
            return value;
        }



        public int SaveChanges()
        {
            return 0;
        }

        //public void MarkAsModified(Product item) { }
        public void Dispose()
        { }

        public System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity)
        {
            throw new NotImplementedException();
        }

        public System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<ContactType> ContactTypes { get; set; }

        public DbSet<Continent> Continents { get; set; }

        public DbSet<Country> Countries { get; set; }

        //public Database Database { get; set; }


        public DbSet<Department> Departments { get; set; }

        public DbSet<EducationStudyForm> EducationStudyForms { get; set; }

        public DbSet<EmployeeCar> EmployeeCars { get; set; }

        public DbSet<EmployeeContact> EmployeeContacts { get; set; }

        public DbSet<EmployeeDocumentScan> EmployeeDocumentScans { get; set; }

        public DbSet<EmployeeDrivingLicense> EmployeeDrivingLicenses { get; set; }

        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }

        public DbSet<EmployeeFoundationDocument> EmployeeFoundationDocuments { get; set; }

        public DbSet<EmployeeGun> EmployeeGun { get; set; }

        public DbSet<EmployeeInternationalPassport> EmployeeInternationalPassports { get; set; }

        public DbSet<EmployeeLanguage> EmployeeLanguages { get; set; }

        public DbSet<EmployeeLanguageType> EmployeeLanguageTypes { get; set; }

        public DbSet<EmployeeMilitaryCard> EmployeeMilitaryCards { get; set; }

        public DbSet<EmployeeOffice> EmployeeOffices { get; set; }


        public DbSet<EmployeePassport> EmployeePassports { get; set; }

        public DbSet<EmployeePhoto> EmployeePhotos { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<FoundationDocument> FoundationDocuments { get; set; }

        public DbSet<OrganizationGTI> GTIOrganizations { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<AddressLocality> Localities { get; set; }

        public DbSet<Office> Offices { get; set; }

        public DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        public DbSet<OrganizationAddressType> OrganizationAddressTypes { get; set; }


        public DbSet<OrganizationContactPersonContact> OrganizationContactPersonContacts { get; set; }


        public DbSet<OrganizationContactPerson> OrganizationContactPersons { get; set; }

        public DbSet<OrganizationContactPersonView> OrganizationContactPersonViews { get; set; }

        public DbSet<OrganizationGTILink> OrganizationGTILinks { get; set; }

        public DbSet<OrganizationLanguageName> OrganizationLanguageNames { get; set; }

        public DbSet<OrganizationLegalForm> OrganizationLegalForms { get; set; }

        public DbSet<OrganizationProperty> OrganizationProperties { get; set; }

        public DbSet<OrganizationPropertyTypeAlias> OrganizationPropertyTypeAliases { get; set; }

        public DbSet<OrganizationPropertyType> OrganizationPropertyTypes { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<OrganizationTaxAddress> OrganizationTaxAddresses { get; set; }

        public DbSet<OrganizationTaxAddressType> OrganizationTaxAddressTypes { get; set; }

        public DbSet<AddressPlace> Places { get; set; }

        public DbSet<Profession> Professions { get; set; }

        public DbSet<AddressRegion> Regions { get; set; }

        public DbSet<AddressVillage> Villages { get; set; }

        public bool DeleteDocumentScan(Guid scanId)
        {
            return true;
        }

        public IEnumerable<EmployeeDocumentScanDTO> EmployeeAllDocumentScans(int employeeId)
        {
            List<EmployeeDocumentScanDTO> list = new List<EmployeeDocumentScanDTO>();
            list.Add(new EmployeeDocumentScanDTO()
            {
                Id = 1,
                ScanName = "some path",
                TableId = 1,
                TableName = "EmployeePassport"
            });
            list.Add(new EmployeeDocumentScanDTO()
            {
                Id = 2,
                ScanName = "some path",
                TableId = 1,
                TableName = "EmployeePassport"
            });
            return list;
        }

        public IEnumerable<EmployeeView> EmployeeByOffices(IEnumerable<int> officeIds)
        {
            List<EmployeeView> list = new List<EmployeeView>();
            list.Add(new EmployeeView()
            {
                DateOfBirth = new DateTime(2012, 2, 1),
                Email = "someemail@mailemail.rrt",
                FirstName = "anyway",
                SecondName = "this are",
                Surname = "results of procedure"
            });
            list.Add(new EmployeeView()
            {
                Id = 1,
                FirstName = "Another",
                Surname = "sss"
            });
            return list;
        }

        public IEnumerable<EmployeeView> EmployeeFilter(string myFilter)
        {
            List<EmployeeView> list = new List<EmployeeView>();
            list.Add(new EmployeeView()
            {
                DateOfBirth = new DateTime(2012, 2, 1),
                Email = "someemail@mailemail.rrt",
                FirstName = "anyway",
                SecondName = "this are",
                Surname = "results of procedure"
            });
            list.Add(new EmployeeView()
            {
                Id = 1,
                FirstName = "Another",
                Surname = "sss"
            });
            return list;
        }


        public int FileNameUnique()
        {
            return 42;
        }

        public DealContainerViewDTO GetContainer(Guid id)
        {
            return new DealContainerViewDTO()
            {
                Seal = "seal",
                Container = "sssss",
                Id = id
            };
        }

        public IEnumerable<DealContainerViewDTO> GetContainersByDeal(Guid dealId)
        {
            List<DealContainerViewDTO> list = new List<DealContainerViewDTO>();
            list.Add(new DealContainerViewDTO()
            {
               Seal = "ssss"
            });
            list.Add(new DealContainerViewDTO()
            {
                Seal = "sss"
            });
            return list;
        }

        public IEnumerable<InvoiceContainerViewDTO> GetContainersByInvoiceId(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DealContainerViewDTO> GetContainersFiltered(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            throw new NotImplementedException();
        }

        public DealFullViewDTO GetDealCardInfo(Guid dealId)
        {
            throw new NotImplementedException();
        }

        public DocumentScanDTO GetDealDocumentScanById(Guid scanId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DealViewDTO> GetDealsFiltered(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocumentScanDTO> GetDocumentScanByDeal(Guid dealId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocumentScanTypeDTO> GetDocumentScanTypes()
        {
            throw new NotImplementedException();
        }

        public InvoiceFullViewDTO GetInvoiceCardInfo(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceLineViewDTO> GetInvoiceLinesByInvoice(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DealInvoiceViewDTO> GetInvoicesByDeal(Guid dealId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DealInvoiceViewDTO> GetInvoicesList(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrganizationGTIShortDTO> GetOrganizationGTIByOrganization(int organizationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrganizationView> GetOrganizationsByOffices(IEnumerable<int> officeIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrganizationView> GetOrganizationsFilter(string myFilter)
        {
            throw new NotImplementedException();
        }

        public Guid InsertDealDocumentScan(Guid dealId, byte[] fileContent, string fileName, string email, int documentScanTypeId)
        {
            throw new NotImplementedException();
        }

        public bool IsEmployeeInformationFilled(int employeeId)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<OrganizationSearchDTO> SearchOrganization(int countryId, string registrationNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrganizationGTI> SearchOrganizationGTI(IEnumerable<int> officeIds, string registrationNumber)
        {
            throw new NotImplementedException();
        }

        public DocumentScanDTO UpdateDocumentScanType(Guid scanId, int documentScanTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
