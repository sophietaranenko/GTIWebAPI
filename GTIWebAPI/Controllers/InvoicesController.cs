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
    /// <summary>
    /// To show invoices by organization
    /// </summary>
    [RoutePrefix("api/Invoices")]
    public class InvoicesController : ApiController
    {

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<DealInvoiceViewDTO>))]
        public IHttpActionResult GetInvoiceAll(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            IEnumerable<DealInvoiceViewDTO> dtos = new List<DealInvoiceViewDTO>();
            if (organizationId == 0)
            {
                return Ok(dtos);
            }
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.GetInvoicesList(organizationId, dateBegin, dateEnd);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok(dtos);
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
            InvoiceFullViewDTO dto = new InvoiceFullViewDTO();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dto = db.GetInvoiceCardInfo(id);
                    if (dto != null)
                    {
                        dto.Containers = db.GetContainersByInvoiceId(id);
                        dto.Lines = db.GetInvoiceLinesByInvoice(id);
                    }
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
