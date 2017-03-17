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
    public class TestOrganizationGTILinksController
    {
        private IDbContextFactory factory;
        private IOrganizationRepository<OrganizationGTILink> repo;

        public TestOrganizationGTILinksController()
        {
            factory = new TestDbContextFactory();
            repo = new OrganizationGTILinksRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetByOrganizationId_ShouldReturnNotDeletedOrganizationIdsGTILinks()
        {
            var controller = new OrganizationGTILinksController(repo);
            var result = controller.GetOrganizationGTILinkByOrganizationId(1) as OkNegotiatedContentResult<List<OrganizationGTILinkDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationGTILinksController(repo);
            var result = controller.GetOrganizationGTILink(1) as OkNegotiatedContentResult<OrganizationGTILinkDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new OrganizationGTILinksController(repo);
            var item = GetDemo();
            //with no principal bad request 
            var result = controller.PostOrganizationGTILink(item) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            OrganizationGTILink link = repo.Add(GetDemo());

            var controller = new OrganizationGTILinksController(repo);
            var result = controller.DeleteOrganizationGTILink(link.Id) as OkNegotiatedContentResult<OrganizationGTILinkDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(link.Id, result.Content.Id);
        }

        private OrganizationGTILink GetDemo()
        {
            OrganizationGTILink gun = new OrganizationGTILink
            {
                Id = 0,
                OrganizationId = 3,
                GTIId = 5
            };
            return gun;
        }

        private void GetFewDemo()
        {
            repo.Add(new OrganizationGTILink { Id = 1, Deleted = true, OrganizationId = 1, GTIId = 1 });
            repo.Add(new OrganizationGTILink { Id = 2, Deleted = false, OrganizationId = 1, GTIId = 2 });
            repo.Add(new OrganizationGTILink { Id = 3, Deleted = false, OrganizationId = 2, GTIId = 3 });
            repo.Add(new OrganizationGTILink { Id = 4, Deleted = false, OrganizationId = 2, GTIId = 4 });
        }
    }
}
