using GTIWebAPI.Filters;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/Containers")]
    public class ContainersController : ApiController
    {
        DbOrganization db = new DbOrganization();

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<DealContainerViewDTO> GetContainers(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (organizationId == 0)
            {
                return new List<DealContainerViewDTO>();
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            return db.GetContainersFiltered(organizationId, dateBegin, dateEnd);
        }

        [GTIFilter]
        [HttpGet]
        [ResponseType(typeof(DealContainerViewDTO))]
        [Route("Get")]
        public IHttpActionResult GetContainer(Guid id)
        {
            DealContainerViewDTO container = db.GetContainer(id);
            return Ok(container);
        }

    }
}
