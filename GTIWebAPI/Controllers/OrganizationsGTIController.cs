using GTIWebAPI.Filters;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/OrganizationsGTI")]
    public class OrganizationsGTIController : ApiController
    {
        /// <summary>
        /// List of Text and Value of enum Sex
        /// </summary>
        /// <returns>Collection of Json objects</returns>
        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetOrganizationsGTI")]
        public IEnumerable<OrganizationGTIDTO> GetOrganizationsGTI(string officeIds, string registrationNumber = "", string taxNumber = "", string Name = "")
        {
            IEnumerable<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            List<OrganizationGTIDTO> dtos = new List<OrganizationGTIDTO>();
            return dtos;
        }
    }
}
