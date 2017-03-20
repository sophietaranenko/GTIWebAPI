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
    public class TestEmployeeInternationalPassportsController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeInternationalPassport> repo;

        public TestEmployeeInternationalPassportsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeInternationalPassportsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAllInternationalPassports_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeInternationalPassportsController(repo);
            var result = controller.GetEmployeeInternationalPassportAll() as OkNegotiatedContentResult<List<EmployeeInternationalPassportDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetInternationalPassportsByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeeInternationalPassportsController(repo);
            var result = controller.GetEmployeeInternationalPassportByEmployee(1) as OkNegotiatedContentResult<List<EmployeeInternationalPassportDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetInternationalPassportById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeInternationalPassportsController(repo);
            var result = controller.GetEmployeeInternationalPassport(1) as OkNegotiatedContentResult<EmployeeInternationalPassportDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutInternationalPassport_ShouldReturnOk()
        {
            var controller = new EmployeeInternationalPassportsController(repo);
            EmployeeInternationalPassport internationalPassport = repo.Add(GetDemo());
            var result = controller.PutEmployeeInternationalPassport(internationalPassport.Id, internationalPassport) as OkNegotiatedContentResult<EmployeeInternationalPassportDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutInternationalPassport_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeInternationalPassportsController(repo);
            EmployeeInternationalPassport internationalPassport = GetDemo();
            var badresult = controller.PutEmployeeInternationalPassport(999, internationalPassport);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostInternationalPassport_ShouldReturnSame()
        {
            var controller = new EmployeeInternationalPassportsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeInternationalPassport(item) as CreatedAtRouteNegotiatedContentResult<EmployeeInternationalPassportDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeInternationalPassport");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteInternationalPassport_ShouldReturnOK()
        {
            EmployeeInternationalPassport internationalPassport = GetDemo();
            internationalPassport = repo.Add(internationalPassport);

            var controller = new EmployeeInternationalPassportsController(repo);
            var result = controller.DeleteEmployeeInternationalPassport(internationalPassport.Id) as OkNegotiatedContentResult<EmployeeInternationalPassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(internationalPassport.Id, result.Content.Id);
        }

        private EmployeeInternationalPassport GetDemo()
        {
            EmployeeInternationalPassport internationalPassport = new EmployeeInternationalPassport
            {
                Id = 0,
                Seria = "SS",
                EmployeeId = 1
            };
            return internationalPassport;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeInternationalPassport { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeInternationalPassport { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeInternationalPassport { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeInternationalPassport { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
