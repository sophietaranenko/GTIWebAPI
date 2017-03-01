using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
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
        //private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Method fo file upload
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("UploadFile")]
        public HttpResponseMessage UploadEmployeeDocumentScan(string tableName, int tableId)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 1)
            {
                using (DbMain db = new DbMain(User))
                {
                    int uploadedScanId = 0;
                    foreach (string file in httpRequest.Files)
                    {
                        EmployeeDocumentScan scan = new EmployeeDocumentScan();
                        scan.Id = scan.NewId(db);
                        scan.TableId = tableId;
                        scan.ScanTableName = tableName;
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath(
                            "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "_" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        scan.ScanName = filePath;
                        db.EmployeeDocumentScans.Add(scan);
                        db.SaveChanges();
                        uploadedScanId = scan.Id;
                    }
                    EmployeeDocumentScan uploadedScan = db.EmployeeDocumentScans.Find(uploadedScanId);
                    result = Request.CreateResponse(HttpStatusCode.Created, uploadedScan);
                }
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
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    List<int> scans = db.EmployeeDocumentScans.Where(s => s.ScanName == null).Select(s => s.Id).ToList();
                    List<EmployeeDocumentScan> newScans = new List<EmployeeDocumentScan>();
                    foreach (var item in scans)
                    {
                        EmployeeDocumentScan scan = db.EmployeeDocumentScans.Find(item);
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
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            } 
        }

        /// <summary>
        /// Get all scans by document Id 
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByDocumentId")]
        [ResponseType(typeof(IEnumerable<EmployeeDocumentScan>))]
        public IHttpActionResult GetEmployeeDocumentScanByDocumentId(string tableName, int tableId)
        {

            List<EmployeeDocumentScan> scanList = new List<EmployeeDocumentScan>();
            try
            {
                using (DbMain db = new DbMain(User))
                {
                    scanList = db.EmployeeDocumentScans
                    .Where(e => e.ScanTableName == tableName && e.TableId == tableId && e.Deleted != true)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(scanList);
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
        [ResponseType(typeof(IEnumerable<EmployeeDocumentScanDTO>))]
        public IHttpActionResult GetEmployeeDocumentScanByEmployeeId(int employeeId)
        {
            IEnumerable<EmployeeDocumentScanDTO> scanList = new List<EmployeeDocumentScanDTO>();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    scanList = db.EmployeeAllDocumentScans(employeeId).ToList();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(scanList);
        }

        /// <summary>
        /// Get scan by its id
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetEmployeeDocumentScan(int id)
        {
            EmployeeDocumentScan scan = new EmployeeDocumentScan();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    scan = db.EmployeeDocumentScans.Find(id);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

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
        [Route("Delete")]
        public IHttpActionResult DeleteEmployeeDocumentScan(int id)
        {
            EmployeeDocumentScan scan = new EmployeeDocumentScan();

            try
            {
                using (DbMain db = new DbMain(User))
                {
                    scan = db.EmployeeDocumentScans.Find(id);
                    if (scan != null)
                    {
                        scan.Deleted = true;
                        db.Entry(scan).State = System.Data.Entity.EntityState.Modified;

                        bool saveFailed = false;
                        do

                        {
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                saveFailed = true;

                                // Update the values of the entity that failed to save from the store 
                                ex.Entries.Single().Reload();
                            }
                        } while (saveFailed);

                        //db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest("Troubles with database connection");
            }

            return Ok(scan);
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool EmployeeDocumentScanExists(int id)
        {
            using (DbMain db = new DbMain(User))
            {
                return db.EmployeeDocumentScans.Count(e => e.Id == id) > 0;
            }
        }
    }

}
