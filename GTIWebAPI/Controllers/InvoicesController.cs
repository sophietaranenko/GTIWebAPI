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
    /// <summary>
    /// To show invoices by organization
    /// </summary>
    [RoutePrefix("api/Invoices")]
    public class InvoicesController : ApiController
    {
        private IInvoicesRepository repo;

        public InvoicesController()
        {
            repo = new InvoicesRepository();
        }

        public InvoicesController(IInvoicesRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<DealInvoiceViewDTO>))]
        public IHttpActionResult GetInvoiceAll(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }

            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modDateEnd = dateBegin.GetValueOrDefault();

            try
            {
                List<DealInvoiceViewDTO> dtos = repo.GetAll(organizationId, modDateBegin, modDateEnd);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
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
            try
            {
                InvoiceFullViewDTO dto = repo.Get(id);
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
