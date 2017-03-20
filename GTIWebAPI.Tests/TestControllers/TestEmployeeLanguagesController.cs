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
    public class TestEmployeeLanguagesController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeLanguage> repo;

        public TestEmployeeLanguagesController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeLanguagesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAllLanguages_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeLanguagesController(repo);
            var result = controller.GetEmployeeLanguageAll() as OkNegotiatedContentResult<List<EmployeeLanguageDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetLanguagesByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeeLanguagesController(repo);
            var result = controller.GetEmployeeLanguageByEmployee(1) as OkNegotiatedContentResult<List<EmployeeLanguageDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetLanguageById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeLanguagesController(repo);
            var result = controller.GetEmployeeLanguage(1) as OkNegotiatedContentResult<EmployeeLanguageDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutLanguage_ShouldReturnOk()
        {
            var controller = new EmployeeLanguagesController(repo);
            EmployeeLanguage language = repo.Add(GetDemo());
            var result = controller.PutEmployeeLanguage(language.Id, language) as OkNegotiatedContentResult<EmployeeLanguageDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutLanguage_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeLanguagesController(repo);
            EmployeeLanguage language = GetDemo();
            var badresult = controller.PutEmployeeLanguage(999, language);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostLanguage_ShouldReturnSame()
        {
            var controller = new EmployeeLanguagesController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeLanguage(item) as CreatedAtRouteNegotiatedContentResult<EmployeeLanguageDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeLanguage");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteLanguage_ShouldReturnOK()
        {
            EmployeeLanguage language = GetDemo();
            language = repo.Add(language);

            var controller = new EmployeeLanguagesController(repo);
            var result = controller.DeleteEmployeeLanguage(language.Id) as OkNegotiatedContentResult<EmployeeLanguageDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(language.Id, result.Content.Id);
        }

        private EmployeeLanguage GetDemo()
        {
            EmployeeLanguage language = new EmployeeLanguage
            {
                Id = 0,
                EmployeeId = 1
            };
            return language;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeLanguage { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeLanguage { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeLanguage { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeLanguage { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
