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
    public class TestEmployeeDrivingLicensesController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeDrivingLicense> repo;

        public TestEmployeeDrivingLicensesController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeDrivingLicensesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAllLicenses_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeDrivingLicensesController(repo);
            var result = controller.GetEmployeeDrivingLicenseAll() as OkNegotiatedContentResult<List<EmployeeDrivingLicenseDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetLicensesByEmployeeId_ShouldReturnNotDeletedLicenses()
        {
            var controller = new EmployeeDrivingLicensesController(repo);
            var result = controller.GetEmployeeDrivingLicenseByEmployee(1) as OkNegotiatedContentResult<List<EmployeeDrivingLicenseDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetLicenseById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeDrivingLicensesController(repo);
            var result = controller.GetDrivingLicense(1) as OkNegotiatedContentResult<EmployeeDrivingLicenseDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutLicense_ShouldReturnOk()
        {
            var controller = new EmployeeDrivingLicensesController(repo);
            EmployeeDrivingLicense car = repo.Add(GetDemo());
            var result = controller.PutEmployeeDrivingLicense(car.Id, car) as OkNegotiatedContentResult<EmployeeDrivingLicenseDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutLicense_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeDrivingLicensesController(repo);
            EmployeeDrivingLicense car = GetDemo();
            var badresult = controller.PutEmployeeDrivingLicense(999, car);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostLicense_ShouldReturnSame()
        {
            var controller = new EmployeeDrivingLicensesController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeDrivingLicense(item) as CreatedAtRouteNegotiatedContentResult<EmployeeDrivingLicenseDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetDrivingLicense");
            Assert.AreEqual(result.RouteValues["Id"], item.Id);
        }

        [TestMethod]
        public void DeleteLicense_ShouldReturnOK()
        {
            EmployeeDrivingLicense car = GetDemo();
            car = repo.Add(car);

            var controller = new EmployeeDrivingLicensesController(repo);
            var result = controller.DeleteEmployeeDrivingLicense(car.Id) as OkNegotiatedContentResult<EmployeeDrivingLicenseDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(car.Id, result.Content.Id);
        }

        private EmployeeDrivingLicense GetDemo()
        {
            EmployeeDrivingLicense car = new EmployeeDrivingLicense
            {
                Id = 0,
                Seria = "SS",
                EmployeeId = 1,
                IssuedBy = "Кем-то там выдана"
            };
            return car;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeDrivingLicense { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeDrivingLicense { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeDrivingLicense { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeDrivingLicense { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
