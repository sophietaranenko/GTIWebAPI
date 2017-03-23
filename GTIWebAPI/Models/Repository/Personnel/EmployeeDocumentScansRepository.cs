using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeDocumentScansRepository : IEmployeeDocumentScansRepository
    {
        private IDbContextFactory factory;
        public EmployeeDocumentScansRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeDocumentScansRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

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
            if (filePath == null)
            {
                throw new NotFoundException();
            }
            return filePath;
        }

        public EmployeeDocumentScan Add(EmployeeDocumentScan scan)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                scan.Id = scan.NewId(db);
                db.EmployeeDocumentScans.Add(scan);
                db.SaveChanges();
            }
            return scan;
        }

        public EmployeeDocumentScan Delete(int id)
        {
            EmployeeDocumentScan scan = new EmployeeDocumentScan();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                scan = db.EmployeeDocumentScans.Where(d => d.Id == id).FirstOrDefault();

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
                            ex.Entries.Single().Reload();
                        }
                    } while (saveFailed);
                }
            }

            scan = Get(scan.Id);
            return scan;
        }

        private bool EmployeeDocumentScanExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeDocumentScans.Count(e => e.Id == id) > 0;
            }
        }

        public List<EmployeeDocumentScan> FromByteArrayToString()
        {
            List<EmployeeDocumentScan> scans = new List<EmployeeDocumentScan>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                List<int> scanIds = db.EmployeeDocumentScans.Where(s => s.ScanName == null).Select(s => s.Id).ToList();
                foreach (var item in scanIds)
                {
                    EmployeeDocumentScan scan = db.EmployeeDocumentScans.Find(item);
                    if (scan.Scan != null)
                    {
                        try
                        {
                            WebImage image = new WebImage(scan.Scan);
                            var formatString = image.ImageFormat.ToString();
                            var filePath = HttpContext.Current.Server.MapPath("~/PostedFiles/" + db.FileNameUnique().ToString().Trim() + "." + formatString);
                            image.Save(filePath);
                            scan.ScanName = filePath;
                            db.Entry(scan).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }
            return scans;
        }

        public EmployeeDocumentScan Get(int id)
        {
            EmployeeDocumentScan scan = new EmployeeDocumentScan();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                scan = db.EmployeeDocumentScans
                    .Where(d => d.Id == id)
                    .FirstOrDefault();
            }
            if (scan == null)
            {
                throw new NotFoundException();
            }
            return scan;
        }

        public List<EmployeeDocumentScan> GetByDocumentId(string tableName, int tableId)
        {
            List<EmployeeDocumentScan> scans = new List<EmployeeDocumentScan>();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                scans = db.EmployeeDocumentScans
                .Where(e => e.ScanTableName == tableName && e.TableId == tableId && e.Deleted != true)
                .ToList();
            }
            if (scans == null)
            {
                throw new NotFoundException();
            }
            return scans;
        }

        public List<EmployeeDocumentScan> GetByEmployeeId(int employeeId)
        {
            List<EmployeeDocumentScan> scans = new List<EmployeeDocumentScan>();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                scans = db.EmployeeAllDocumentScans(employeeId).ToList();
            }
            if (scans == null)
            {
                throw new NotFoundException();
            }
            return scans;
        }
    }
}
