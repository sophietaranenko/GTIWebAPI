using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
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
        private IDbContextFactory factory;
        private IIdentityHelper identityHelper;
        private IRequest request; 

        public DealDocumentScansController()
        {
            factory = new DbContextFactory();
            identityHelper = new IdentityHelper();
            request = new Request();
        }

        public DealDocumentScansController(IDbContextFactory factory)
        {
            this.factory = factory;
            identityHelper = new IdentityHelper();
            request = new Request();
        }

        public DealDocumentScansController(IDbContextFactory factory, IIdentityHelper identityHelper)
        {
            this.factory = factory;
            this.identityHelper = identityHelper;
            this.request = new Request();
        }

        public DealDocumentScansController(IDbContextFactory factory, IIdentityHelper identityHelper, IRequest request)
        {
            this.factory = factory;
            this.identityHelper = identityHelper;
            this.request = request;
        }
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
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<DocumentScanTypeDTO> dtos = unitOfWork.SQLQuery<DocumentScanTypeDTO>("exec GetDocumentScanTypes");
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get scans by deal id
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByDealId")]
        [ResponseType(typeof(IEnumerable<DocumentScanDTO>))]
        public IHttpActionResult GetDocumentScansByDealId(Guid dealId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@DealId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Guid,
                    Value = dealId
                };
                IEnumerable<DocumentScanDTO> dtos = unitOfWork.SQLQuery<DocumentScanDTO>("exec GetDocumentScanByDeal @DealId", parameter);
                return Ok(dtos);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        /// <summary>
        /// Ony documentScanTypeId property of documentScan can be updated 
        /// </summary>
        /// <param name="scanId">Id of scan we want to update</param>
        /// <param name="documentScanTypeId">Id of document scan type</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutDealDocumentScan(Guid scanId, int documentScanTypeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter pScanId = new SqlParameter
                {
                    ParameterName = "@ScanId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Guid,
                    Value = scanId
                };
                DocumentScanDTO dto = unitOfWork.SQLQuery<DocumentScanDTO>("exec GetDocumentScanById @ScanId", pScanId).FirstOrDefault();

                if (dto != null)
                {
                    //   string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                    //  ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

                    string email = identityHelper.GetUserEmail(User);
                        if (dto.ComputerName != null && dto.ComputerName.Trim().ToUpper() == email.Trim().ToUpper())
                        {
                            SqlParameter pDocumentScanTypeId = new SqlParameter
                            {
                                ParameterName = "@DocumentScanTypeId",
                                IsNullable = false,
                                Direction = ParameterDirection.Input,
                                DbType = DbType.Int32,
                                Value = documentScanTypeId
                            };
                            dto = unitOfWork.SQLQuery<DocumentScanDTO>("exec UpdateDocumentScanType @ScanId, @DocumentScanTypeId", pScanId, pDocumentScanTypeId).FirstOrDefault();
                            if (dto != null)
                            {
                                IEnumerable<DocumentScanTypeDTO> types = unitOfWork.SQLQuery<DocumentScanTypeDTO>("exec GetDocumentScanTypes");
                                if (types != null)
                                {
                                    dto.DocumentScanType = types.Where(d => d.Id == dto.DocumentScanTypeId).FirstOrDefault();
                                }
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
                    return NotFound();
                }
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
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
            DocumentScanDTO dto = new DocumentScanDTO();


            if (request.FileCount() == 1)
            {
                string fileName = "";
                byte[] fileContent = new byte[0];
                string email = "";

                email = identityHelper.GetUserEmail(User);

                foreach(string file in request.Collection())
                {
                    fileContent = request.GetBytes(file);
                    fileName = request.GetFileName(file);
                    try
                    {
                        UnitOfWork unitOfWork = new UnitOfWork(factory);
                        SqlParameter pDealId = new SqlParameter
                        {
                            ParameterName = "@DealId",
                            IsNullable = false,
                            DbType = DbType.Guid,
                            Value = dealId
                        };

                        SqlParameter pFileContent = new SqlParameter
                        {
                            ParameterName = "@FileContent",
                            IsNullable = false,
                            DbType = DbType.Binary,
                            Value = fileContent
                        };

                        SqlParameter pFileName = new SqlParameter
                        {
                            ParameterName = "@FileName",
                            IsNullable = false,
                            DbType = DbType.AnsiStringFixedLength,
                            Size = 100,
                            Value = fileName
                        };

                        SqlParameter pEmail = new SqlParameter
                        {
                            ParameterName = "@Email",
                            IsNullable = false,
                            DbType = DbType.AnsiStringFixedLength,
                            Size = 25,
                            Value = email
                        };

                        SqlParameter pTypeId = new SqlParameter
                        {
                            ParameterName = "@DocumentScanTypeId",
                            IsNullable = false,
                            DbType = DbType.Int32,
                            Value = documentScanTypeId
                        };

                        Guid scanId = unitOfWork.SQLQuery<Guid>("exec InsertDocumentScanByDeal @DealId, @FileContent, @FileName, @Email, @DocumentScanTypeId",
                                pDealId,
                                pFileContent,
                                pFileName,
                                pEmail,
                                pTypeId
                                ).FirstOrDefault();


                        SqlParameter pScanId = new SqlParameter
                        {
                            ParameterName = "@ScanId",
                            IsNullable = false,
                            Direction = ParameterDirection.Input,
                            DbType = DbType.Guid,
                            Value = scanId
                        };

                        dto = unitOfWork.SQLQuery<DocumentScanDTO>("exec GetDocumentScanById @ScanId", pScanId).FirstOrDefault();

                        if (dto != null)
                        {
                            IEnumerable<DocumentScanTypeDTO> types = unitOfWork.SQLQuery<DocumentScanTypeDTO>("exec GetDocumentScanTypes");
                            if (types != null)
                            {
                                dto.DocumentScanType = types.Where(d => d.Id == dto.DocumentScanTypeId).FirstOrDefault();
                            }
                        }
                    }
                    catch (NotFoundException nfe)
                    {
                        return NotFound();
                    }
                    catch (ConflictException ce)
                    {
                        return Conflict();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
            }
            else
            {
                return BadRequest();
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
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter pScanId = new SqlParameter
                {
                    ParameterName = "@ScanId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Guid,
                    Value = id
                };
                DocumentScanDTO dto = unitOfWork.SQLQuery<DocumentScanDTO>("exec GetDocumentScanById @ScanId", pScanId).FirstOrDefault();

                //Error: SqlParameter уже содержится в другом SqlParameterCollection 
                SqlParameter pScanId1 = new SqlParameter
                {
                    ParameterName = "@ScanId",
                    IsNullable = false,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Guid,
                    Value = id
                };
                bool result = unitOfWork.SQLQuery<bool>("exec DeleteDocumentScan @ScanId", pScanId1).FirstOrDefault();
                if (result)
                {
                    return Ok(dto);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
