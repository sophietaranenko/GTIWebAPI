﻿using System;
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
                //почему бы и нет? 
                Employee employee = 
                employee = unitOfWork.EmployeesRepository
                .Get(d => d.Id == id, includeProperties: @"Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country,
                                                    EmployeeOffices,EmployeeOffices.Department,EmployeeOffices.Office,EmployeeOffices.Profession,
                                                    EmployeePassports,EmployeePassports.Address,EmployeePassports.Address.AddressLocality,
                                                    EmployeePassports.Address.AddressPlace,EmployeePassports.Address.AddressRegion,EmployeePassports.Address.AddressVillage,EmployeePassports.Address.Country,
                                                    EmployeeMilitaryCards,
                                                    EmployeeLanguages,EmployeeLanguages.EmployeeLanguageType,EmployeeLanguages.Language,
                                                    EmployeeInternationalPassports,
                                                    EmployeeGuns,
                                                    EmployeeFoundationDocuments,EmployeeFoundationDocuments.FoundationDocument,
                                                    EmployeeEducations,EmployeeEducations.EducationStudyForm,
                                                    EmployeeDrivingLicenses,
                                                    EmployeeContacts,EmployeeContacts.ContactType,
                                                    EmployeeCars,
                                                    EmployeeInsurance")
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

                //employee.EmployeeOffices = unitOfWork.EmployeeOfficesRepository
                //    .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "Department,Office,Profession").ToList();

                //employee.EmployeePassports = unitOfWork.EmployeePassportsRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country").ToList() ;

              //  employee.EmployeeMilitaryCards = unitOfWork.EmployeeMilitaryCardsRepository.Get(d => d.Deleted != true && d.EmployeeId == id);

                //employee.EmployeeLanguages = unitOfWork.EmployeeLanguagesRepository
                //    .Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "EmployeeLanguageType,Language");

                //employee.EmployeeInternationalPassports = unitOfWork.EmployeeInternationalPassportsRepository
                //    .Get(d => d.Deleted != true && d.EmployeeId == id);

                //employee.EmployeeGuns = unitOfWork.EmployeeGunsRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id);

                //employee.EmployeeFoundationDocuments = unitOfWork.EmployeeFoundationDocumentsRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "FoundationDocument");

                //employee.EmployeeEducations = unitOfWork.EmployeeEducationsRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "EducationStudyForm");

                //employee.EmployeeDrivingLicenses = unitOfWork.EmployeeDrivingLicensesRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id);

                //employee.EmployeeContacts = unitOfWork.EmployeeContactsRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id, includeProperties: "ContactType");

                //employee.EmployeeCars = unitOfWork.EmployeeCarsRepository
                //.Get(d => d.Deleted != true && d.EmployeeId == id);

                


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
                .Get(d => d.Id == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country,EmployeeInsurance")
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
        public IHttpActionResult PutEmployee(int id, EmployeeEditDTO employee)
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
                Employee empl = employee.FromDTO();

                UnitOfWork unitOfWork = new UnitOfWork(factory);
           
                unitOfWork.AddressesRepository.Update(empl.Address);
                unitOfWork.EmployeeInsurancesRepository.Update(empl.EmployeeInsurance);
                unitOfWork.EmployeesRepository.UpdateFields(empl, "Id,Sex,IdentityCode,DateOfBirth,AddressId,EmployeeInsuranceId");
                unitOfWork.Save();

                EmployeeEditDTO dto = unitOfWork.EmployeesRepository
                .Get(d => d.Id == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country,EmployeeInsurance")
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
        public IHttpActionResult PostEmployee(EmployeeEditDTO employee)
        {
            if (employee == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Employee e = employee.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                e.Id = e.NewId(unitOfWork);
                e.Address.Id = e.Address.NewId(unitOfWork);
                e.AddressId = e.Address.Id;

                unitOfWork.EmployeesRepository.Insert(e);

                EmployeeEditDTO dto = e.ToDTOEdit();
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


        [GTIFilter]
        [HttpGet]
        [Route("GetStatistics")]
        public IHttpActionResult GetEmployeeUserStatistic()
        {
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            List<EmployeeStatistic> statistics = unitOfWork.SQLQuery<EmployeeStatistic>("exec EmployeeStatistic").ToList();
            return Ok(statistics);
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