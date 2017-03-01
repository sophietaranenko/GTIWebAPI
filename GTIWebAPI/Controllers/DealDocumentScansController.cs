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
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Document scans that person attaches to deal 
    /// </summary>
    [RoutePrefix("api/DealDocumentScans")]
    public class DealDocumentScansController : ApiController
    {
        /// <summary>
        /// To get all scan types (types of document type we attach - BL, CMR etc.) 
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetDocumentScanTypes")]
        [ResponseType(typeof(IEnumerable<DocumentScanTypeDTO>))]
        public IHttpActionResult GetDocumentScanTypes()
        {
            IEnumerable<DocumentScanTypeDTO> dtos = new List<DocumentScanTypeDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.GetDocumentScanTypes();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Get one document scan by id of deal it is attached to 
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByDealId")]
        [ResponseType(typeof(IEnumerable<DocumentScanDTO>))]
        public IHttpActionResult GetDocumentScansByDealId(Guid dealId)
        {
            IEnumerable<DocumentScanDTO> dtos = new List<DocumentScanDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dtos = db.GetDocumentScanByDeal(dealId);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(dtos);
        }

        /// <summary>
        /// We can update only type of document to deal document scan. Otherwise we need to delete and upload new document scan. 
        /// </summary>
        /// <param name="scanId"></param>
        /// <param name="documentScanTypeId"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutDealDocumentScan(Guid scanId, int documentScanTypeId)
        {
            DocumentScanDTO dto = new DocumentScanDTO();

            //everything must be all right when return into using (Stackoverflow told me!) 
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    dto = db.GetDealDocumentScanById(scanId);
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
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return BadRequest();
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
                        using (DbMain db = new DbMain(User))
                        {
                            Guid scanId = db.InsertDealDocumentScan(dealId, fileContent, fileName, email, documentScanTypeId);
                            dto = db.GetDealDocumentScanById(scanId);
                        }
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
                using (DbMain db = new DbMain(User))
                {

                    db.GetDealDocumentScanById(id);

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

                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return BadRequest();
        }



    }
}
