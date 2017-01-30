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
        DbOrganization db = new DbOrganization();

        //[GTIFilter]
        //[HttpGet]
        //[Route("GetAll")]
        //public IEnumerable<DealShortViewDTO> GetDeals(int clientId, DateTime dateBegin, DateTime dateEnd)
        //{
        //    if (clientId == 0)
        //    {
        //        return new List<DealShortViewDTO>();
        //    }
        //    if (dateBegin == null)
        //    {
        //        dateBegin = new DateTime(1900, 1, 1);
        //    }
        //    if (dateEnd == null)
        //    {
        //        dateEnd = new DateTime(2200, 1, 1);
        //    }
        //    return db.DealList(clientId, dateBegin, dateEnd);
        //}

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
            DealFullViewDTO dto = db.GetDealCardInfo(id);
            if (dto == null)
            {
                return NotFound();
            }

            dto.Containers = db.GetContainersByDeal(id);
            dto.Invoices = db.GetInvoicesByDeal(id);

            return Ok(dto);
        }
    }
}
