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
    public class TestEmployeeInternationalPassportsController
    {
        [TestMethod]
        public void GetAllPassports_ShouldReturnNotDeleted()
        {
            var passportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport { Id = 1, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeInternationalPassportsController(factory.Object);
            var result = controller.GetEmployeeInternationalPassportAll() as OkNegotiatedContentResult<IEnumerable<EmployeeInternationalPassportDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetPassportsByEmployeeId_ShouldReturn()
        {
            var passportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport { Id = 1, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeInternationalPassportsController(factory.Object);
            var result = controller.GetEmployeeInternationalPassportByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeInternationalPassportDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetInternationalPassportById_ShouldReturn()
        {
            var passportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport { Id = 1, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeInternationalPassportsController(factory.Object);
            var result = controller.GetEmployeeInternationalPassport(1) as OkNegotiatedContentResult<EmployeeInternationalPassportDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var passportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport { Id = 1, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(passports.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeInternationalPassport passport = new EmployeeInternationalPassport { Id = 3, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeInternationalPassportsController(factory.Object);
            var result = controller.PutEmployeeInternationalPassport(3, passport.ToDTO()) as OkNegotiatedContentResult<EmployeeInternationalPassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostInternationalPassport_ShoulAddInternationalPassport()
        {
            var passportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport { Id = 1, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            passports.Setup(d => d.Add(It.IsAny<EmployeeInternationalPassport>())).Returns<EmployeeInternationalPassport>((contact) =>
            {
                passportsTestData.Add(contact);
                passports = MockHelper.MockDbSet(passportsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(passports.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = passports.Object.Max(d => d.Id) + 1;
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

            EmployeeInternationalPassport passport = new EmployeeInternationalPassport { Id = 0, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeInternationalPassportsController(factory.Object);
            var result = controller.PostEmployeeInternationalPassport(passport.ToDTO()) as CreatedAtRouteNegotiatedContentResult<EmployeeInternationalPassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
            Assert.AreEqual("Wow", result.Content.IssuedBy);
        }

        [TestMethod]
        public void DeleteInternationalPassport_ShouldDeleteAndReturnOk()
        {
            var passportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport { Id = 1, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeInternationalPassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(passports.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeInternationalPassport passport = new EmployeeInternationalPassport { Id = 3, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeInternationalPassportsController(factory.Object);
            var result = controller.DeleteEmployeeInternationalPassport(3) as OkNegotiatedContentResult<EmployeeInternationalPassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }
}
