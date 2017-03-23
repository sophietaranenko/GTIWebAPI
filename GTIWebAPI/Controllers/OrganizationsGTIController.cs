using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/OrganizationsGTI")]
    public class OrganizationsGTIController : ApiController
    {

        IOrganizationsGTIRepository repo;

        public OrganizationsGTIController()
        {
            repo = new OrganizationsGTIRepository();
        }

        public OrganizationsGTIController(IOrganizationsGTIRepository repo)
        {
            this.repo = repo;
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
                List<OrganizationGTIDTO> dtos =
                    repo.Search(OfficeIds, registrationNumber)
                    .Select(d => d.ToDTO())
                    .ToList();
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
    }
}
