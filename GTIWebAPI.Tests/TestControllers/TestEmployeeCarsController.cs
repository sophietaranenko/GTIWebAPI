using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GTIWebAPI.Controllers;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestEmployeeCarsController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeCar> repo;

        public TestEmployeeCarsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeCarRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAll_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeCarsController(repo);
            var result = controller.GetEmplyeeCarAll() as OkNegotiatedContentResult<List<EmployeeCarDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeeCarsController(repo);
            var result = controller.GetEmployeeCarByEmployee(1) as OkNegotiatedContentResult<List<EmployeeCarDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeCarsController(repo);
            var result = controller.GetEmployeeCar(1) as OkNegotiatedContentResult<EmployeeCarDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new EmployeeCarsController(repo);
            EmployeeCar car = GetDemo();
            var result = controller.PutEmployeeCar(car.Id, car) as OkNegotiatedContentResult<EmployeeCarDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeCarsController(repo);
            EmployeeCar car = GetDemo();
            var badresult = controller.PutEmployeeCar(999, car);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new EmployeeCarsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeCar(item) as CreatedAtRouteNegotiatedContentResult<EmployeeCarDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeCar");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.MassInService, item.MassInService);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            EmployeeCar car = GetDemo();
            car = repo.Add(car);

            var controller = new EmployeeCarsController(repo);
            var result = controller.DeleteEmployeeCar(car.Id) as OkNegotiatedContentResult<EmployeeCarDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(car.Id, result.Content.Id);
        }

        private EmployeeCar GetDemo()
        {
            EmployeeCar car = new EmployeeCar
            {
                Id = 0,
                Seria = "SS",
                MassInService = 1100,
                EmployeeId = 1
            };
            return car;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeCar { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeCar { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeCar { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeCar { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
