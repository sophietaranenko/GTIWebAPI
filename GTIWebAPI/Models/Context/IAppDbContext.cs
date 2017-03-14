using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IAppDbContext : IDisposable, IServiceDbContext, IDbContextAddress, IDbContextEmployeeLanguage, IDbContextOrganization
    {

        //string CreateConnectionString(IPrincipal user);
        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();

        void MarkAsModified(object entity);

        DbSet<Address> Addresses { get; set; }

        DbSet<AddressLocality> Localities { get; set; }

        DbSet<AddressPlace> Places { get; set; }

        DbSet<AddressRegion> Regions { get; set; }

        DbSet<AddressVillage> Villages { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<Continent> Continents { get; set; }

        DbSet<Employee> Employees { get; set; }

        DbSet<EmployeeCar> EmployeeCars { get; set; }

        DbSet<EmployeeDocumentScan> EmployeeDocumentScans { get; set; }

        DbSet<EmployeeDrivingLicense> EmployeeDrivingLicenses { get; set; }

        DbSet<EmployeeEducation> EmployeeEducations { get; set; }

        DbSet<EmployeeInternationalPassport> EmployeeInternationalPassports { get; set; }

        DbSet<EmployeeLanguage> EmployeeLanguages { get; set; }

        DbSet<EmployeeMilitaryCard> EmployeeMilitaryCards { get; set; }

        DbSet<EmployeePassport> EmployeePassports { get; set; }

        DbSet<EmployeePhoto> EmployeePhotos { get; set; }

        DbSet<EmployeeContact> EmployeeContacts { get; set; }

        DbSet<EmployeeOffice> EmployeeOffices { get; set; }

        DbSet<EmployeeFoundationDocument> EmployeeFoundationDocuments { get; set; }

        DbSet<EmployeeGun> EmployeeGun { get; set; }

        DbSet<Language> Languages { get; set; }

        DbSet<OrganizationGTI> GTIOrganizations { get; set; }

        DbSet<ContactType> ContactTypes { get; set; }

        DbSet<Office> Offices { get; set; }

        DbSet<Profession> Professions { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<FoundationDocument> FoundationDocuments { get; set; }

        DbSet<EducationStudyForm> EducationStudyForms { get; set; }

        DbSet<EmployeeLanguageType> EmployeeLanguageTypes { get; set; }

        DbSet<Organization> Organizations { get; set; }

        DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        DbSet<OrganizationAddressType> OrganizationAddressTypes { get; set; }

        DbSet<OrganizationContactPerson> OrganizationContactPersons { get; set; }

        DbSet<OrganizationContactPersonView> OrganizationContactPersonViews { get; set; }

        DbSet<OrganizationContactPersonContact> OrganizationContactPersonContacts { get; set; }

        DbSet<OrganizationGTILink> OrganizationGTILinks { get; set; }

        DbSet<OrganizationProperty> OrganizationProperties { get; set; }

        DbSet<OrganizationPropertyType> OrganizationPropertyTypes { get; set; }

        DbSet<OrganizationPropertyTypeAlias> OrganizationPropertyTypeAliases { get; set; }

        DbSet<OrganizationLegalForm> OrganizationLegalForms { get; set; }

        DbSet<OrganizationTaxAddress> OrganizationTaxAddresses { get; set; }

        DbSet<OrganizationTaxAddressType> OrganizationTaxAddressTypes { get; set; }

        DbSet<OrganizationLanguageName> OrganizationLanguageNames { get; set; }

        bool IsEmployeeInformationFilled(int employeeId);

        int FileNameUnique();

        int NewTableId(string tableName);

        IEnumerable<EmployeeView> EmployeeFilter(string myFilter);

        IEnumerable<EmployeeView> EmployeeByOffices(IEnumerable<int> officeIds);

        IEnumerable<EmployeeDocumentScan> EmployeeAllDocumentScans(int employeeId);

        IEnumerable<OrganizationView> GetOrganizationsFilter(string myFilter);

        IEnumerable<OrganizationView> GetOrganizationsByOffices(IEnumerable<int> officeIds);

        IEnumerable<OrganizationGTI> SearchOrganizationGTI(IEnumerable<int> officeIds, string registrationNumber);

        IEnumerable<OrganizationSearchDTO> SearchOrganization(int countryId, string registrationNumber);

        IEnumerable<DealViewDTO> GetDealsFiltered(int organizationId, DateTime? dateBegin, DateTime? dateEnd);

        DealFullViewDTO GetDealCardInfo(Guid dealId);

        IEnumerable<DealContainerViewDTO> GetContainersByDeal(Guid dealId);

        IEnumerable<DealInvoiceViewDTO> GetInvoicesByDeal(Guid dealId);

        IEnumerable<DealInvoiceViewDTO> GetInvoicesList(int organizationId, DateTime? dateBegin, DateTime? dateEnd);

        InvoiceFullViewDTO GetInvoiceCardInfo(int invoiceId);

        IEnumerable<InvoiceLineViewDTO> GetInvoiceLinesByInvoice(int invoiceId);

        IEnumerable<InvoiceContainerViewDTO> GetContainersByInvoiceId(int invoiceId);

        IEnumerable<DealContainerViewDTO> GetContainersFiltered(int organizationId, DateTime? dateBegin, DateTime? dateEnd);

        DealContainerViewDTO GetContainer(Guid id);

        IEnumerable<DocumentScanTypeDTO> GetDocumentScanTypes();

        IEnumerable<DocumentScanDTO> GetDocumentScanByDeal(Guid dealId);

        IEnumerable<OrganizationGTIShortDTO> GetOrganizationGTIByOrganization(int organizationId);

        Guid InsertDealDocumentScan(Guid dealId, byte[] fileContent, string fileName, string email, int documentScanTypeId);

        DocumentScanDTO UpdateDocumentScanType(Guid scanId, int documentScanTypeId);

        bool DeleteDocumentScan(Guid scanId);

        DocumentScanDTO GetDealDocumentScanById(Guid scanId);
    }
}
