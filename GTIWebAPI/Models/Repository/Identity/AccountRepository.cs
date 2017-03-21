using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.Models.Repository.Identity
{
    public class AccountRepository : IAccountRepository
    {
        private IDbContextFactory factory;

        public AccountRepository()
        {
            factory = new DbContextFactory();
        }

        public AccountRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public bool IsEmployeeInformationFilled(int tableId)
        {
            bool result = false;
            using (IAppDbContext db = factory.CreateDbContext())
            {
                result = db.IsEmployeeInformationFilled(tableId);
            }
            return result;
        }

        public int GetOrganizationIdOfPerson(int organizationContactPersonId)
        {
            int organizationId = 0;
            using (IAppDbContext db = factory.CreateDbContext())
            {
                organizationId =
                    db.OrganizationContactPersons
                    .Where(d => d.Id == organizationContactPersonId)
                    .Select(d => d.OrganizationId)
                    .FirstOrDefault()
                    .GetValueOrDefault();
            }
            return organizationId;
        }

        public string SaveFile(HttpPostedFile postedFile)
        {
            string filePath = "";
            using (IAppDbContext db = factory.CreateDbContext())
            { 
                filePath = HttpContext.Current.Server.MapPath(
                            "~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "_" + postedFile.FileName);
            postedFile.SaveAs(filePath);
            }
            if (filePath != null && filePath.Length > 3)
            {
                return filePath;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public string GetProfilePicturePathByUserId(string userId)
        {
            UserImage image = new UserImage();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                image = db.UserImages.Where(d => d.UserId == userId && d.IsProfilePicture == true).FirstOrDefault();
            }
            if (image != null)
            {
                return image.ImageName;
            }
            else
            {
                return null;
            }
        }

        public UserImage AddNewProfilePicture(UserImage image)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                List<UserImage> images = db.UserImages.Where(d => d.UserId == image.UserId).ToList();
                foreach (var item in images)
                {
                    item.IsProfilePicture = false;
                    db.MarkAsModified(item);
                }
                image.Id = image.NewId(db);
                db.UserImages.Add(image);
                db.SaveChanges();
            }
            return image;
        }

        public UserImage SetAsProfilePicture(int pictureId)
        {
            UserImage image = new UserImage();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                image = db.UserImages.Where(d => d.Id == pictureId).FirstOrDefault();
                

                List<UserImage> images = db.UserImages.Where(d => d.UserId == image.UserId).ToList();
                foreach (var item in images)
                {
                    item.IsProfilePicture = false;
                    db.MarkAsModified(item);
                }

                image.IsProfilePicture = true;
                db.MarkAsModified(image);
                db.SaveChanges();
            }
            return image;
        }

        public bool CreateOrganization(string email, string password)
        {
            bool result = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                result = db.CreateOrganization(email, password);
            }
            return result;
        }

        public bool CreateHoldingUser(string email, string password)
        {
            bool result = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                result = db.CreateHoldingUser(email, password);
            }
            return result;
        }

        public OrganizationContactPersonView FindPerson(int id)
        {
            OrganizationContactPersonView person = new OrganizationContactPersonView();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                person = db.OrganizationContactPersonViews.Where(d => d.Id == id).FirstOrDefault();
            }
            return person;
        }

    }
}
