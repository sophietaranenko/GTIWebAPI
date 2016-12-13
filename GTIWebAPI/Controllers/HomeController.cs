using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GTIWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            List<EmployeeDocumentScan> list = new DbPersonnel().EmployeeDocumentScan.Where(s => s.Deleted != true && s.ScanName == null).OrderBy(c => c.Id).Skip(15).Take(5).ToList();

            //List<string> sList = new List<string>();

            //foreach (var item in list)
            //{
            //    try
            //    {
            //        var base64 = Convert.ToBase64String(item.Scan);
            //        var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            //        sList.Add(imgSrc);
            //    }
            //    catch (Exception e)
            //    {
            //        string m = e.Message;
            //        continue;
            //    }
            //}

            //return View(sList);

            return View();
        }
    }
}
