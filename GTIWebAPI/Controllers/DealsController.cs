using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Repository.Accounting;
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
    [RoutePrefix("api/Deals")]
    public class DealsController : ApiController
    {
        private IDbContextFactory factory;


        public DealsController()
        {
            factory = new DbContextFactory();
        }

        public DealsController(IDbContextFactory factory)
        {
            this.factory = factory;
        }


        /// <summary>
        /// Get short info about deals 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<DealViewDTO>))]
        public IHttpActionResult GetDeals(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (organizationId == 0)
            {
                return BadRequest("Empty organizationId");
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<DealViewDTO> dtos = unitOfWork.SQLQuery<DealViewDTO>("exec DealsFilter @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd);
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


        /// <summary>
        /// Get one deal full view by its id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        [ResponseType(typeof(DealFullViewDTO))]
        public IHttpActionResult GetDealView(string Id)
        {
            Guid id;
            bool result = Guid.TryParse(Id, out id);

            if (result == false)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@DealId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Guid,
                    Value = Id
                };
                DealFullViewDTO dto = unitOfWork.SQLQuery<DealFullViewDTO>("exec DealInfo @DealId", parameter).FirstOrDefault();
                if (dto == null)
                {
                    return NotFound();
                }
                dto.Containers = unitOfWork.SQLQuery<DealContainerViewDTO>("exec DealContainersList @DealId", parameter);
                dto.Invoices = unitOfWork.SQLQuery<DealInvoiceViewDTO>("exec DealInvoicesList @DealId", parameter);
                dto.DocumentScans = unitOfWork.SQLQuery<DocumentScanDTO>("exec GetDocumentScanByDeal @DealId", parameter);
                IEnumerable<DocumentScanTypeDTO> types = unitOfWork.SQLQuery<DocumentScanTypeDTO>("exec GetDocumentScanTypes");
                if (dto.DocumentScans != null && types != null)
                {
                    foreach (var item in dto.DocumentScans)
                    {
                        item.DocumentScanType = types.Where(d => d.Id == item.DocumentScanTypeId).FirstOrDefault();
                    }
                }
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
