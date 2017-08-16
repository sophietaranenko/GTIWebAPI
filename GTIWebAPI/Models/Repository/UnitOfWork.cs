using GTIWebAPI.Exceptions;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Reports.ProductivityReport;
using GTIWebAPI.Models.Sales;
using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public class UnitOfWork : IDisposable, IGetNewTableId
    {
        private IAppDbContext context;

        public UnitOfWork(IDbContextFactory factory)
        {
            this.context = factory.CreateDbContext();
        }


        public string GetChanges(object myObject)
        {
            return context.GetChanges(myObject);

        }


        //UserImages 
        private GenericRepository<UserImage> userImagesRepository;
        public GenericRepository<UserImage> UserImagesRepository
        {
            get
            {
                if (this.userImagesRepository == null)
                {
                    this.userImagesRepository = new GenericRepository<UserImage>(context);
                }
                return userImagesRepository;
            }
        }



        ////UserRights Block

        //private GenericRepository<UserRight> userRightsRepository;
        //public GenericRepository<UserRight> UserRightsRepository
        //{
        //    get
        //    {
        //        if (this.userRightsRepository == null)
        //        {
        //            this.userRightsRepository = new GenericRepository<UserRight>(context);
        //        }
        //        return userRightsRepository;
        //    }
        //}


        private GenericRepository<UserRightMask> userRightMasksRepository;
        public GenericRepository<UserRightMask> UserRightMasksRepository
        {
            get
            {
                if (this.userRightMasksRepository == null)
                {
                    this.userRightMasksRepository = new GenericRepository<UserRightMask>(context);
                }
                return userRightMasksRepository;
            }
        }


        private GenericRepository<UserRightMaskRight> userRightMaskRightsRepository;
        public GenericRepository<UserRightMaskRight> UserRightMaskRightsRepository
        {
            get
            {
                if (this.userRightMaskRightsRepository == null)
                {
                    this.userRightMaskRightsRepository = new GenericRepository<UserRightMaskRight>(context);
                }
                return userRightMaskRightsRepository;
            }
        }


        private GenericRepository<RightControllerAction> actionsRepository;
        public GenericRepository<RightControllerAction> ActionsRepository
        {
            get
            {
                if (this.actionsRepository == null)
                {
                    this.actionsRepository = new GenericRepository<RightControllerAction>(context);
                }
                return actionsRepository;
            }
        }


        private GenericRepository<RightController> controllersRepository;
        public GenericRepository<RightController> ControllersRepository
        {
            get
            {
                if (this.controllersRepository == null)
                {
                    this.controllersRepository = new GenericRepository<RightController>(context);
                }
                return controllersRepository;
            }
        }



        //Employee

        private GenericRepository<EmployeeCar> employeeCarsRepository;
        public GenericRepository<EmployeeCar> EmployeeCarsRepository
        {
            get
            {
                if (this.employeeCarsRepository == null)
                {
                    this.employeeCarsRepository = new GenericRepository<EmployeeCar>(context);
                }
                return employeeCarsRepository;
            }
        }

        private GenericRepository<EmployeeContact> employeeContactsRepository;
        public GenericRepository<EmployeeContact> EmployeeContactsRepository
        {
            get
            {
                if (this.employeeContactsRepository == null)
                {
                    this.employeeContactsRepository = new GenericRepository<EmployeeContact>(context);
                }
                return employeeContactsRepository;
            }
        }


        private GenericRepository<EmployeeDocumentScan> employeeDocumentScansRepository;
        public GenericRepository<EmployeeDocumentScan> EmployeeDocumentScansRepository
        {
            get
            {
                if (this.employeeDocumentScansRepository == null)
                {
                    this.employeeDocumentScansRepository = new GenericRepository<EmployeeDocumentScan>(context);
                }
                return employeeDocumentScansRepository;
            }
        }

        private GenericRepository<EmployeeDrivingLicense> employeeDrivingLicensesRepository;
        public GenericRepository<EmployeeDrivingLicense> EmployeeDrivingLicensesRepository
        {
            get
            {
                if (this.employeeDrivingLicensesRepository == null)
                {
                    this.employeeDrivingLicensesRepository = new GenericRepository<EmployeeDrivingLicense>(context);
                }
                return employeeDrivingLicensesRepository;
            }
        }

        private GenericRepository<EmployeeEducation> employeeEducationsRepository;
        public GenericRepository<EmployeeEducation> EmployeeEducationsRepository
        {
            get
            {
                if (this.employeeEducationsRepository == null)
                {
                    this.employeeEducationsRepository = new GenericRepository<EmployeeEducation>(context);
                }
                return employeeEducationsRepository;
            }
        }

        private GenericRepository<EmployeeFoundationDocument> employeeFoundationDocumentsRepository;
        public GenericRepository<EmployeeFoundationDocument> EmployeeFoundationDocumentsRepository
        {
            get
            {
                if (this.employeeFoundationDocumentsRepository == null)
                {
                    this.employeeFoundationDocumentsRepository = new GenericRepository<EmployeeFoundationDocument>(context);
                }
                return employeeFoundationDocumentsRepository;
            }
        }

        private GenericRepository<EmployeeGun> employeeGunsRepository;
        public GenericRepository<EmployeeGun> EmployeeGunsRepository
        {
            get
            {
                if (this.employeeGunsRepository == null)
                {
                    this.employeeGunsRepository = new GenericRepository<EmployeeGun>(context);
                }
                return employeeGunsRepository;
            }
        }

        private GenericRepository<EmployeeInternationalPassport> employeeInternationalPassportsRepository;
        public GenericRepository<EmployeeInternationalPassport> EmployeeInternationalPassportsRepository
        {
            get
            {
                if (this.employeeInternationalPassportsRepository == null)
                {
                    this.employeeInternationalPassportsRepository = new GenericRepository<EmployeeInternationalPassport>(context);
                }
                return employeeInternationalPassportsRepository;
            }
        }

        private GenericRepository<EmployeeLanguage> employeeLanguagesRepository;
        public GenericRepository<EmployeeLanguage> EmployeeLanguagesRepository
        {
            get
            {
                if (this.employeeLanguagesRepository == null)
                {
                    this.employeeLanguagesRepository = new GenericRepository<EmployeeLanguage>(context);
                }
                return employeeLanguagesRepository;
            }
        }

        private GenericRepository<EmployeeMilitaryCard> employeeMilitaryCardsRepository;
        public GenericRepository<EmployeeMilitaryCard> EmployeeMilitaryCardsRepository
        {
            get
            {
                if (this.employeeMilitaryCardsRepository == null)
                {
                    this.employeeMilitaryCardsRepository = new GenericRepository<EmployeeMilitaryCard>(context);
                }
                return employeeMilitaryCardsRepository;
            }
        }

        private GenericRepository<EmployeeOffice> employeeOfficesRepository;
        public GenericRepository<EmployeeOffice> EmployeeOfficesRepository
        {
            get
            {
                if (this.employeeOfficesRepository == null)
                {
                    this.employeeOfficesRepository = new GenericRepository<EmployeeOffice>(context);
                }
                return employeeOfficesRepository;
            }
        }


        private GenericRepository<EmployeePassport> employeePassportsRepository;
        public GenericRepository<EmployeePassport> EmployeePassportsRepository
        {
            get
            {
                if (this.employeePassportsRepository == null)
                {
                    this.employeePassportsRepository = new GenericRepository<EmployeePassport>(context);
                }
                return employeePassportsRepository;
            }
        }

        private GenericRepository<Employee> employeesRepository;
        public GenericRepository<Employee> EmployeesRepository
        {
            get
            {
                if (this.employeesRepository == null)
                {
                    this.employeesRepository = new GenericRepository<Employee>(context);
                }
                return employeesRepository;
            }
        }




        //Справочники (дополнить) 


        private GenericRepository<Address> addressesRepository;
        public GenericRepository<Address> AddressesRepository
        {
            get
            {
                if (this.addressesRepository == null)
                {
                    this.addressesRepository = new GenericRepository<Address>(context);
                }
                return addressesRepository;
            }
        }

        private GenericRepository<Profession> professionsRepository;
        public GenericRepository<Profession> ProfessionsRepository
        {
            get
            {
                if (this.professionsRepository == null)
                {
                    this.professionsRepository = new GenericRepository<Profession>(context);
                }
                return professionsRepository;
            }
        }

        private GenericRepository<Office> officesRepository;
        public GenericRepository<Office> OfficesRepository
        {
            get
            {
                if (this.officesRepository == null)
                {
                    this.officesRepository = new GenericRepository<Office>(context);
                }
                return officesRepository;
            }
        }

        private GenericRepository<Department> departmentsRepository;
        public GenericRepository<Department> DepartmentsRepository
        {
            get
            {
                if (this.departmentsRepository == null)
                {
                    this.departmentsRepository = new GenericRepository<Department>(context);
                }
                return departmentsRepository;
            }
        }

        private GenericRepository<KPIValue> kPIValuesRepository;
        public GenericRepository<KPIValue> KPIValuesRepository
        {
            get
            {
                if (this.kPIValuesRepository == null)
                {
                    this.kPIValuesRepository = new GenericRepository<KPIValue>(context);
                }
                return kPIValuesRepository;
            }
        }

        private GenericRepository<KPIParameter> kPIParametersRepository;
        public GenericRepository<KPIParameter> KPIParametersRepository
        {
            get
            {
                if (this.kPIParametersRepository == null)
                {
                    this.kPIParametersRepository = new GenericRepository<KPIParameter>(context);
                }
                return kPIParametersRepository;
            }
        }

        private GenericRepository<KPIPeriod> kPIPeriodsRepository;
        public GenericRepository<KPIPeriod> KPIPeriodsRepository
        {
            get
            {
                if (this.kPIPeriodsRepository == null)
                {
                    this.kPIPeriodsRepository = new GenericRepository<KPIPeriod>(context);
                }
                return kPIPeriodsRepository;
            }
        }

        private GenericRepository<UserRightOff> userRightsRepository;
        public GenericRepository<UserRightOff> UserRightsRepository
        {
            get
            {
                if (this.userRightsRepository == null)
                {
                    this.userRightsRepository = new GenericRepository<UserRightOff>(context);
                }
                return userRightsRepository;
            }
        }

        //все данные для, например, EmployeeList будут должго грузится - будут создаваться репозитории
        //надо как-то объединить эти вещи, либо раздавать непосредственно из UnitOfWork, что тоже не очень гуд 
        //сначала сделать правильно, потестить производительность, потом уже что-то решать 
        public EmployeeList CreateEmployeeList()
        {
            EmployeeList list = new EmployeeList();
            try
            {
                list.AddressList = AddressList.CreateAddressList(context);
                list.EmployeeLanguageList = EmployeeLanguageList.CreateEmployeeLanguageList(context);

                list.EmployeeOfficeList = new EmployeeOfficeList();

                list.EmployeeOfficeList.Offices = context.Offices.ToList().Select(d => d.ToDTO());
                list.EmployeeOfficeList.Professions = context.Professions.Where(d => d.Deleted != true).ToList().Select(d => d.ToDTO());
                list.EmployeeOfficeList.Departments = context.Departments.Where(d => d.Deleted != true).ToList().Select(d => d.ToDTO());

                list.ContactTypes = context.ContactTypes.Where(d => d.Deleted != true).ToList().Select(d => d.ToDTO());
                list.FoundationDocuments = context.FoundationDocuments.Where(d => d.Deleted != true).ToList().Select(d => d.ToDTO());
                list.EducationStudyForms = context.EducationStudyForms.ToList().Select(d => d.ToDTO());
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }
            return list;
        }

        public OrganizationList CreateOrganizationList()
        {
            return OrganizationList.CreateOrganizationList(context);
        }


        //Organization
        private GenericRepository<OrganizationAddress> organizationAddressesRepository;
        public GenericRepository<OrganizationAddress> OrganizationAddressesRepository
        {
            get
            {
                if (this.organizationAddressesRepository == null)
                {
                    this.organizationAddressesRepository = new GenericRepository<OrganizationAddress>(context);
                }
                return organizationAddressesRepository;
            }
        }

        private GenericRepository<OrganizationTaxAddress> organizationTaxAddressesRepository;
        public GenericRepository<OrganizationTaxAddress> OrganizationTaxAddressesRepository
        {
            get
            {
                if (this.organizationTaxAddressesRepository == null)
                {
                    this.organizationTaxAddressesRepository = new GenericRepository<OrganizationTaxAddress>(context);
                }
                return organizationTaxAddressesRepository;
            }
        }

        private GenericRepository<OrganizationContactPersonContact> organizationContactPersonContactsRepository;
        public GenericRepository<OrganizationContactPersonContact> OrganizationContactPersonContactsRepository
        {
            get
            {
                if (this.organizationContactPersonContactsRepository == null)
                {
                    this.organizationContactPersonContactsRepository = new GenericRepository<OrganizationContactPersonContact>(context);
                }
                return organizationContactPersonContactsRepository;
            }
        }

        private GenericRepository<OrganizationContactPerson> organizationContactPersonsRepository;
        public GenericRepository<OrganizationContactPerson> OrganizationContactPersonsRepository
        {
            get
            {
                if (this.organizationContactPersonsRepository == null)
                {
                    this.organizationContactPersonsRepository = new GenericRepository<OrganizationContactPerson>(context);
                }
                return organizationContactPersonsRepository;
            }
        }

        private GenericRepository<OrganizationContactPersonView> organizationContactPersonViewsRepository;
        public GenericRepository<OrganizationContactPersonView> OrganizationContactPersonsViewRepository
        {
            get
            {
                if (this.organizationContactPersonViewsRepository == null)
                {
                    this.organizationContactPersonViewsRepository = new GenericRepository<OrganizationContactPersonView>(context);
                }
                return organizationContactPersonViewsRepository;
            }
        }

        private GenericRepository<OrganizationGTILink> organizationGTILinksRepository;
        public GenericRepository<OrganizationGTILink> OrganizationGTILinksRepository
        {
            get
            {
                if (this.organizationGTILinksRepository == null)
                {
                    this.organizationGTILinksRepository = new GenericRepository<OrganizationGTILink>(context);
                }
                return organizationGTILinksRepository;
            }
        }

        private GenericRepository<OrganizationGTI> organizationGTIsRepository;
        public GenericRepository<OrganizationGTI> OrganizationGTIsRepository
        {
            get
            {
                if (this.organizationGTIsRepository == null)
                {
                    this.organizationGTIsRepository = new GenericRepository<OrganizationGTI>(context);
                }
                return organizationGTIsRepository;
            }
        }

        private GenericRepository<OrganizationLanguageName> organizationLanguageNamesRepository;
        public GenericRepository<OrganizationLanguageName> OrganizationLanguageNamesRepository
        {
            get
            {
                if (this.organizationLanguageNamesRepository == null)
                {
                    this.organizationLanguageNamesRepository = new GenericRepository<OrganizationLanguageName>(context);
                }
                return organizationLanguageNamesRepository;
            }
        }

        private GenericRepository<OrganizationProperty> organizationPropertiesRepository;
        public GenericRepository<OrganizationProperty> OrganizationPropertiesRepository
        {
            get
            {
                if (this.organizationPropertiesRepository == null)
                {
                    this.organizationPropertiesRepository = new GenericRepository<OrganizationProperty>(context);
                }
                return organizationPropertiesRepository;
            }
        }

        private GenericRepository<OrganizationPropertyType> organizationPropertyTypesRepository;
        public GenericRepository<OrganizationPropertyType> OrganizationPropertyTypesRepository
        {
            get
            {
                if (this.organizationPropertyTypesRepository == null)
                {
                    this.organizationPropertyTypesRepository = new GenericRepository<OrganizationPropertyType>(context);
                }
                return organizationPropertyTypesRepository;
            }
        }

        private GenericRepository<Organizations.Organization> organizationsRepository;
        public GenericRepository<Organizations.Organization> OrganizationsRepository
        {
            get
            {
                if (this.organizationsRepository == null)
                {
                    this.organizationsRepository = new GenericRepository<Organizations.Organization>(context);
                }
                return organizationsRepository;
            }
        }




        private GenericRepository<Organizations.OrganizationOwner> organizationOwnersRepository;
        public GenericRepository<Organizations.OrganizationOwner> OrganizationOwnersRepository
        {
            get
            {
                if (this.organizationOwnersRepository == null)
                {
                    this.organizationOwnersRepository = new GenericRepository<Organizations.OrganizationOwner>(context);
                }
                return organizationOwnersRepository;
            }
        }

        //Sales 

        private GenericRepository<Act> actsRepository;
        public GenericRepository<Act> ActsRepository
        {
            get
            {
                if (this.actsRepository == null)
                {
                    this.actsRepository = new GenericRepository<Act>(context);
                }
                return actsRepository;
            }
        }

        private GenericRepository<Interaction> interactionsRepository;
        public GenericRepository<Interaction> InteractionsRepository
        {
            get
            {
                if (this.interactionsRepository == null)
                {
                    this.interactionsRepository = new GenericRepository<Interaction>(context);
                }
                return interactionsRepository;
            }
        }

        private GenericRepository<InteractionAct> interactionActsRepository;
        public GenericRepository<InteractionAct> InteractionActsRepository
        {
            get
            {
                if (this.interactionActsRepository == null)
                {
                    this.interactionActsRepository = new GenericRepository<InteractionAct>(context);
                }
                return interactionActsRepository;
            }
        }

        private GenericRepository<InteractionActMember> interactionActMembersRepository;
        public GenericRepository<InteractionActMember> InteractionActMembersRepository
        {
            get
            {
                if (this.interactionActMembersRepository == null)
                {
                    this.interactionActMembersRepository = new GenericRepository<InteractionActMember>(context);
                }
                return interactionActMembersRepository;
            }
        }

        private GenericRepository<InteractionMember> interactionMembersRepository;
        public GenericRepository<InteractionMember> InteractionMembersRepository
        {
            get
            {
                if (this.interactionMembersRepository == null)
                {
                    this.interactionMembersRepository = new GenericRepository<InteractionMember>(context);
                }
                return interactionMembersRepository;
            }
        }

        private GenericRepository<InteractionActOrganizationMember> interactionActOrganizationMembersRepository;
        public GenericRepository<InteractionActOrganizationMember> InteractionActOrganizationMembersRepository
        {
            get
            {
                if (this.interactionActOrganizationMembersRepository == null)
                {
                    this.interactionActOrganizationMembersRepository = new GenericRepository<InteractionActOrganizationMember>(context);
                }
                return interactionActOrganizationMembersRepository;
            }
        }

        private GenericRepository<Sales.Task> tasksRepository;
        public GenericRepository<Sales.Task> TasksRepository
        {
            get
            {
                if (this.tasksRepository == null)
                {
                    this.tasksRepository = new GenericRepository<Sales.Task>(context);
                }
                return tasksRepository;
            }
        }

        private GenericRepository<TaskMember> taskMembersRepository;
        public GenericRepository<TaskMember> TaskMembersRepository
        {
            get
            {
                if (this.taskMembersRepository == null)
                {
                    this.taskMembersRepository = new GenericRepository<TaskMember>(context);
                }
                return taskMembersRepository;
            }
        }

        private GenericRepository<TaskMemberRole> taskMemberRolesRepository;
        public GenericRepository<TaskMemberRole> TaskMemberRolesRepository
        {
            get
            {
                if (this.taskMemberRolesRepository == null)
                {
                    this.taskMemberRolesRepository = new GenericRepository<TaskMemberRole>(context);
                }
                return taskMemberRolesRepository;
            }
        }

        private GenericRepository<Country> countriesRepository;
        public GenericRepository<Country> CountriesRepository
        {
            get
            {
                if (this.countriesRepository == null)
                {
                    this.countriesRepository = new GenericRepository<Country>(context);
                }
                return countriesRepository;
            }
        }

        private GenericRepository<InteractionStatusMovement> interactionStatusMovementsRepository;
        public GenericRepository<InteractionStatusMovement> InteractionStatusMovementsRepository
        {
            get
            {
                if (this.interactionStatusMovementsRepository == null)
                {
                    this.interactionStatusMovementsRepository = new GenericRepository<InteractionStatusMovement>(context);
                }
                return interactionStatusMovementsRepository;
            }
        }

        private GenericRepository<EmployeeInsurance> employeeInsurancesRepository;
        public GenericRepository<EmployeeInsurance> EmployeeInsurancesRepository
        {
            get
            {
                if (this.employeeInsurancesRepository == null)
                {
                    this.employeeInsurancesRepository = new GenericRepository<EmployeeInsurance>(context);
                }
                return employeeInsurancesRepository;
            }
        }

        private GenericRepository<Notification> notificationsRepository;
        public GenericRepository<Notification> NotificationsRepository
        {
            get
            {
                if (this.notificationsRepository == null)
                {
                    this.notificationsRepository = new GenericRepository<Notification>(context);
                }
                return notificationsRepository;
            }
        }

        private GenericRepository<NotificationRecipient> notificationRecipientsRepository;
        public GenericRepository<NotificationRecipient> NotificationRecipientsRepository
        {
            get
            {
                if (this.notificationRecipientsRepository == null)
                {
                    this.notificationRecipientsRepository = new GenericRepository<NotificationRecipient>(context);
                }
                return notificationRecipientsRepository;
            }
        }





        /// <summary>
        /// For using Stored Procedures throw UnitOfWork
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> SQLQuery<T>(string sql, params object[] parameters)
        {
            return context.ExecuteStoredProcedure<T>(sql, parameters);
        }

        public Task<IEnumerable<T>> SQLQueryAsync<T>(string sql, params object[] parameters) where T : class
        {
            return context.ExecuteStoredProcedureAsync<T>(sql, parameters);
        }

        public void DoSomething()
        {
            //  var myObjectState = 
        }

        /// <summary>
        /// Stored Procedure call 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Int32 GetNewTableId(string tableName)
        {
            Int32 result = 0;
            SqlParameter parTableName = new SqlParameter
            {
                ParameterName = "@TableName",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                Value = tableName
            };
            try
            {
                result = context.ExecuteStoredProcedure<Int32>("exec NewTableId @TableName", parTableName).FirstOrDefault();
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }
            return result;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
