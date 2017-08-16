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
    public class TestEmployeeGunsController 
    {
        [TestMethod]
        public void GetAllDocuments_ShouldReturnNotDeleted()
        {
            var gunsTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1, EmployeeId = 2 },
                new EmployeeGun { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeGun { Id = 3, EmployeeId = 3 }
            };
            var guns = MockHelper.MockDbSet(gunsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeGuns).Returns(guns.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(guns.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeGunsController(factory.Object);
            var result = controller.GetEmployeeGunAll() as OkNegotiatedContentResult<IEnumerable<EmployeeGunDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetDocumentsByEmployeeId_ShouldReturn()
        {
            var gunsTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1, EmployeeId = 2 },
                new EmployeeGun { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeGun { Id = 3, EmployeeId = 3 }
            };
            var guns = MockHelper.MockDbSet(gunsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeGuns).Returns(guns.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(guns.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeGunsController(factory.Object);
            var result = controller.GetEmployeeGunByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeGunDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetGunById_ShouldReturn()
        {
            var gunsTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1, EmployeeId = 2 },
                new EmployeeGun { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeGun { Id = 3, EmployeeId = 3 }
            };
            var guns = MockHelper.MockDbSet(gunsTestData);
            guns.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return guns.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeGuns).Returns(guns.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(guns.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeGunsController(factory.Object);
            var result = controller.GetEmployeeGun(1) as OkNegotiatedContentResult<EmployeeGunDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var gunsTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1, EmployeeId = 2 },
                new EmployeeGun { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeGun { Id = 3, EmployeeId = 3 }
            };
            var guns = MockHelper.MockDbSet(gunsTestData);
            guns.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return guns.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeGuns).Returns(guns.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(guns.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeGun license = new EmployeeGun { Id = 3, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeGunsController(factory.Object);
            var result = controller.PutEmployeeGun(3, license.ToDTO()) as OkNegotiatedContentResult<EmployeeGunDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostGun_ShoulAddGun()
        {
            var gunsTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1, EmployeeId = 2 },
                new EmployeeGun { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeGun { Id = 3, EmployeeId = 3 }
            };
            var guns = MockHelper.MockDbSet(gunsTestData);
            guns.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return guns.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            guns.Setup(d => d.Add(It.IsAny<EmployeeGun>())).Returns<EmployeeGun>((contact) =>
            {
                gunsTestData.Add(contact);
                guns = MockHelper.MockDbSet(gunsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeGuns).Returns(guns.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(guns.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = guns.Object.Max(d => d.Id) + 1;
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

            EmployeeGun license = new EmployeeGun { Id = 0, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeGunsController(factory.Object);
            var result = controller.PostEmployeeGun(license.ToDTO()) as CreatedAtRouteNegotiatedContentResult<EmployeeGunDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
            Assert.AreEqual("Wow", result.Content.IssuedBy);
        }

        [TestMethod]
        public void DeleteGun_ShouldDeleteAndReturnOk()
        {
            var gunsTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1, EmployeeId = 2 },
                new EmployeeGun { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeGun { Id = 3, EmployeeId = 3 }
            };
            var guns = MockHelper.MockDbSet(gunsTestData);
            guns.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return guns.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeGuns).Returns(guns.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(guns.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeGun license = new EmployeeGun { Id = 3, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeGunsController(factory.Object);
            var result = controller.DeleteEmployeeGun(3) as OkNegotiatedContentResult<EmployeeGunDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }
}
