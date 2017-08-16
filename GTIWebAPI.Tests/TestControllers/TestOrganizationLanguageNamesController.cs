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
    public class TestOrganizationLanguageNamesController
    {
        [TestMethod]
        public void GetPassportsByOrganizationId_ShouldReturn()
        {
            var passportsTestData = new List<OrganizationLanguageName>()
            {
                new OrganizationLanguageName { Id = 1, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationLanguageNames).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationLanguageName>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationLanguageNamesController(factory.Object);
            var result = controller.GetOrganizationLanguageNameByOrganizationId(2) as OkNegotiatedContentResult<IEnumerable<OrganizationLanguageNameDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetPassportById_ShouldReturn()
        {
            var passportsTestData = new List<OrganizationLanguageName>()
            {
                new OrganizationLanguageName { Id = 1, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationLanguageNames).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationLanguageName>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationLanguageNamesController(factory.Object);
            var result = controller.GetOrganizationLanguageName(1) as OkNegotiatedContentResult<OrganizationLanguageNameDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.OrganizationId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var passportsTestData = new List<OrganizationLanguageName>()
            {
                new OrganizationLanguageName { Id = 1, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationLanguageNames).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationLanguageName>()).Returns(passports.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationLanguageName passport = new OrganizationLanguageName { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationLanguageNamesController(factory.Object);
            var result = controller.PutOrganizationLanguageName(3, passport.ToDTO()) as OkNegotiatedContentResult<OrganizationLanguageNameDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostPassport_ShoulAddPassport()
        {
            var passportsTestData = new List<OrganizationLanguageName>()
            {
                new OrganizationLanguageName { Id = 1, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);



            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            passports.Setup(d => d.Add(It.IsAny<OrganizationLanguageName>())).Returns<OrganizationLanguageName>((contact) =>
            {
                passportsTestData.Add(contact);
                passports = MockHelper.MockDbSet(passportsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationLanguageNames).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationLanguageName>()).Returns(passports.Object);

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
            OrganizationLanguageName passport = new OrganizationLanguageName { Id = 0, OrganizationId = 3 };
            var controller = new OrganizationLanguageNamesController(factory.Object);
            var result = controller.PostOrganizationLanguageName(passport.ToDTO()) as CreatedAtRouteNegotiatedContentResult<OrganizationLanguageNameDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }

        [TestMethod]
        public void DeletePassport_ShouldDeleteAndReturnOk()
        {
            var passportsTestData = new List<OrganizationLanguageName>()
            {
                new OrganizationLanguageName { Id = 1, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationLanguageName { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationLanguageNames).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationLanguageName>()).Returns(passports.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationLanguageName passport = new OrganizationLanguageName { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationLanguageNamesController(factory.Object);
            var result = controller.DeleteOrganizationLanguageName(3) as OkNegotiatedContentResult<OrganizationLanguageNameDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }
    }
}
