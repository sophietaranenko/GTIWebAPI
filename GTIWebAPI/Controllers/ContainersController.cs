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
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    //Containers as part of deal 

    [RoutePrefix("api/Containers")]
    public class ContainersController : ApiController
    {
        IDbContextFactory factory;

        public ContainersController()
        {
            factory = new DbContextFactory();
        }

        public ContainersController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<DealContainerViewDTO>))]
        public IHttpActionResult GetContainers(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (organizationId == 0)
            {
                return BadRequest("Empty organization");
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
                IEnumerable<DealContainerViewDTO> list =
                    unitOfWork.SQLQuery<DealContainerViewDTO>("exec ContainersList @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd);
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

        [GTIFilter]
        [HttpGet]
        [ResponseType(typeof(DealContainerViewDTO))]
        [Route("Get")]
        public IHttpActionResult GetContainer(Guid id)
        {
            try
            { 
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parId = new SqlParameter
                {
                    ParameterName = "@ContainerId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Guid,
                    Value = id
                };
                DealContainerViewDTO container = unitOfWork.SQLQuery<DealContainerViewDTO>("exec ContainerFullView @ContainerId", parId).FirstOrDefault(); 
                return Ok(container);
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
