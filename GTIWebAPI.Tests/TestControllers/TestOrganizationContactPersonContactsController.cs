using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
    public class TestOrganizationContactPersonContactsController
    {
        [TestMethod]
        public void GetPassportsByOrganizationId_ShouldReturn()
        {
            var passportsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1, OrganizationContactPersonId = 2},
                new OrganizationContactPersonContact { Id = 2, Deleted = true, OrganizationContactPersonId = 2 },
                new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationContactPersonContactsController(factory.Object);
            var result = controller.GetOrganizationContactPersonContactByOrganizationContactPersonId(2) as OkNegotiatedContentResult<IEnumerable<OrganizationContactPersonContactDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetPassportById_ShouldReturn()
        {
            var passportsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1, OrganizationContactPersonId = 2},
                new OrganizationContactPersonContact { Id = 2, Deleted = true, OrganizationContactPersonId = 2 },
                new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationContactPersonContactsController(factory.Object);
            var result = controller.GetOrganizationContactPersonContact(1) as OkNegotiatedContentResult<OrganizationContactPersonContactDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.OrganizationContactPersonId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var passportsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1, OrganizationContactPersonId = 2},
                new OrganizationContactPersonContact { Id = 2, Deleted = true, OrganizationContactPersonId = 2 },
                new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(passports.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationContactPersonContact passport = new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 };
            var controller = new OrganizationContactPersonContactsController(factory.Object);
            var result = controller.PutOrganizationContactPersonContact(3, passport.ToDTO()) as OkNegotiatedContentResult<OrganizationContactPersonContactDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostPassport_ShoulAddPassport()
        {
            var passportsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1, OrganizationContactPersonId = 2},
                new OrganizationContactPersonContact { Id = 2, Deleted = true, OrganizationContactPersonId = 2 },
                new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            passports.Setup(d => d.Add(It.IsAny<OrganizationContactPersonContact>())).Returns<OrganizationContactPersonContact>((contact) =>
            {
                passportsTestData.Add(contact);
                passports = MockHelper.MockDbSet(passportsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(passports.Object);

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
            OrganizationContactPersonContact passport = new OrganizationContactPersonContact { Id = 0, OrganizationContactPersonId = 3 };
            var controller = new OrganizationContactPersonContactsController(factory.Object);
            var result = controller.PostOrganizationContactPersonContact(passport.ToDTO()) as CreatedAtRouteNegotiatedContentResult<OrganizationContactPersonContactDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationContactPersonId);
        }

        [TestMethod]
        public void DeletePassport_ShouldDeleteAndReturnOk()
        {
            var passportsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1, OrganizationContactPersonId = 2},
                new OrganizationContactPersonContact { Id = 2, Deleted = true, OrganizationContactPersonId = 2 },
                new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(passports.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationContactPersonContact passport = new OrganizationContactPersonContact { Id = 3, OrganizationContactPersonId = 3 };
            var controller = new OrganizationContactPersonContactsController(factory.Object);
            var result = controller.DeleteOrganizationContactPersonContact(3) as OkNegotiatedContentResult<OrganizationContactPersonContactDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationContactPersonId);
        }
    }
}
