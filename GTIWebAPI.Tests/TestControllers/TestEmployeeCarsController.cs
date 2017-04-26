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
using System.Data.Entity;
using Moq;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestEmployeeCarsController
    {
        [TestMethod]
        public void GetAllCars_ShouldReturnNotDeletedCars()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };
            var cars = MockHelper.MockDbSet(carsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);
            var result = controller.GetEmplyeeCarAll() as OkNegotiatedContentResult<IEnumerable<EmployeeCarDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetCarsByEmployeeId_ShouldReturnNotDeletedCars()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };
            var cars = MockHelper.MockDbSet(carsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);
            var result = controller.GetEmployeeCarByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeCarDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.FirstOrDefault().Id);
        }

        [TestMethod]
        public void GetCarById_ShouldReturnObjectWithSameId()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };
            var cars = MockHelper.MockDbSet(carsTestData);
            cars.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return cars.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);
            var result = controller.GetEmployeeCar(1) as OkNegotiatedContentResult<EmployeeCarDTO>;
            Assert.AreEqual(result.Content.Id, 1);
            Assert.AreEqual(result.Content.EmployeeId, 2);
        }

        [TestMethod]
        public void PutCar_ShouldReturnOk()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };
            var cars = MockHelper.MockDbSet(carsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);

            EmployeeCar car = new EmployeeCar { Id = 4, Capacity = 33, Deleted = null, EmployeeId = 1 };

            var result = controller.PutEmployeeCar(4, car) as OkNegotiatedContentResult<EmployeeCarDTO>;
            // EmployeeCarDTO dto = result.Content;
          
            Assert.IsNotNull(result);
            Assert.AreEqual(33, (int)result.Content.Capacity);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(1, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutCar_ShouldFail_WhenDifferentID()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };
            var cars = MockHelper.MockDbSet(carsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);

            EmployeeCar car = new EmployeeCar { Id = 1, Capacity = 333, Deleted = null, EmployeeId = 1 };
            var badresult = controller.PutEmployeeCar(999, car);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostCar_ShouldReturnSame()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };


            var cars = MockHelper.MockDbSet(carsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);

            EmployeeCar item = new EmployeeCar { Id = 0, Capacity = 333, Deleted = null, EmployeeId = 3 };
            var result = controller.PostEmployeeCar(item) as CreatedAtRouteNegotiatedContentResult<EmployeeCarDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeCar");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.Capacity, item.Capacity);
        }

        [TestMethod]
        public void DeleteCar_ShouldReturnOK()
        {
            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar { Id = 1, EmployeeId = 2 },
                new EmployeeCar { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeCar { Id = 3, EmployeeId = 3 }
            };
            object ids = 344;
            carsTestData.Find(d => d.Id == Int32.Parse(ids.ToString()));

            var cars = MockHelper.MockDbSet(carsTestData);
            cars.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return cars.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeCarsController(fac);

            var result = controller.DeleteEmployeeCar(1) as OkNegotiatedContentResult<EmployeeCarDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Id);
        }

        
    }
}
