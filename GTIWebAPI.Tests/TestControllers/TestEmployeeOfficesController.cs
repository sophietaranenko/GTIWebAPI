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
    public class TestEmployeeOfficesController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeOffice> repo;

        public TestEmployeeOfficesController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeOfficesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAll_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeOfficesController(repo);
            var result = controller.GetEmployeeOfficeAll() as OkNegotiatedContentResult<List<EmployeeOfficeDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeeOfficesController(repo);
            var result = controller.GetEmployeeOfficeByEmployeeId(1) as OkNegotiatedContentResult<List<EmployeeOfficeDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeOfficesController(repo);
            var result = controller.GetEmployeeOffice(1) as OkNegotiatedContentResult<EmployeeOfficeDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new EmployeeOfficesController(repo);
            EmployeeOffice office = repo.Add(GetDemo());
            var result = controller.PutEmployeeOffice(office.Id, office) as OkNegotiatedContentResult<EmployeeOfficeDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeOfficesController(repo);
            EmployeeOffice office = GetDemo();
            var badresult = controller.PutEmployeeOffice(999, office);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new EmployeeOfficesController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeOffice(item) as CreatedAtRouteNegotiatedContentResult<EmployeeOfficeDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeOffice");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            EmployeeOffice office = GetDemo();
            office = repo.Add(office);

            var controller = new EmployeeOfficesController(repo);
            var result = controller.DeleteEmployeeOffice(office.Id) as OkNegotiatedContentResult<EmployeeOfficeDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(office.Id, result.Content.Id);
        }

        private EmployeeOffice GetDemo()
        {
            EmployeeOffice office = new EmployeeOffice
            {
                Id = 0,
                EmployeeId = 1
            };
            return office;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeOffice { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeOffice { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeOffice { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeOffice { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
