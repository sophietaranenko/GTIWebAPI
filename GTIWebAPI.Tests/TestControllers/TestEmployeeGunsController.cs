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
    public class TestEmployeeGunsController 
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeGun> repo;

        public TestEmployeeGunsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeGunsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAll_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeGunsController(repo);
            var result = controller.GetEmployeeGunAll() as OkNegotiatedContentResult<List<EmployeeGunDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeeGunsController(repo);
            var result = controller.GetEmployeeGunByEmployee(1) as OkNegotiatedContentResult<List<EmployeeGunDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeGunsController(repo);
            var result = controller.GetEmployeeGun(1) as OkNegotiatedContentResult<EmployeeGunDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new EmployeeGunsController(repo);
            EmployeeGun gun = repo.Add(GetDemo());
            var result = controller.PutEmployeeGun(gun.Id, gun) as OkNegotiatedContentResult<EmployeeGunDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeGunsController(repo);
            EmployeeGun gun = GetDemo();
            var badresult = controller.PutEmployeeGun(999, gun);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new EmployeeGunsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeGun(item) as CreatedAtRouteNegotiatedContentResult<EmployeeGunDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeGun");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            EmployeeGun gun = GetDemo();
            gun = repo.Add(gun);

            var controller = new EmployeeGunsController(repo);
            var result = controller.DeleteEmployeeGun(gun.Id) as OkNegotiatedContentResult<EmployeeGunDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(gun.Id, result.Content.Id);
        }

        private EmployeeGun GetDemo()
        {
            EmployeeGun gun = new EmployeeGun
            {
                Id = 0,
                Seria = "SS",
                EmployeeId = 1
            };
            return gun;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeGun { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeGun { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeGun { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeGun { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
