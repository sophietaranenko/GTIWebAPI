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
    public class TestEmployeesController
    {
        private IDbContextFactory factory;
        private IEmployeesRepository repo;
        private TestDbContext db;

        public TestEmployeesController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new EmployeesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAll_ShouldReturnNotDeleted()
        {
            var controller = new EmployeesController(repo);
            string officeIds = "1,2,4";
            var result = controller.GetEmployeeAll(officeIds) as OkNegotiatedContentResult<List<EmployeeViewDTO>>;
            Assert.AreEqual(4, result.Content.Count());
        }

        [TestMethod]
        public void GetView_ShouldReturnObjectContainsOtherObjects()
        {
            var controller = new EmployeesController(repo);
            var result = controller.GetEmployeeView(1) as OkNegotiatedContentResult<EmployeeDTO>;
            Assert.AreEqual(result.Content.Id, 1);
            Assert.IsNotNull(result.Content.EmployeePassports);
            Assert.IsNotNull(result.Content.EmployeePassports.Take(1).FirstOrDefault().Address);
        }

        [TestMethod]
        public void GetView_ShouldReturnObjectWithSameIdAndContainsSomeStuff()
        {
            var controller = new EmployeesController(repo);
            var result = controller.GetEmployeeEdit(1) as OkNegotiatedContentResult<EmployeeEditDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new EmployeesController(repo);
            Employee employee = repo.Add(GetDemo());
            var result = controller.PutEmployee(employee.Id, employee) as OkNegotiatedContentResult<EmployeeEditDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeesController(repo);
            Employee car = GetDemo();
            var badresult = controller.PutEmployee(999, car);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new EmployeesController(repo);
            var item = GetDemo();
            var result = controller.PostEmployee(item) as CreatedAtRouteNegotiatedContentResult<EmployeeEditDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeEdit");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            Employee employee = repo.Add(GetDemo());

            var controller = new EmployeesController(repo);
            var result = controller.DeleteEmployee(employee.Id) as OkNegotiatedContentResult<EmployeeEditDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Id, result.Content.Id);
        }

        private Employee GetDemo()
        {
            Employee employee = new Employee();
            employee.Id = 101;
            employee.AddressId = 220;
            Models.Dictionary.Address employeeAddress = new Models.Dictionary.Address()
            {
                Id = 220,
                Apartment = "1"
            };
            employee.Address = employeeAddress;
            Models.Dictionary.Address passportAddress = new Models.Dictionary.Address()
            {
                Id = 330,
                Apartment = "1"
            };

            employee.EmployeePassports = new List<EmployeePassport>();
            employee.EmployeePassports.Add(new EmployeePassport()
                {
                    EmployeeId = 101,
                    FirstName = "First Name",
                    SecondName = "Secons Name",
                    AddressId = 330,
                    Address = passportAddress
                });

            return employee;
        }

        private void GetFewDemo()
        {
            db.EmployeeViews.Add(
                new EmployeeView()
                {
                    DateOfBirth = new DateTime(1999, 1, 1),
                    Id = 50,
                    Position = "some position"
                });
            db.EmployeeViews.Add(
                new EmployeeView()
                {
                    DateOfBirth = new DateTime(1999, 1, 1),
                    Id = 20,
                    Position = "some position"
                });
            db.EmployeeViews.Add(
                new EmployeeView()
                {
                    DateOfBirth = new DateTime(1999, 1, 1),
                    Id = 30,
                    Position = "some position"
                });
            db.EmployeeViews.Add(
                new EmployeeView()
                {
                    DateOfBirth = new DateTime(1999, 1, 1),
                    Id = 40,
                    Position = "some position"
                });

            Employee employee = new Employee();
            employee.Id = 1;
            employee.AddressId = 22;
            Models.Dictionary.Address employeeAddress = new Models.Dictionary.Address()
            {
                Id = 22,
                Apartment = "1"
            };
            employee.Address = employeeAddress;

           
            Models.Dictionary.Address passportAddress = new Models.Dictionary.Address()
            {
                Id = 33,
                Apartment = "1"
            };

            employee.EmployeePassports = new List<EmployeePassport>();
            

            db.Addresses.Add(employeeAddress);
            db.Addresses.Add(passportAddress);

            db.EmployeePassports.Add(
                new EmployeePassport()
                {
                    EmployeeId = 1,
                    FirstName = "First Name",
                    SecondName = "Secons Name",
                    AddressId = 33,
                    Address = passportAddress
                });

            db.Employees.Add(employee);

        }
    }
}
