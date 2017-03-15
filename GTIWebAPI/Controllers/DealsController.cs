using GTIWebAPI.Filters;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository.Accounting;
using System;
using System.Collections.Generic;
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
        private IDealsRepository repo;

        public DealsController()
        {
            repo = new DealsRepository();
        }

        public DealsController(IDealsRepository repo)
        {
            this.repo = repo;
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
        [ResponseType(typeof(IEnumerable<DealViewDTO>))]
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
                List<DealViewDTO> dtos = repo.GetAll(organizationId, modDateBegin, modDateEnd);
                return Ok(dtos);
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
                DealFullViewDTO dto = repo.Get(id);
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
