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
    public class TestEmployeeDrivingLicensesController
    {
        [TestMethod]
        public void GetAllLicenses_ShouldReturnNotDeleted()
        {
            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense { Id = 1, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 3, EmployeeId = 3 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDrivingLicensesController(factory.Object);
            var result = controller.GetEmployeeDrivingLicenseAll() as OkNegotiatedContentResult<IEnumerable<EmployeeDrivingLicenseDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetLicensesByEmployeeId_ShouldReturnNotDeletedLicenses()
        {
            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense { Id = 1, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 3, EmployeeId = 3 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDrivingLicensesController(factory.Object);
            var result = controller.GetEmployeeDrivingLicenseByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeDrivingLicenseDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetLicense_ShouldReturnLicense()
        {
            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense { Id = 1, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 3, EmployeeId = 3 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);
            licenses.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return licenses.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDrivingLicensesController(factory.Object);
            var result = controller.GetDrivingLicense(1) as OkNegotiatedContentResult<EmployeeDrivingLicenseDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);

        }

        [TestMethod]
        public void PutLicense_ShouldReturnOk()
        {
            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense { Id = 1, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 3, EmployeeId = 3 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);
            licenses.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return licenses.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeDrivingLicense license = new EmployeeDrivingLicense { Id = 3, EmployeeId = 3, Category = "b", IssuedBy = "Wow" };
            var controller = new EmployeeDrivingLicensesController(factory.Object);
            var result = controller.PutEmployeeDrivingLicense(3, license.ToDTO()) as OkNegotiatedContentResult<EmployeeDrivingLicenseDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostLicense_ShoulAddLicense()
        {
            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense { Id = 1, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 3, EmployeeId = 3 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);
            licenses.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return licenses.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            licenses.Setup(d => d.Add(It.IsAny<EmployeeDrivingLicense>())).Returns<EmployeeDrivingLicense>((contact) =>
            {
                licensesTestData.Add(contact);
                licenses = MockHelper.MockDbSet(licensesTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = licenses.Object.Max(d => d.Id) + 1;
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

            EmployeeDrivingLicense license = new EmployeeDrivingLicense { Id = 0, EmployeeId = 3, Category = "b", IssuedBy = "Wow" };
            var controller = new EmployeeDrivingLicensesController(factory.Object);
            var result = controller.PostEmployeeDrivingLicense(license.ToDTO()) as CreatedAtRouteNegotiatedContentResult<EmployeeDrivingLicenseDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
            Assert.AreEqual("b", result.Content.Category);
            Assert.AreEqual("Wow", result.Content.IssuedBy);
        }

        [TestMethod]
        public void DeleteLicense_ShouldDeleteAndReturnOk()
        {
            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense { Id = 1, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeDrivingLicense { Id = 3, EmployeeId = 3 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);
            licenses.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return licenses.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeDrivingLicense license = new EmployeeDrivingLicense { Id = 3, EmployeeId = 3, Category = "b", IssuedBy = "Wow" };
            var controller = new EmployeeDrivingLicensesController(factory.Object);
            var result = controller.DeleteEmployeeDrivingLicense(3) as OkNegotiatedContentResult<EmployeeDrivingLicenseDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }

    }
}
