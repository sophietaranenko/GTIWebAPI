using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Reports.ProductivityReport;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/ProductivityReport")]
    public class ProductivityReportController : ApiController
    {
        IDbContextFactory factory;

        public ProductivityReportController()
        {
            factory = new DbContextFactory();
        }

        public ProductivityReportController(IDbContextFactory factory)
        {
            this.factory = factory;
        }


        [GTIFilter]
        [HttpGet]
        [Route("GetAllPeriodicDataByMonths")]
        [ResponseType(typeof(IEnumerable<ReportMonth>))]
        public IHttpActionResult GetAllPeriodicDataByMonths(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId 
                };

                IEnumerable<ReportMonth> list =
                    unitOfWork.SQLQuery<ReportMonth>("exec ProductivityReportAllMonth @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetAllPeriodicDataByWeeks")]
        [ResponseType(typeof(IEnumerable<ReportWeek>))]
        public IHttpActionResult GetAllPeriodicDataByWeeks(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<ReportWeek> list =
                    unitOfWork.SQLQuery<ReportWeek>("exec ProductivityReportAllWeek @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetKPIByMonths")]
        [ResponseType(typeof(IEnumerable<KPIValueMonth>))]
        public IHttpActionResult GetKPIByMonths(DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2100, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                IEnumerable<KPIValueMonth> list =
                    unitOfWork.SQLQuery<KPIValueMonth>("exec AllKPIValuesByMonth @DateBegin, @DateEnd", parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetKPIByWeeks")]
        [ResponseType(typeof(IEnumerable<KPIValueMonth>))]
        public IHttpActionResult GetKPIByWeeks(DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                IEnumerable<KPIValueMonth> list =
                    unitOfWork.SQLQuery<KPIValueMonth>("exec AllKPIValuesByWeek @DateBegin, @DateEnd", parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetContactsByMonths")]
        [ResponseType(typeof(IEnumerable<ContactMonth>))]
        public IHttpActionResult GetContactsByMonths(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<ContactMonth> list =
                    unitOfWork.SQLQuery<ContactMonth>("exec ProductivityReportContactWithKPIMonth @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetContactsByWeeks")]
        [ResponseType(typeof(IEnumerable<ContactWeek>))]
        public IHttpActionResult GetContactsByWeeks(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<ContactWeek> list =
                    unitOfWork.SQLQuery<ContactWeek>("exec ProductivityReportContactWithKPIWeek @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetWorksByMonths")]
        [ResponseType(typeof(IEnumerable<WorkMonth>))]
        public IHttpActionResult GetWorksByMonths(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<WorkMonth> list =
                    unitOfWork.SQLQuery<WorkMonth>("exec ProductivityReportWorkMonth @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetWorksByWeeks")]
        [ResponseType(typeof(IEnumerable<WorkWeek>))]
        public IHttpActionResult GetWorksByWeeks(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<WorkWeek> list =
                    unitOfWork.SQLQuery<WorkWeek>("exec ProductivityReportWorkWeek @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetSalesByMonths")]
        [ResponseType(typeof(IEnumerable<SalesMonth>))]
        public IHttpActionResult GetSalesByMonths(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<SalesMonth> list =
                    unitOfWork.SQLQuery<SalesMonth>("exec ProductivityReportSalesWithKPIMonth @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetSalesByWeeks")]
        [ResponseType(typeof(IEnumerable<SalesWeek>))]
        public IHttpActionResult GetSalesByWeeks(int employeeId, int officeId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateEnd.GetValueOrDefault();

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parBegin = new SqlParameter
                {
                    ParameterName = "@DateBegin",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateBegin
                };
                SqlParameter parEnd = new SqlParameter
                {
                    ParameterName = "@DateEnd",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.DateTime,
                    Value = modDateEnd
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<SalesWeek> list =
                    unitOfWork.SQLQuery<SalesWeek>("exec ProductivityReportSalesWithKPIWeek @EmployeeId, @OfficeId, @DateBegin, @DateEnd", parEmployee, parOffice, parBegin, parEnd);
                list = list.OrderBy(d => d.DateBegin);
                return Ok(list);
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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
        [Route("GetStatuses")]
        [ResponseType(typeof(OrganizationStatusDTO))]
        public IHttpActionResult GetStatuses(int employeeId, int officeId)
        {
            if (employeeId == 0)
            {
                return BadRequest("Empty employee");
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parEmployee = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                SqlParameter parOffice = new SqlParameter
                {
                    ParameterName = "@OfficeId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = officeId
                };

                IEnumerable<OrganizationStatus> list =
                    unitOfWork.SQLQuery<OrganizationStatus>("exec ProductivityReportStatus @EmployeeId, @OfficeId", parEmployee, parOffice);
                return Ok(new OrganizationStatusDTO(list.ToList()));
            }
            catch (NullReferenceException nre)
            {
                return NotFound();
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

    }
}
