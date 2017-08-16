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
    public class TestEmployeeLanguagesController
    {
        [TestMethod]
        public void GetAllLanguages_ShouldReturnNotDeleted()
        {
            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1, EmployeeId = 2 },
                new EmployeeLanguage { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeLanguage { Id = 3, EmployeeId = 3 }
            };
            var languages = MockHelper.MockDbSet(languagesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeLanguages).Returns(languages.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(languages.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeLanguagesController(factory.Object);
            var result = controller.GetEmployeeLanguageAll() as OkNegotiatedContentResult<IEnumerable<EmployeeLanguageDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetLanguagesByEmployeeId_ShouldReturn()
        {
            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1, EmployeeId = 2 },
                new EmployeeLanguage { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeLanguage { Id = 3, EmployeeId = 3 }
            };
            var languages = MockHelper.MockDbSet(languagesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeLanguages).Returns(languages.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(languages.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeLanguagesController(factory.Object);
            var result = controller.GetEmployeeLanguageByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeLanguageDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetLanguageById_ShouldReturn()
        {
            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1, EmployeeId = 2 },
                new EmployeeLanguage { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeLanguage { Id = 3, EmployeeId = 3 }
            };
            var languages = MockHelper.MockDbSet(languagesTestData);
            languages.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return languages.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeLanguages).Returns(languages.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(languages.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeLanguagesController(factory.Object);
            var result = controller.GetEmployeeLanguage(1) as OkNegotiatedContentResult<EmployeeLanguageDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1, EmployeeId = 2 },
                new EmployeeLanguage { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeLanguage { Id = 3, EmployeeId = 3 }
            };
            var languages = MockHelper.MockDbSet(languagesTestData);
            languages.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return languages.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeLanguages).Returns(languages.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(languages.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeLanguage passport = new EmployeeLanguage { Id = 3, EmployeeId = 3};
            var controller = new EmployeeLanguagesController(factory.Object);
            var result = controller.PutEmployeeLanguage(3, passport.ToDTO()) as OkNegotiatedContentResult<EmployeeLanguageDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostLanguage_ShoulAddLanguage()
        {
            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1, EmployeeId = 2 },
                new EmployeeLanguage { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeLanguage { Id = 3, EmployeeId = 3 }
            };
            var languages = MockHelper.MockDbSet(languagesTestData);
            languages.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return languages.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            languages.Setup(d => d.Add(It.IsAny<EmployeeLanguage>())).Returns<EmployeeLanguage>((contact) =>
            {
                languagesTestData.Add(contact);
                languages = MockHelper.MockDbSet(languagesTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeLanguages).Returns(languages.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(languages.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = languages.Object.Max(d => d.Id) + 1;
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

            EmployeeLanguage passport = new EmployeeLanguage { Id = 0, EmployeeId = 3};
            var controller = new EmployeeLanguagesController(factory.Object);
            var result = controller.PostEmployeeLanguage(passport.ToDTO()) as CreatedAtRouteNegotiatedContentResult<EmployeeLanguageDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }

        [TestMethod]
        public void DeleteLanguage_ShouldDeleteAndReturnOk()
        {
            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1, EmployeeId = 2 },
                new EmployeeLanguage { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeLanguage { Id = 3, EmployeeId = 3 }
            };
            var languages = MockHelper.MockDbSet(languagesTestData);
            languages.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return languages.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeLanguages).Returns(languages.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(languages.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeLanguage passport = new EmployeeLanguage { Id = 3, EmployeeId = 3};
            var controller = new EmployeeLanguagesController(factory.Object);
            var result = controller.DeleteEmployeeLanguage(3) as OkNegotiatedContentResult<EmployeeLanguageDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }
}
