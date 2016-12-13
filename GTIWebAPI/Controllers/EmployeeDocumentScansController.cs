using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
    /// Controllers for storing and showing images of documents
    /// </summary>
    [RoutePrefix("api/EmployeeDocumentScans")]
    public class EmployeeDocumentScansController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Method fo file upload
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("UploadFile")]
        public HttpResponseMessage UploadFile(string tableName, int tableId)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    EmployeeDocumentScan scan = new EmployeeDocumentScan();
                    scan.Id = scan.NewId(db);
                    scan.ScanTableId = tableId;
                    scan.ScanTableName = tableName;
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath(
                        "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "_" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    scan.ScanName = filePath;
                    db.EmployeeDocumentScan.Add(scan);
                    db.SaveChanges();
                }
                List<EmployeeDocumentScan> scans = db.EmployeeDocumentScan
                    .Where(s => s.Deleted != true && s.ScanTableId == tableId && s.ScanTableName == tableName)
                    .ToList();
                result = Request.CreateResponse(HttpStatusCode.Created, scans);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        /// <summary>
        /// Читает байтовый массив и превращает его в файл (для первоначального переноса при начале работы с реальной базой данных)
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutDbFilesToFilesystem")]
        public HttpResponseMessage PutDbFilesToFilesystem()
        {
            List<int> scans = db.EmployeeDocumentScan.Where(s => s.ScanName == null).Select(s => s.Id).ToList();
            List<EmployeeDocumentScan> newScans = new List<EmployeeDocumentScan>();
            foreach (var item in scans)
            {
                EmployeeDocumentScan scan = db.EmployeeDocumentScan.Find(item);
                if (scan.Scan != null)
                {
                    try
                    {
                        WebImage image = new WebImage(scan.Scan);
                        var formatString = image.ImageFormat.ToString();
                        var filePath = HttpContext.Current.Server.MapPath(
   "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "." + formatString);
                        image.Save(filePath);
                            scan.ScanName = filePath;
                            db.Entry(scan).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            var result = Request.CreateResponse(HttpStatusCode.Created, newScans.Select(a => new { a.Id, a.ScanName }));
            return result;
        }

        /// <summary>
        /// Get all scans by document Id 
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetAllScansByDocumentId")]
        public IEnumerable<EmployeeDocumentScan> GetAllScansByDocumentId(string tableName, int tableId)
        {
            List<EmployeeDocumentScan> scanList = db.EmployeeDocumentScan
                .Where(e => e.ScanTableName == tableName && e.ScanTableId == tableId && e.Deleted != true)
                .ToList();
            return scanList;
        }

        /// <summary>
        /// Get scan by its id
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetScan")]
        public IHttpActionResult GetScan(int id)
        {
            EmployeeDocumentScan scan = db.EmployeeDocumentScan.Find(id);
            if (scan != null)
            { 
                return Ok(scan);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete scan 
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("DeleteScan")]
        public IHttpActionResult DeleteScan(int id)
        {
            EmployeeDocumentScan scan = db.EmployeeDocumentScan.Find(id);
            if (scan != null)
            {
                scan.Deleted = true;
                db.Entry(scan).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Ok(scan);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeDocumentScanExists(int id)
        {
            return db.EmployeeDocumentScan.Count(e => e.Id == id) > 0;
        }
    }

}
