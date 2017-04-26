using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
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
    /// <summary>
    /// To show invoices by organization
    /// </summary>
    [RoutePrefix("api/Invoices")]
    public class InvoicesController : ApiController
    {
        private IDbContextFactory factory;

        public InvoicesController()
        {
            factory = new DbContextFactory();
        }

        public InvoicesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<DealInvoiceViewDTO>))]
        public IHttpActionResult GetInvoiceAll(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
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
                IEnumerable<DealInvoiceViewDTO> dtos = unitOfWork.SQLQuery<DealInvoiceViewDTO>("exec InvoicesList @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd);
                return Ok(dtos);
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
        [Route("Get")]
        [ResponseType(typeof(InvoiceFullViewDTO))]
        public IHttpActionResult GetInvoice(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                    SqlParameter parameter = new SqlParameter
                    {
                        ParameterName = "@InvoiceId",
                        IsNullable = false,
                        Direction = ParameterDirection.Input,
                        DbType = DbType.Int32,
                        Value = id
                    };

                InvoiceFullViewDTO dto = unitOfWork.SQLQuery<InvoiceFullViewDTO>("exec InvoiceInfo @InvoiceId", parameter).FirstOrDefault();
                if (dto == null)
                {
                    return NotFound();
                }

                SqlParameter parameter1 = new SqlParameter
                {
                    ParameterName = "@InvoiceId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = id
                };

                dto.Containers = unitOfWork.SQLQuery<InvoiceContainerViewDTO>("Exec InvoiceContainerList @InvoiceId", parameter1).OrderBy(l => l.Name);

                SqlParameter parameter2 = new SqlParameter
                {
                    ParameterName = "@InvoiceId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = id
                };
                dto.Lines = unitOfWork.SQLQuery<InvoiceLineViewDTO>("Exec InvoiceLineList @InvoiceId", parameter2).OrderBy(l => l.LinePosition);
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
    }
}
