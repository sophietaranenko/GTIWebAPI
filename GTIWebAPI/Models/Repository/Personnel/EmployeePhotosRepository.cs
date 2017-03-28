using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Context;
using System.Data.Entity.Infrastructure;
using System.Web.Helpers;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeePhotosRepository : IEmployeePhotosRepository
    {
        private IDbContextFactory factory;
        public EmployeePhotosRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeePhotosRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeePhoto Add(EmployeePhoto photo)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                photo.Id = photo.NewId(db);
                db.EmployeePhotos.Add(photo);
                db.SaveChanges();
            }
            return Get(photo.Id);
        }

        public EmployeePhoto Delete(int id)
        {
            EmployeePhoto photo = new EmployeePhoto();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                photo = db.EmployeePhotos.Where(d => d.Id == id).FirstOrDefault();
                if (photo != null)
                {
                    photo.Deleted = true;
                    db.MarkAsModified(photo);
                    db.SaveChanges();
                }
                else
                {
                    throw new NotFoundException();
                }
            }
            return photo;
        }

        public EmployeePhoto Get(int id)
        {
            EmployeePhoto photo = new EmployeePhoto();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                photo = db.EmployeePhotos
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
            }
            if (photo == null)
            {
                throw new NotFoundException();
            }
            return photo;
        }

        public List<EmployeePhoto> GetByEmployeeId(int employeeId)
        {
            List<EmployeePhoto> list = new List<EmployeePhoto>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeePhotos
                    .Where(d => d.Deleted != true && d.EmployeeId == employeeId)
                    .ToList();
            }
            if (list == null)
            {
                throw new NotFoundException();
            }
            return list;
        }

        //public List<EmployeePhoto> PutDbFilesToFilesystem()
        //{
        //    List<EmployeePhoto> photos = new List<EmployeePhoto>();
        //    using (IAppDbContext db = factory.CreateDbContext())
        //    {
        //        //поскольку тут байтовые массивы, все данные тянутся долго, и все подвисает
        //        //пришлось доставать Idшки,
        //        //а потом по каждой Id - EmployeePhoto 
        //        List<int> photoIds = new List<int>();
        //        photoIds = db.EmployeePhotos.Where(s => s.PhotoName == null && s.Deleted != true)
        //            .Select(s => s.Id)
        //            .ToList();
        //        foreach (var item in photoIds)
        //        {
        //            EmployeePhoto photo = db.EmployeePhotos.Find(item);
        //            if (photo.Photo != null)
        //            {
        //                try
        //                {
        //                    WebImage image = new WebImage(photo.Photo);
        //                    var formatString = image.ImageFormat.ToString();
        //                    var filePath = HttpContext.Current.Server.MapPath(
        //                     "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "." + formatString);
        //                    image.Save(filePath);
        //                    photo.PhotoName = filePath;
        //                    db.Entry(photo).State = System.Data.Entity.EntityState.Modified;
        //                    db.SaveChanges();
        //                    photos.Add(photo);
        //                }
        //                catch
        //                {
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    return photos;
        //}

        public string SaveFile(HttpPostedFile postedFile)
        {
            string filePath = "";
            if (postedFile != null)
            {
                using (IAppDbContext db = factory.CreateDbContext())
                {
                    filePath = HttpContext.Current.Server.MapPath(
    "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "_" + postedFile.FileName);
                }
                postedFile.SaveAs(filePath);
            }
            return filePath;
        }

        private bool EmployeePhotoExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeePhotos.Count(e => e.Id == id) > 0;
            }
        }
    }
}
