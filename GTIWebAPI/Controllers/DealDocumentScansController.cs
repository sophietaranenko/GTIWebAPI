using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository.Accounting;
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
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Document scans that person attaches to deal 
    /// </summary>
    [RoutePrefix("api/DealDocumentScans")]
    public class DealDocumentScansController : ApiController
    {
        private IDealDocumentScansRepository repo;

        public DealDocumentScansController()
        {
            repo = new DealDocumentScansRepository();
        }

        public DealDocumentScansController(IDealDocumentScansRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// To get all scan types (types of document type we attach - BL, CMR etc.) 
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetDocumentScanTypes")]
        [ResponseType(typeof(List<DocumentScanTypeDTO>))]
        public IHttpActionResult GetDocumentScanTypes()
        {
            try
            {
                List<DocumentScanTypeDTO> dtos = repo.GetDocumentScanTypes();
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByDealId")]
        [ResponseType(typeof(List<DocumentScanDTO>))]
        public IHttpActionResult GetDocumentScansByDealId(Guid dealId)
        {
            try
            {
                List<DocumentScanDTO> dtos = repo.GetDocumentScansByDealId(dealId);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutDealDocumentScan(Guid scanId, int documentScanTypeId)
        {
            try
            {
                DocumentScanDTO dto = new DocumentScanDTO();
                dto = repo.GetDocumentScan(scanId);
                if (dto != null)
                {
                    string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                    ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

                    if (user != null)
                    {
                        string email = user.Email;
                        if (dto.ComputerName != null && dto.ComputerName.Trim().ToUpper() == email.Trim().ToUpper())
                        {
                            dto = repo.UpdateDealDocumentScan(dto);
                            if (dto != null)
                            {
                                return Ok(dto);
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest("Invalid arguments");
                        }
                    }
                    else
                    {
                        return BadRequest("User not found");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Upload new document scan to some deal by deal id 
        /// </summary>
        /// <param name="documentScanTypeId"></param>
        /// <param name="dealId"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Upload")]
        public IHttpActionResult UploadDealDocumentScan(int documentScanTypeId, Guid dealId)
        {
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.BadRequest);
            var httpRequest = HttpContext.Current.Request;
            DocumentScanDTO dto = new DocumentScanDTO();
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
                    try
                    {
                        using (var binaryReader = new BinaryReader(postedFile.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                        }
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Error reading file");
                    }
                    fileContent = fileData;
                    fileName = postedFile.FileName;
                    try
                    {
                        dto = repo.UploadDealDocumentScan(dealId, fileContent, fileName, email, documentScanTypeId);
                    }
                    catch (Exception e)
                    {
                        return BadRequest();
                    }
                }
            }
            return Ok(dto);
        }

        /// <summary>
        /// Delete document scan by its id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteDealDocumentScan(Guid id)
        {
            DocumentScanDTO dto = new DocumentScanDTO();
            try
            {
                bool result = repo.DeleteDealDocumentScan(id);
                if (result)
                {
                    return Ok(repo.GetDocumentScan(id));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
