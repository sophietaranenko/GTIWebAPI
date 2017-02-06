using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
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
        [Route("GetByDealId")]
        public IEnumerable<DocumentScanDTO> GetDocumentScansByDealId(Guid dealId)
        {
            IEnumerable<DocumentScanDTO> dtos = new List<DocumentScanDTO>();
            dtos = db.GetDocumentScanByDeal(dealId);
            return dtos;
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutDealDocumentScan(Guid scanId, int documentScanTypeId)
        {
            DocumentScanDTO dto = db.GetDealDocumentScanById(scanId);
            if (dto != null)
            {
                string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                if (user != null)
                {
                    string email = user.Email;

                    if (dto.ComputerName != null && dto.ComputerName.Trim().ToUpper() == email.Trim().ToUpper())
                    {
                        dto = db.UpdateDocumentScanType(scanId, documentScanTypeId);
                        if (dto != null)
                        {
                            return Ok(dto);
                        }
                    }
                }
            }
            return BadRequest();
        }

        [GTIFilter]
        [HttpPost]
        [Route("Upload")]
        public IHttpActionResult UploadDealDocumentScan(int documentScanTypeId, Guid dealId)
        {
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.BadRequest);

            DocumentScanDTO dto = new DocumentScanDTO();

            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count == 1)
            {
                string fileName = "";
                byte[] fileContent = new byte[0];
                string email = "";

                string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                email = user.Email;

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }
                    fileContent = fileData;
                    fileName = postedFile.FileName;
                    Guid scanId = db.InsertDealDocumentScan(dealId, fileContent, fileName, email, documentScanTypeId);
                    dto = db.GetDealDocumentScanById(scanId);
                }
                return Ok(dto);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteDealDocumentScan(Guid id)
        {
            DocumentScanDTO dto = db.GetDealDocumentScanById(id);
            if (dto != null)
            {
                string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                if (user != null)
                {
                    string email = user.Email;

                    if (dto.ComputerName != null && dto.ComputerName.Trim().ToUpper() == email.Trim().ToUpper())
                    {
                        bool result = db.DeleteDocumentScan(id);
                        if (result == true)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest("Sorry you can delete only your own uploads");
                    }
                }
            }
            return BadRequest();
        }



    }
}
