using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
        /// <summary>
        /// List of Text and Value of enum Sex
        /// </summary>
        /// <returns>Collection of Json objects</returns>
        [GTIOfficeFilter]
        [HttpGet]
        [Route("SearchOrganizationsGTI")]
        [ResponseType(typeof(IEnumerable<OrganizationGTIDTO>))]
        public IHttpActionResult SearchOrganizationsGTI(string officeIds, string registrationNumber = "")
        {
            IEnumerable<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            IEnumerable<OrganizationGTI> orgs = new List<OrganizationGTI>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    orgs = db.SearchOrganizationGTI(OfficeIds, registrationNumber);
                    foreach (var item in orgs)
                    {
                        item.Office = db.Offices.Find(item.OfficeId);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            IEnumerable<OrganizationGTIDTO> dtos = orgs.Select(c => c.ToDTO()).ToList();
            return Ok(dtos);
        }
    }
}
