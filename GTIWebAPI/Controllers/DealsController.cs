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
    [RoutePrefix("api/Deals")]
    public class DealsController : ApiController
    {
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
        public IHttpActionResult GetDeals(int organizationId, DateTime dateBegin, DateTime dateEnd)
        {
            IEnumerable<DealViewDTO> dtos = new List<DealViewDTO>();

            if (organizationId == 0)
            {
                return Ok(dtos);
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
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dtos = db.GetDealsFiltered(organizationId, dateBegin, dateEnd);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
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
            DealFullViewDTO dto = new DealFullViewDTO();
            try
            {
                using (IAppDbContext db = AppDbContextFactory.CreateDbContext(User))
                {
                    dto = db.GetDealCardInfo(id);

                    if (dto == null)
                    {
                        return NotFound();
                    }

                    dto.Containers = db.GetContainersByDeal(id);
                    dto.Invoices = db.GetInvoicesByDeal(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dto);
        }
    }
}
