using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestEmployeeDocumentScansController
    {
        //i don't know how to test httprequest files without any mocking framework 
        //to do later!


        private IDbContextFactory factory;
        private IEmployeeDocumentScansRepository repo;

        public TestEmployeeDocumentScansController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeDocumentScansRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void PostScan_ShouldReturnBadRequest()
        {
            var controller = new EmployeeDocumentScansController(repo);
            
            var item = GetDemo();
            var result = controller.UploadEmployeeDocumentScan("EmployeePassport", 1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
       }

        //doesn't work!!!
        //[TestMethod]
        //public void PostScan_ShouldBePosted()
        //{
        //       var content = new ByteArrayContent(File.ReadAllBytes(@"C:\Projects\GTIWebAPI\GTIWebAPI.Tests\Images\EcU6XCRo7sY.jpg"));
        //        content.Headers.Add("Content-Disposition", "form-data");

        //        var controllerContext = new HttpControllerContext
        //        {
        //            Request = new HttpRequestMessage
        //            {
        //                Content = new MultipartContent { content }
        //            }
        //        };
        //        var controller = new EmployeeDocumentScansController();
        //        controller.ControllerContext = controllerContext;
        //    var result = controller.UploadEmployeeDocumentScan("EmployeePassport", 1) as CreatedAtRouteNegotiatedContentResult<List<EmployeeDocumentScan>>;
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(result.RouteName, "GetScanListByDocumentId");
        //   // Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        //   // Assert.IsNotNull(result.Content.ScanName);
        //}


        private EmployeeDocumentScan GetDemo()
        {
            EmployeeDocumentScan documentScan = new EmployeeDocumentScan
            {
                Id = 0,
                TableId = 3,
                ScanTableName = "EmployeePassport"
            };
            return documentScan;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeDocumentScan { Id = 1, ScanName = "somescanname.jpg", ScanTableName = "EmployeePassport", TableId = 1 });
            repo.Add(new EmployeeDocumentScan { Id = 1, ScanName = "somescanname.jpg", ScanTableName = "EmployeePassport", TableId = 1 });
            repo.Add(new EmployeeDocumentScan { Id = 1, ScanName = "somescanname.jpg", Deleted = true, ScanTableName = "EmployeePassport", TableId = 2 });
            repo.Add(new EmployeeDocumentScan { Id = 1, ScanName = "somescanname.jpg", ScanTableName = "EmployeePassport", TableId = 2  });
        }
    }
}

