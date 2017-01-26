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
        public IEnumerable<DealInvoiceViewDTO> GetInvoices(int clientId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (clientId == 0)
            {
                return new List<DealInvoiceViewDTO>();
            }
            return db.InvoicesList(clientId, dateBegin, dateEnd);
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
            InvoiceFullViewDTO dto = db.InvoiceCardInfo(id);
            dto.Containers = db.ContainersByInvoiceId(id);
            dto.Lines = db.InvoiceLinesByInvoice(id);
            return Ok(dto);
        }
    }
}
