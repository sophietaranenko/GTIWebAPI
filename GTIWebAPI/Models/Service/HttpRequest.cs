using GTIWebAPI.Exceptions;
using GTIWebAPI.Models.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.Models.Service
{
    public interface IRequest
    {
        List<string> Collection();

        string SaveFile(string fileName);

        string GetPath();

        string AppPath();

        byte[] GetBytes(string fileName);

        string GetFileName(string fileName);

        int FileCount();

    }

    public class Request : IRequest
    {
        public List<string> Collection()
        {
            List<string> list = new List<string>();
            foreach (string file in HttpContext.Current.Request.Files)
            {
                list.Add(file);
            }
            return list;
        }

        public string SaveFile(string fileName)
        {
            var postedFile = HttpContext.Current.Request.Files[fileName];
            if (postedFile == null)
            {
                throw new NotFoundException();
            }
            string filePath = GetPath() + Guid.NewGuid().ToString().Trim() + System.IO.Path.GetExtension(postedFile.FileName);
            if (filePath == null)
            {
                throw new NotFoundException();
            }
            postedFile.SaveAs(filePath);
            filePath = filePath.Replace(AppPath(), String.Empty);
            return filePath;
        }

        public string GetPath()
        {
            return HttpContext.Current.Server.MapPath("~/PostedFiles/");
        }

        public string AppPath()
        {
            return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
        }

        public string GetFileName(string fileName)
        {
            return HttpContext.Current.Request.Files[fileName].FileName;
        }

        public byte[] GetBytes(string fileName)
        {
            var postedFile = HttpContext.Current.Request.Files[fileName];
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
                throw new NotFoundException();
            }
            return fileData;
        }

        public int FileCount()
        {
            return HttpContext.Current.Request.Files.Count;
        }

    }
}
