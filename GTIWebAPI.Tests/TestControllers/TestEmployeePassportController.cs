using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests
{
    [TestClass]
    public class TestEmployeePassportsController
    {
        [TestMethod]
        public void GetAllPassports_ShouldReturnNotDeleted()
        {
            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1, EmployeeId = 2 },
                new EmployeePassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeePassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeePassportsController(factory.Object);
            var result = controller.GetEmployeePassportAll() as OkNegotiatedContentResult<IEnumerable<EmployeePassportDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetPassportsByEmployeeId_ShouldReturn()
        {
            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1, EmployeeId = 2 },
                new EmployeePassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeePassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeePassportsController(factory.Object);
            var result = controller.GetEmployeePassportByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeePassportDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetPassportById_ShouldReturn()
        {
            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1, EmployeeId = 2 },
                new EmployeePassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeePassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeePassportsController(factory.Object);
            var result = controller.GetEmployeePassport(1) as OkNegotiatedContentResult<EmployeePassportDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1, EmployeeId = 2 },
                new EmployeePassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeePassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeePassport passport = new EmployeePassport { Id = 3, EmployeeId = 3 };
            var controller = new EmployeePassportsController(factory.Object);
            var result = controller.PutEmployeePassport(3, passport.ToDTO()) as OkNegotiatedContentResult<EmployeePassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostPassport_ShoulAddPassport()
        {
            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1, EmployeeId = 2 },
                new EmployeePassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeePassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            passports.Setup(d => d.Add(It.IsAny<EmployeePassport>())).Returns<EmployeePassport>((contact) =>
            {
                passportsTestData.Add(contact);
                passports = MockHelper.MockDbSet(passportsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);

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
            EmployeePassport passport = new EmployeePassport { Id = 0, EmployeeId = 3 , Address = new Address { Apartment = "3", BuildingNumber = 45}  };
            var controller = new EmployeePassportsController(factory.Object);
            var result = controller.PostEmployeePassport(passport.ToDTO()) as CreatedAtRouteNegotiatedContentResult<EmployeePassportDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }

        [TestMethod]
        public void DeletePassport_ShouldDeleteAndReturnOk()
        {
            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1, EmployeeId = 2 },
                new EmployeePassport { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeePassport { Id = 3, EmployeeId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeePassport passport = new EmployeePassport { Id = 3, EmployeeId = 3 };
            var controller = new EmployeePassportsController(factory.Object);
            var result = controller.DeleteEmployeePassport(3) as OkNegotiatedContentResult<EmployeePassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }


}
