using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
    [RoutePrefix("api/OrganizationsGTI")]
    public class OrganizationsGTIController : ApiController
    {

        private IDbContextFactory factory;

        public OrganizationsGTIController()
        {
            factory = new DbContextFactory();
        }

        public OrganizationsGTIController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIOfficeFilter]
        [HttpGet]
        [Route("SearchOrganizationsGTI")]
        [ResponseType(typeof(IEnumerable<OrganizationGTIDTO>))]
        public IHttpActionResult SearchOrganizationsGTI(string officeIds, string registrationNumber = "")
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            IEnumerable<OrganizationGTI> orgs = new List<OrganizationGTI>();
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("Value");
                foreach (var id in OfficeIds)
                {
                    DataRow row = dataTable.NewRow();
                    row["Value"] = id;
                    dataTable.Rows.Add(row);
                }
                SqlParameter pOffices = new SqlParameter
                {
                    ParameterName = "@OfficeIds",
                    TypeName = "ut_IntList",
                    Value = dataTable,
                    SqlDbType = SqlDbType.Structured
                };
                SqlParameter pRegistrationNumber = new SqlParameter
                {
                    ParameterName = "@RegistrationNumber",
                    IsNullable = true,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    Size = 1000,
                    Value = registrationNumber
                };
                List<OrganizationGTIDTO> gtis =
                    unitOfWork.SQLQuery<OrganizationGTI>("exec SearchOrganizationGTI @OfficeIds, @RegistrationNumber", pOffices, pRegistrationNumber)
                    .Select(d => d.ToDTO()).ToList();
                foreach (var item in gtis)
                {
                    item.Office = new Models.Dictionary.OfficeDTO { ShortName = item.OfficeShortName, Id = item.OfficeId };
                }
                return Ok(gtis);
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
