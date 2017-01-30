using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for storing and showing Employee photos
    /// </summary>
    [RoutePrefix("api/EmployeePhotos")]
    public class EmployeePhotosController : ApiController
    {
        private DbPersonnel db = new DbPersonnel();

        /// <summary>
        /// Method fo file upload
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("UploadFile")]
        public HttpResponseMessage UploadEmployeePhoto(int employeeId)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                int photoId = 0;
                foreach (string file in httpRequest.Files)
                {
                    EmployeePhoto photo = new EmployeePhoto();
                    photo.Id = photo.NewId(db);
                    photo.EmployeeId = employeeId;
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath(
                        "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "_" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    photo.PhotoName = filePath;
                    db.EmployeePhotos.Add(photo);
                    db.SaveChanges();
                    photoId = photo.Id;
                }
                EmployeePhoto uploadedPhoto = db.EmployeePhotos.Find(photoId);
                result = Request.CreateResponse(HttpStatusCode.Created, uploadedPhoto);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        /// <summary>
        /// Читает байтовый массив и превращает его в файл (для первоначального переноса при начале работы с новой базой данных)
        /// </summary>
        /// <returns></returns>
        [GTIFilter]
        [HttpPut]
        [Route("PutDbFilesToFilesystem")]
        public HttpResponseMessage PutDbFilesToFilesystem()
        {
            List<int> photos = db.EmployeePhotos.Where(s => s.PhotoName == null).Select(s => s.Id).ToList();
            List<EmployeePhoto> newPhotos = new List<EmployeePhoto>();
            foreach (var item in photos)
            {
                EmployeePhoto photo = db.EmployeePhotos.Find(item);
                if (photo.Photo != null)
                {
                    try
                    {
                        WebImage image = new WebImage(photo.Photo);
                        var formatString = image.ImageFormat.ToString();
                        var filePath = HttpContext.Current.Server.MapPath(
   "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "." + formatString);
                        image.Save(filePath);
                        photo.PhotoName = filePath;
                        db.Entry(photo).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            var result = Request.CreateResponse(HttpStatusCode.Created, newPhotos.Select(a => new { a.Id, a.PhotoName }));
            return result;
        }

        /// <summary>
        /// Get all photos by employee Id 
        /// </summary>
        /// <param name="employeeId">Id of employee</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        public IEnumerable<EmployeePhoto> GetEmployeePhotoByEmployeeId(int employeeId)
        {
            List<EmployeePhoto> photoList = db.EmployeePhotos
                .Where(e => e.Deleted != true && e.EmployeeId == employeeId)
                .ToList();
            return photoList;
        }

        /// <summary>
        /// Get photo by its id
        /// </summary>
        /// <param name="id">photo id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetEmployeePhoto(int id)
        {
            EmployeePhoto photo = db.EmployeePhotos.Find(id);
            if (photo != null)
            {
                return Ok(photo);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete photo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteEmployeePhoto(int id)
        {
            EmployeePhoto photo = db.EmployeePhotos.Find(id);
            if (photo != null)
            {
                photo.Deleted = true;
                db.Entry(photo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Ok(photo);
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

        private bool EmployeePhotoExists(int id)
        {
            return db.EmployeePhotos.Count(e => e.Id == id) > 0;
        }
    }
}
