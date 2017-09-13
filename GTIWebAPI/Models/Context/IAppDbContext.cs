using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Sales;
using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public interface IAppDbContext : IDisposable, IDbContextAddress, IDbContextEmployeeLanguage, IDbContextOrganization
    {
        IEnumerable<T> ExecuteStoredProcedure<T>(string query, params object[] parameters);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string query, params object[] parameters) where TEntity : class;

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

        IDbSet<EmployeeCar> EmployeeCars { get; set; }

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

        DbSet<EmployeeGun> EmployeeGuns { get; set; }

        DbSet<EmployeeInsurance> EmployeeInsurances { get; set; }

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

        DbSet<OrganizationOwner> OrganizationOwners { get; set; }

        DbSet<UserImage> UserImages { get; set; }

        DbSet<UserRightOff> UserRigths { get; set; }

        bool CreateOrganization(string email, string password);

        bool CreateHoldingUser(string email, string password);

        string GetChanges(object obj);

        //Sales

        DbSet<Act> Act { get; set; }

        DbSet<Interaction> Interaction { get; set; }

        DbSet<InteractionAct> InteractionAct { get; set; }

        DbSet<InteractionActMember> InteractionActMember { get; set; }

        DbSet<InteractionStatus> InteractionStatuses { get; set; }

        DbSet<InteractionBrokenReason> InteractionBrokenReasons { get; set; }

        DbSet<Tasks.Task> Task { get; set; }

        ObjectStateManager ObjectStateManager { get; set; }
    }
}
