using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Exceptions;
using GTIWebAPI.Models.Context;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller manipulating employees
    /// </summary>
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        private IDbContextFactory factory;

        public EmployeesController()
        {
            factory = new DbContextFactory();
        }

        public EmployeesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeView>))]
        public async Task<IHttpActionResult> GetEmployeeAll(string officeIds)
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("Value");
            foreach (var id in OfficeIds)
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
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeView> employees = await unitOfWork.SQLQueryAsync<EmployeeView>("exec EmployeeByOfficeIds @OfficeIds", parameter);
                if (employees == null)
                {
                    return NotFound();
                }                
             //   IEnumerable<EmployeeViewDTO> dtos = employees.Select(d => d.ToDTO());
                return Ok(employees);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get employee full information (with driving licenses, passports, etc.) 
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetView")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult GetEmployeeView(int id)
        {
            //провобала делать асинхронные, получается медленнее 
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                //Employee
                Employee employee = 
                employee = unitOfWork.EmployeesRepository
                .Get(d => d.Id == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country")
                .FirstOrDefault();
                if (employee == null)
                {
                    throw new NotFoundException();
                }

                ////EmployeeProfilePicture
                SqlParameter pEmployeeId = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = id
                };
                employee.ProfilePicture = unitOfWork.SQLQuery<string>("exec GetProfilePicturePathByEmployeeId @EmployeeId ", pEmployeeId).FirstOrDefault();
                // Task<string> tProfilePicture = SetProfilePicture(id, unitOfWork);


                //Error: SqlParameter уже содержится в другом SqlParameterCollection 
                SqlParameter pEmployeeId1 = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = id
                };
                //AspNetUserName 
                employee.FullUserName = unitOfWork.SQLQuery<string>("exec GetFullAspNetUserNameByEmployeeId @EmployeeId ", pEmployeeId1).FirstOrDefault();
                //   Task<string> tUserName = SetUserName(id, unitOfWork);


                employee.EmployeeOffices = unitOfWork.EmployeeOfficesRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "Department,Office,Profession");
                //Task<IEnumerable<EmployeeOffice>> tOffice = SetOffices(id, unitOfWork);

                employee.EmployeePassports = unitOfWork.EmployeePassportsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country");
                //  var tPassport = SetPassports(id, unitOfWork);

                employee.EmployeeMilitaryCards = unitOfWork.EmployeeMilitaryCardsRepository.Get(d => d.Deleted != true && d.EmployeeId == id);
                //  var tMCard = SetMilitaryCards(id, unitOfWork);

                employee.EmployeeLanguages = unitOfWork.EmployeeLanguagesRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "EmployeeLanguageType,Language");
                // var tLanguage = SetLanguages(id, unitOfWork);

                employee.EmployeeInternationalPassports = unitOfWork.EmployeeInternationalPassportsRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == id);
                // var tIntPassport = SetInternationalPassports(id, unitOfWork);

                employee.EmployeeGuns = unitOfWork.EmployeeGunsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id);
                // var tGun = SetGuns(id, unitOfWork);


                employee.EmployeeFoundationDocuments = unitOfWork.EmployeeFoundationDocumentsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "FoundationDocument");
                // var tFDoc = SetFoundationDocuments(id, unitOfWork);

                employee.EmployeeEducations = unitOfWork.EmployeeEducationsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "EducationStudyForm");
                // var tEdu = SetEducations(id, unitOfWork);

                employee.EmployeeDrivingLicenses = unitOfWork.EmployeeDrivingLicensesRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id);
                //  var tDL = SetDrivingLicenses(id, unitOfWork);

                employee.EmployeeContacts = unitOfWork.EmployeeContactsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "ContactType");
                // var tContact = SetContacts(id, unitOfWork);

                employee.EmployeeCars = unitOfWork.EmployeeCarsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id);

                // var tCar = SetCars(id, unitOfWork);

                //await Task.WhenAll(tOffice, tPassport, tMCard, tCar, tDL, tEdu, tFDoc, tGun, tIntPassport, tProfilePicture, tUserName, tContact, tLanguage);

                //employee.EmployeeCars = tCar.Result;
                //employee.EmployeeContacts = tContact.Result;
                //employee.EmployeeDrivingLicenses = tDL.Result;
                //employee.EmployeeEducations = tEdu.Result;
                //employee.EmployeeFoundationDocuments = tFDoc.Result;
                //employee.EmployeeGuns = tGun.Result;
                //employee.EmployeeInternationalPassports = tIntPassport.Result;
                //employee.EmployeeLanguages = tLanguage.Result;
                //employee.EmployeeMilitaryCards = tMCard.Result;
                //employee.EmployeeOffices = tOffice.Result;
                //employee.EmployeePassports = tPassport.Result;
                //employee.FullUserName = tUserName.Result;
                //employee.ProfilePicture = tProfilePicture.Result;


                EmployeeDTO dto = employee.ToDTOView();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task<string> SetProfilePicture(int id, UnitOfWork unitOfWork)
        {
            SqlParameter pEmployeeId = new SqlParameter
            {
                ParameterName = "@EmployeeId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = id
            };
            return unitOfWork.SQLQuery<string>("exec GetProfilePicturePathByEmployeeId @EmployeeId ", pEmployeeId).FirstOrDefault();
        }

        private async Task<string> SetUserName(int id, UnitOfWork unitOfWork)
        {
            SqlParameter pEmployeeId1 = new SqlParameter
            {
                ParameterName = "@EmployeeId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = id
            };
            return unitOfWork.SQLQuery<string>("exec GetFullAspNetUserNameByEmployeeId @EmployeeId ", pEmployeeId1).FirstOrDefault();
        }

        private async Task<IEnumerable<EmployeeOffice>> SetOffices(int id, UnitOfWork unitOfWork)
        { 
            return unitOfWork.EmployeeOfficesRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "Department,Office,Profession");
        }

        private async Task<IEnumerable<EmployeePassport>> SetPassports(int id, UnitOfWork unitOfWork)
        { 
            return unitOfWork.EmployeePassportsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country");
        }

        private async Task<IEnumerable<EmployeeMilitaryCard>> SetMilitaryCards(int id, UnitOfWork unitOfWork)
        { 
            return unitOfWork.EmployeeMilitaryCardsRepository.Get(d => d.Deleted != true && d.EmployeeId == id);
        }

        private async Task<IEnumerable<EmployeeLanguage>> SetLanguages(int id, UnitOfWork unitOfWork)
        { 
            return unitOfWork.EmployeeLanguagesRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "EmployeeLanguageType,Language");
        }

        private async Task<IEnumerable<EmployeeInternationalPassport>> SetInternationalPassports(int id, UnitOfWork unitOfWork)
        {
            return unitOfWork.EmployeeInternationalPassportsRepository
                    .Get(d => d.Deleted != true && d.EmployeeId == id);
        }

        private async Task<IEnumerable<EmployeeGun>> SetGuns(int id, UnitOfWork unitOfWork)
        {
            return unitOfWork.EmployeeGunsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id);
        }

        private async Task<IEnumerable<EmployeeFoundationDocument>> SetFoundationDocuments(int id, UnitOfWork unitOfWork)
        {
            return unitOfWork.EmployeeFoundationDocumentsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "FoundationDocument");
        }

        private async Task<IEnumerable<EmployeeEducation>> SetEducations(int id, UnitOfWork unitOfWork)
        {
            return unitOfWork.EmployeeEducationsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "EducationStudyForm");
        }

        private async Task<IEnumerable<EmployeeDrivingLicense>> SetDrivingLicenses(int id, UnitOfWork unitOfWork)
        {

            return unitOfWork.EmployeeDrivingLicensesRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id);
        }

        private async Task<IEnumerable<EmployeeContact>> SetContacts(int id, UnitOfWork unitOfWork)
        {

            return unitOfWork.EmployeeContactsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "ContactType");
        }

        private async Task<IEnumerable<EmployeeCar>> SetCars(int id, UnitOfWork unitOfWork)
        {
            return unitOfWork.EmployeeCarsRepository
                .Get(d => d.Deleted != true && d.EmployeeId == id);
        }



        /// <summary>
        /// Get employee for edit (contains only employees data) 
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>EmployeeEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetEdit", Name = "GetEmployeeEdit")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult GetEmployeeEdit(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeEditDTO employee = unitOfWork.EmployeesRepository
                .Get(d => d.Id == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country")
                .FirstOrDefault().ToDTOEdit();
                return Ok(employee);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != employee.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.AddressesRepository.Update(employee.Address);
                unitOfWork.EmployeesRepository.Update(employee);
                unitOfWork.Save();

                EmployeeEditDTO dto = unitOfWork.EmployeesRepository
                .Get(d => d.Id == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country")
                .FirstOrDefault().ToDTOEdit();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (employee == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                employee.Id = employee.NewId(unitOfWork);
                employee.Address.Id = employee.Address.NewId(unitOfWork);
                employee.AddressId = employee.Address.Id;
                unitOfWork.EmployeesRepository.Insert(employee);
                EmployeeEditDTO dto = employee.ToDTOEdit();
                return CreatedAtRoute("GetEmployeeEdit", new { id = dto.Id }, dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeEditDTO))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                Employee employee = unitOfWork.EmployeesRepository
                    .Get(d => d.Id == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country")
                    .FirstOrDefault();
                employee.Deleted = true;
                unitOfWork.EmployeesRepository.Update(employee);
                unitOfWork.Save();
                EmployeeEditDTO dto = employee.ToDTOEdit();
                return Ok(dto);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    //    [GTIFilter]
        [HttpGet]
        [Route("GetLists")]
        [ResponseType(typeof(EmployeeList))]
        public IHttpActionResult GetEmployeeLists()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeList list = unitOfWork.CreateEmployeeList();
                return Ok(list);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}