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
        [Route("GetInvoices")]
        public IEnumerable<InvoiceViewDTO> GetInvoices(int clientId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (clientId == 0)
            {
                return new List<InvoiceViewDTO>();
            }
            return db.InvoicesList(clientId, dateBegin, dateEnd);
        }
    }
}
