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
    public class TestOrganizationTaxAddressesController
    {
        private IDbContextFactory factory;
        private IOrganizationRepository<OrganizationTaxAddress> repo;

        public TestOrganizationTaxAddressesController()
        {
            factory = new TestDbContextFactory();
            repo = new OrganizationTaxAddressesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetTaxAddressesByOrganizationId_ShouldReturnNotDeletedOrganizationIdsAddresses()
        {
            var controller = new OrganizationTaxAddressesController(repo);
            var result = controller.GetOrganizationTaxAddressByOrganizationId(1) as OkNegotiatedContentResult<List<OrganizationTaxAddressDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetTaxAddressById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationTaxAddressesController(repo);
            var result = controller.GetOrganizationTaxAddress(1) as OkNegotiatedContentResult<OrganizationTaxAddressDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutTaxAddress_ShouldReturnOk()
        {
            var controller = new OrganizationTaxAddressesController(repo);
            OrganizationTaxAddress address = repo.Add(GetDemo());
            var result = controller.PutOrganizationTaxAddress(address.Id, address) as OkNegotiatedContentResult<OrganizationTaxAddressDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutTaxAddress_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationTaxAddressesController(repo);
            OrganizationTaxAddress address = GetDemo();
            var badresult = controller.PutOrganizationTaxAddress(999, address);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostTaxAddress_ShouldReturnSame()
        {
            var controller = new OrganizationTaxAddressesController(repo);
            var item = GetDemo();
            var result = controller.PostOrganizationTaxAddress(item) as CreatedAtRouteNegotiatedContentResult<OrganizationTaxAddressDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetOrganizationTaxAddress");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteTaxAddress_ShouldReturnOK()
        {
            OrganizationTaxAddress gun = repo.Add(GetDemo());

            var controller = new OrganizationTaxAddressesController(repo);
            var result = controller.DeleteOrganizationTaxAddress(gun.Id) as OkNegotiatedContentResult<OrganizationTaxAddressDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(gun.Id, result.Content.Id);
        }

        private OrganizationTaxAddress GetDemo()
        {
            OrganizationTaxAddress gun = new OrganizationTaxAddress
            {
                Id = 0,
                OrganizationId = 1,
                AddressId = 0,
                Address = new Models.Dictionary.Address() { Id = 1 }
            };
            return gun;
        }

        private void GetFewDemo()
        {
            repo.Add(new OrganizationTaxAddress { Id = 0, Deleted = true, OrganizationId = 1, AddressId = 0, Address = new Models.Dictionary.Address { Id = 1 } });
            repo.Add(new OrganizationTaxAddress { Id = 0, Deleted = false, OrganizationId = 1, AddressId = 0, Address = new Models.Dictionary.Address() { Id = 1}  });
            repo.Add(new OrganizationTaxAddress { Id = 0, Deleted = false, OrganizationId = 2, AddressId = 0, Address = new Models.Dictionary.Address() { Id = 1 } });
            repo.Add(new OrganizationTaxAddress { Id = 0, Deleted = false, OrganizationId = 2, AddressId = 0, Address = new Models.Dictionary.Address() { Id = 1 } });
        }
    }
}
