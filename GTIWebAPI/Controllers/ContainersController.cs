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
    //Containers as part of deal 

    [RoutePrefix("api/Containers")]
    public class ContainersController : ApiController
    {
        /// <summary>
        /// All containers by organization and dates (date of deal, to change - look into database) 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<DealContainerViewDTO>))]
        public IHttpActionResult GetContainers(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            IEnumerable<DealContainerViewDTO> list = new List<DealContainerViewDTO>();
            if (organizationId == 0)
            {
                return Ok(list);
            }

            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    list = db.GetContainersFiltered(organizationId, dateBegin, dateEnd);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(list);
        }

        /// <summary>
        /// One container by its id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [ResponseType(typeof(DealContainerViewDTO))]
        [Route("Get")]
        public IHttpActionResult GetContainer(Guid id)
        {
            DealContainerViewDTO container = new DealContainerViewDTO();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    container = db.GetContainer(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(container);
        }

    }
}
