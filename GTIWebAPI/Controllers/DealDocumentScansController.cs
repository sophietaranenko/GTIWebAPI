using GTIWebAPI.Filters;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/DealDocumentScans")]
    public class DealDocumentScansController : ApiController
    {
        DbOrganization db = new DbOrganization();

        [GTIFilter]
        [HttpGet]
        [Route("GetDocumentScanTypes")]
        public IEnumerable<DocumentScanTypeDTO> GetDocumentScanTypes()
        {
            IEnumerable<DocumentScanTypeDTO> dtos = new List<DocumentScanTypeDTO>();
            dtos = db.GetDocumentScanTypes();
            return dtos;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GeByDealId")]
        public IEnumerable<DocumentScanDTO> GetDocumentScansByDealId(Guid dealId)
        {
            IEnumerable<DocumentScanDTO> dtos = new List<DocumentScanDTO>();
            dtos = db.GetDocumentScanByDeal(dealId);
            return dtos;
        }

    }
}
