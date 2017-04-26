using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        [TestMethod]
        public void GetAllOffices_ShouldReturnNotDeleted()
        {
            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1, EmployeeId = 2 },
                new EmployeeOffice { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeOffice { Id = 3, EmployeeId = 3 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeOfficesController(factory.Object);
            var result = controller.GetEmployeeOfficeAll() as OkNegotiatedContentResult<IEnumerable<EmployeeOfficeDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetOfficesByEmployeeId_ShouldReturn()
        {
            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1, EmployeeId = 2 },
                new EmployeeOffice { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeOffice { Id = 3, EmployeeId = 3 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeOfficesController(factory.Object);
            var result = controller.GetEmployeeOfficeByEmployeeId(2) as OkNegotiatedContentResult<IEnumerable<EmployeeOfficeDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetOfficeById_ShouldReturn()
        {
            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1, EmployeeId = 2 },
                new EmployeeOffice { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeOffice { Id = 3, EmployeeId = 3 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);
            offices.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return offices.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeOfficesController(factory.Object);
            var result = controller.GetEmployeeOffice(1) as OkNegotiatedContentResult<EmployeeOfficeDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1, EmployeeId = 2 },
                new EmployeeOffice { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeOffice { Id = 3, EmployeeId = 3 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);
            offices.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return offices.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeOffice passport = new EmployeeOffice { Id = 3, EmployeeId = 3 };
            var controller = new EmployeeOfficesController(factory.Object);
            var result = controller.PutEmployeeOffice(3, passport) as OkNegotiatedContentResult<EmployeeOfficeDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostOffice_ShoulAddOffice()
        {
            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1, EmployeeId = 2 },
                new EmployeeOffice { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeOffice { Id = 3, EmployeeId = 3 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);
            offices.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return offices.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            offices.Setup(d => d.Add(It.IsAny<EmployeeOffice>())).Returns<EmployeeOffice>((contact) =>
            {
                officesTestData.Add(contact);
                offices = MockHelper.MockDbSet(officesTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = offices.Object.Max(d => d.Id) + 1;
                       list.Add(i);
                   }
                   else
                   {
                       list.Add(0);
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeOffice passport = new EmployeeOffice { Id = 0, EmployeeId = 3 };
            var controller = new EmployeeOfficesController(factory.Object);
            var result = controller.PostEmployeeOffice(passport) as CreatedAtRouteNegotiatedContentResult<EmployeeOfficeDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }

        [TestMethod]
        public void DeleteOffice_ShouldDeleteAndReturnOk()
        {
            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1, EmployeeId = 2 },
                new EmployeeOffice { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeOffice { Id = 3, EmployeeId = 3 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);
            offices.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return offices.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeOffice passport = new EmployeeOffice { Id = 3, EmployeeId = 3 };
            var controller = new EmployeeOfficesController(factory.Object);
            var result = controller.DeleteEmployeeOffice(3) as OkNegotiatedContentResult<EmployeeOfficeDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }
}
