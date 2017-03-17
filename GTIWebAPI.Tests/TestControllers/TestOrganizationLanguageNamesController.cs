using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository.Organization;
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
    public class TestOrganizationLanguageNamesController
    {
        private IDbContextFactory factory;
        private IOrganizationRepository<OrganizationLanguageName> repo;

        public TestOrganizationLanguageNamesController()
        {
            factory = new TestDbContextFactory();
            repo = new OrganizationLanguageNamesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetByOrganizationId_ShouldReturnNotDeletedOrganizationIdsLanguageNames()
        {
            var controller = new OrganizationLanguageNamesController(repo);
            var result = controller.GetOrganizationLanguageNameByOrganizationId(1) as OkNegotiatedContentResult<List<OrganizationLanguageNameDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationLanguageNamesController(repo);
            var result = controller.GetOrganizationLanguageName(1) as OkNegotiatedContentResult<OrganizationLanguageNameDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new OrganizationLanguageNamesController(repo);
            OrganizationLanguageName languageName = repo.Add(GetDemo());
            var result = controller.PutOrganizationLanguageName(languageName.Id, languageName) as OkNegotiatedContentResult<OrganizationLanguageNameDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationLanguageNamesController(repo);
            OrganizationLanguageName languageName = GetDemo();
            var badresult = controller.PutOrganizationLanguageName(999, languageName);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new OrganizationLanguageNamesController(repo);
            var item = GetDemo();
            var result = controller.PostOrganizationLanguageName(item) as CreatedAtRouteNegotiatedContentResult<OrganizationLanguageNameDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetOrganizationLanguageName");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            OrganizationLanguageName languageName = repo.Add(GetDemo());

            var controller = new OrganizationLanguageNamesController(repo);
            var result = controller.DeleteOrganizationLanguageName(languageName.Id) as OkNegotiatedContentResult<OrganizationLanguageNameDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(languageName.Id, result.Content.Id);
        }

        private OrganizationLanguageName GetDemo()
        {
            OrganizationLanguageName gun = new OrganizationLanguageName
            {
                Id = 0,
                OrganizationId = 1
            };
            return gun;
        }

        private void GetFewDemo()
        {
            repo.Add(new OrganizationLanguageName { Id = 1, Deleted = true, OrganizationId = 1, LanguageId = 1 } );
            repo.Add(new OrganizationLanguageName { Id = 2, Deleted = false, OrganizationId = 1, LanguageId = 2 });
            repo.Add(new OrganizationLanguageName { Id = 3, Deleted = false, OrganizationId = 2, LanguageId = 1 });
            repo.Add(new OrganizationLanguageName { Id = 4, Deleted = false, OrganizationId = 2, LanguageId = 2 });
        }
    }
}
