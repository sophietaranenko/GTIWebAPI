using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
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
    public class TestOrganizationsController
    {
        private TestDbContext db;
        private IDbContextFactory factory;
        private IOrganizationsRepository repo;

        public TestOrganizationsController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new OrganizationsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetByOrganizationId_ShouldReturnNotDeletedOrganizationIdsProperties()
        {
            var controller = new OrganizationsController(repo);
            var result = controller.GetOrganizationView(444) as OkNegotiatedContentResult<OrganizationDTO>;
            Assert.AreEqual(444, result.Content.Id);
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationsController(repo);
            var result = controller.GetOrganizationEdit(444) as OkNegotiatedContentResult<OrganizationEditDTO>;
            Assert.AreEqual(result.Content.Id, 444);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new OrganizationsController(repo);
            Organization organization = repo.Add(GetDemo());
            var result = controller.PutOrganization(organization.Id, organization) as OkNegotiatedContentResult<OrganizationEditDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationsController(repo);
            Organization property = GetDemo();
            var badresult = controller.PutOrganization(999, property);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new OrganizationsController(repo);
            var item = GetDemo();
            var result = controller.PostOrganization(item) as BadRequestErrorMessageResult;
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            Organization organization = repo.Add(GetDemo());

            var controller = new OrganizationsController(repo);
            var result = controller.DeleteOrganization(organization.Id) as OkNegotiatedContentResult<OrganizationEditDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(organization.Id, result.Content.Id);
        }

        private Organization GetDemo()
        {
            Organization gun = new Organization
            {
                Id = 0
            };
            return gun;
        }

        private void GetFewDemo()
        {
            Address address = new Address
            {
                Id = 99
            };
            db.Addresses.Add(address);
            OrganizationAddress orgAddress = new OrganizationAddress()
            {
                Id = 330,
                Address = address,
                AddressId = 99,
                OrganizationId = 444
            };
            db.OrganizationAddresses.Add(orgAddress);

            Country country = new Country()
            {
                Id = 1,
                Name = "Country"
            };
            db.Countries.Add(country);

            Organization organization = new Organization()
            {
                Id = 444,
                ShortName = "Some short name",
                CountryId = 1
            };
            db.Organizations.Add(organization);

        }
    }
}
