using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void Post_ShouldReturnBadRequest()
        {
            var controller = new EmployeeDocumentScansController(repo);
            
            var item = GetDemo();
            var result = controller.UploadEmployeeDocumentScan("EmployeePassport", 1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
       }


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

