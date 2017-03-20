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
    public class TestOrganizationPropertiesController
    {
        private IDbContextFactory factory;
        private IOrganizationRepository<OrganizationProperty> repo;

        public TestOrganizationPropertiesController()
        {
            factory = new TestDbContextFactory();
            repo = new OrganizationPropertiesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetPropertiesByOrganizationId_ShouldReturnNotDeletedOrganizationIdsProperties()
        {
            var controller = new OrganizationPropertiesController(repo);
            var result = controller.GetOrganizationPropertyByOrganizationId(1) as OkNegotiatedContentResult<List<OrganizationPropertyDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetPropertyById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationPropertiesController(repo);
            var result = controller.GetOrganizationProperty(1) as OkNegotiatedContentResult<OrganizationPropertyDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutProperty_ShouldReturnOk()
        {
            var controller = new OrganizationPropertiesController(repo);
            OrganizationProperty property = repo.Add(GetDemo());
            var result = controller.PutOrganizationProperty(property.Id, property) as OkNegotiatedContentResult<OrganizationPropertyDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutProperty_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationPropertiesController(repo);
            OrganizationProperty property = GetDemo();
            var badresult = controller.PutOrganizationProperty(999, property);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostProperty_ShouldReturnSame()
        {
            var controller = new OrganizationPropertiesController(repo);
            var item = GetDemo();
            var result = controller.PostOrganizationProperty(item) as CreatedAtRouteNegotiatedContentResult<OrganizationPropertyDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetOrganizationProperty");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteProperty_ShouldReturnOK()
        {
            OrganizationProperty property = repo.Add(GetDemo());

            var controller = new OrganizationPropertiesController(repo);
            var result = controller.DeleteOrganizationProperty(property.Id) as OkNegotiatedContentResult<OrganizationPropertyDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(property.Id, result.Content.Id);
        }

        private OrganizationProperty GetDemo()
        {
            OrganizationProperty gun = new OrganizationProperty
            {
                Id = 0,
                OrganizationId = 1
            };
            return gun;
        }

        private void GetFewDemo()
        {
            repo.Add(new OrganizationProperty { Id = 1, Deleted = true, OrganizationId = 1 } );
            repo.Add(new OrganizationProperty { Id = 2, Deleted = false, OrganizationId = 1 } );
            repo.Add(new OrganizationProperty { Id = 3, Deleted = false, OrganizationId = 2 });
            repo.Add(new OrganizationProperty { Id = 4, Deleted = false, OrganizationId = 2 });
        }
    }
}
