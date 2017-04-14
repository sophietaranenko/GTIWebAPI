using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    /// Controllers for storing and showing images of documents
    /// </summary>
    [RoutePrefix("api/EmployeeDocumentScans")]
    public class EmployeeDocumentScansController : ApiController
    {

        private IEmployeeDocumentScansRepository repo;

        public EmployeeDocumentScansController()
        {
            repo = new EmployeeDocumentScansRepository();
        }

        public EmployeeDocumentScansController(IEmployeeDocumentScansRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Method fo file upload
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("UploadFile")]
        [ResponseType(typeof(List<EmployeeDocumentScan>))]
        public IHttpActionResult UploadEmployeeDocumentScan(string tableName, int tableId)
        {
            try
            {
                HttpRequest request = null;
                if (HttpContext.Current != null)
                {
                    request = HttpContext.Current.Request;
                }
                else
                {
                    throw new ArgumentException("No HttpContext found!");
                }

                HttpPostedFile postedFile = null;

                List<EmployeeDocumentScan> scans = new List<EmployeeDocumentScan>();

                foreach (string file in request.Files)
                {
                    postedFile = request.Files[file];
                    if (postedFile != null)
                    { 
                    EmployeeDocumentScan scan = new EmployeeDocumentScan()
                    {
                        TableId = tableId,
                        ScanTableName = tableName,
                        ScanName = repo.SaveFile(postedFile)
                    };
                    scan = repo.Add(scan);
                        scans.Add(scan);
                    }
                }
                return CreatedAtRoute("GetScanListByDocumentId", new { tableName = tableName, tableId = tableId }, scans);
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

        ///// <summary>
        ///// Читает байтовый массив и превращает его в файл (для первоначального переноса при начале работы с реальной базой данных)
        ///// </summary>
        ///// <returns></returns>
        //[GTIFilter]
        //[HttpPut]
        //[Route("PutDbFilesToFilesystem")]
        //[ResponseType(typeof(List<EmployeeDocumentScan>))]
        //public IHttpActionResult PutDbFilesToFilesystem()
        //{
        //    try
        //    {
        //        List<EmployeeDocumentScan> scanList = repo.FromByteArrayToString();
        //        return Ok(scanList);
        //    }
        //    catch (NotFoundException nfe)
        //    {
        //        return NotFound();
        //    }
        //    catch (ConflictException ce)
        //    {
        //        return Conflict();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        /// <summary>
        /// Get all scans by document Id 
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByDocumentId", Name = "GetScanListByDocumentId")]
        [ResponseType(typeof(List<EmployeeDocumentScan>))]
        public IHttpActionResult GetEmployeeDocumentScanByDocumentId(string tableName, int tableId)
        {
            try
            {
                List<EmployeeDocumentScan> scans = repo.GetByDocumentId(tableName, tableId);
                return Ok(scans);
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
        /// Get all scans by employee
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(List<EmployeeDocumentScan>))]
        public IHttpActionResult GetEmployeeDocumentScanByEmployeeId(int employeeId)
        {
            try
            {
                List<EmployeeDocumentScan> scanList = repo.GetByEmployeeId(employeeId);
                return Ok(scanList);
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
        /// Get scan by its id
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        [ResponseType(typeof(EmployeeDocumentScan))]
        public IHttpActionResult GetEmployeeDocumentScan(int id)
        {
            try
            {
                EmployeeDocumentScan scan = repo.Get(id);
                return Ok(scan);
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
        /// Delete scan 
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeDocumentScan))]
        public IHttpActionResult DeleteEmployeeDocumentScan(int id)
        {
            try
            {
                EmployeeDocumentScan scan = repo.Delete(id);
                return Ok(scan);
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
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }

}
