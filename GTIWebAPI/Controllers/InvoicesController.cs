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
    [RoutePrefix("api/Invoices")]
    public class InvoicesController : ApiController
    {
        DbOrganization db = new DbOrganization();

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<DealInvoiceViewDTO> GetInvoiceAll(int clientId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (clientId == 0)
            {
                return new List<DealInvoiceViewDTO>();
            }
            return db.GetInvoicesList(clientId, dateBegin, dateEnd);
        }


        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        [ResponseType(typeof(InvoiceFullViewDTO))]
        public IHttpActionResult GetInvoice(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            InvoiceFullViewDTO dto = db.GetInvoiceCardInfo(id);
            dto.Containers = db.GetContainersByInvoiceId(id);
            dto.Lines = db.GetInvoiceLinesByInvoice(id);
            return Ok(dto);
        }
    }
}
