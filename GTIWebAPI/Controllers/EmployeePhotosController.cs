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
        public HttpResponseMessage UploadFile(int employeeId)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
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
                    db.EmployeePhoto.Add(photo);
                    db.SaveChanges();
                }
                List<EmployeePhoto> photos = db.EmployeePhoto
                    .Where(s => s.Deleted != true && s.EmployeeId == employeeId)
                    .ToList();
                result = Request.CreateResponse(HttpStatusCode.Created, photos);
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
            List<int> photos = db.EmployeePhoto.Where(s => s.PhotoName == null).Select(s => s.Id).ToList();
            List<EmployeePhoto> newPhotos = new List<EmployeePhoto>();
            foreach (var item in photos)
            {
                EmployeePhoto photo = db.EmployeePhoto.Find(item);
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
        [Route("GetPhotosByEmployeeId")]
        public IEnumerable<EmployeePhoto> GetPhotosByEmployeeId(int employeeId)
        {
            List<EmployeePhoto> photoList = db.EmployeePhoto
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
        [Route("GetPhoto")]
        public IHttpActionResult GetPhoto(int id)
        {
            EmployeePhoto photo = db.EmployeePhoto.Find(id);
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
        [Route("DeletePhoto")]
        public IHttpActionResult DeletePhoto(int id)
        {
            EmployeePhoto photo = db.EmployeePhoto.Find(id);
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
            return db.EmployeePhoto.Count(e => e.Id == id) > 0;
        }
    }
}
